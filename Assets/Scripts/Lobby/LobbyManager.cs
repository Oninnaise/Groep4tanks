using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;


namespace Prototype.NetworkLobby
{
    public class LobbyManager : NetworkLobbyManager 
    {
        static short MsgKicked = MsgType.Highest + 1;

        static public LobbyManager s_Singleton;


        [Header("UI Lobby")] // Decoratief voor de editor
        [Tooltip("Tijd voor de countdown")] // Decoratief voor de editor
        public float prematchCountdown = 5.0f;

        [Space] // Decoratief voor de editor
        [Header("UI Referenties")] // Decoratief voor de editor
        public LobbyTopPanel topPanel;

        public RectTransform mainMenuPanel;
        public RectTransform lobbyPanel;
        public RectTransform InstructionsPanel;
        public RectTransform InstructionsPanelPhone;
        public RectTransform SettingsPanel;

        public LobbyInfoPanel infoPanel;
        public LobbyCountdownPanel countdownPanel;
        public GameObject addPlayerButton;

        protected RectTransform currentPanel;

        public Button backButton;

        public Text statusInfo;
        public Text hostInfo;

        
        [HideInInspector]  // Decoratief voor de editor
        public int _playerNumber = 0; // Tellen van aantal spelers

        [HideInInspector]  // Decoratief voor de editor
        public bool _isMatchmaking = false; // Gebruikt voor als de client een spel verlaat

        protected bool _disconnectServer = false; // Gebruikt voor als de client een spel verlaat

        protected ulong _currentMatchID; // Gebruikt voor als de client een spel verlaat

        protected LobbyHook _lobbyHooks; // Gebruikt voor als de client een spel verlaat

        void Start()
        {
            s_Singleton = this;
            _lobbyHooks = GetComponent<Prototype.NetworkLobby.LobbyHook>();
            currentPanel = mainMenuPanel; // Laat de hoofdpaneel zien

            backButton.gameObject.SetActive(false); // Verwijder de back knop
            GetComponent<Canvas>().enabled = true; // Zet de Canvas op aan

            DontDestroyOnLoad(gameObject); // Geen gameObjects verwijderen bij het wisselen van scenes

            SetServerInfo("Offline", "None"); // Default Server Info
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn) // Als de scene veranderd voor een client
        {
            if (SceneManager.GetSceneAt(0).name == lobbyScene) // Als het de lobby scene is
            {
                if (topPanel.isInGame) // Als je in game bent
                {
                    ChangeTo(lobbyPanel); // Ga terug naar de lobby
                    if (_isMatchmaking)
                    {
                        if (conn.playerControllers[0].unetView.isServer)
                        {
                            backDelegate = StopHostClbk;
                        }
                        else
                        {
                            backDelegate = StopClientClbk;
                        }
                    }
                    else
                    {
                        if (conn.playerControllers[0].unetView.isClient) // Doe een check op type client
                        {
                            backDelegate = StopHostClbk; // Stop met hosten
                        }
                        else
                        {
                            backDelegate = StopClientClbk; // Stop met client zijn
                        }
                    }
                }
                else
                {
                    ChangeTo(mainMenuPanel); // Ga naar Main Menu
                }

                topPanel.ToggleVisibility(true);
                topPanel.isInGame = false;
            }
            else
            {
                ChangeTo(null);

                Destroy(GameObject.Find("MainMenuUI(Clone)")); // Garbage Collection

                //backDelegate = StopGameClbk;
                topPanel.isInGame = true;
                topPanel.ToggleVisibility(false);
            }
        }

        public void ChangeTo(RectTransform newPanel) // Verander Panel
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }

            currentPanel = newPanel;

            if (currentPanel != mainMenuPanel)
            {
                backButton.gameObject.SetActive(true);
            }
            else
            {
                backButton.gameObject.SetActive(false);
                SetServerInfo("Offline", "None"); // Default info
                _isMatchmaking = false;
            }
        }

        public void DisplayIsConnecting() // Toon dat er een connectie gemaakt wordt
        {
            var _this = this;
            infoPanel.Display("Connecting...", "Cancel", () => { _this.backDelegate(); });
        }

        public void SetServerInfo(string status, string host) // Verander de server info
        {
            statusInfo.text = status;
            hostInfo.text = host;
        }


        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;
        public void GoBackButton()
        {
            backDelegate();
			topPanel.isInGame = false;
        }

        public void AddLocalPlayer() // Voeg een speler toe
        {
            TryToAddPlayer();
        }

        public void RemovePlayer(LobbyPlayer player) // Verwijder een speler
        {
            player.RemovePlayer();
        }

        public void SimpleBackClbk() // Ga terug knop voor als je geen connectie hebt gemaakt
        {
            ChangeTo(mainMenuPanel);
        }

        public void ShowInstructions() // Laat verschillende instructies voor telefoons en desktops zien
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                ChangeTo(InstructionsPanelPhone);
            }
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                ChangeTo(InstructionsPanel);
            }
            backDelegate = SimpleBackClbk;
        }

        public void Quit() // Quit
        {
            Application.Quit();
        }

       public void ShowSettings() // Laat de settings zien
        {
                ChangeTo(SettingsPanel);

            backDelegate = SimpleBackClbk;
        }
              
           
        public void StopHostClbk() // Stop met hosten
        {
            if (_isMatchmaking)
            {
				matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
				_disconnectServer = true;
            }
            else
            {
                StopHost();
            }

            
            ChangeTo(mainMenuPanel);
        }

        public void StopClientClbk() // Stop met client zijn
        {
            StopClient();

            if (_isMatchmaking)
            {
                StopMatchMaker();
            }

            ChangeTo(mainMenuPanel);
        }

        public void StopServerClbk() // Server stoppen
        {
            StopServer();
            ChangeTo(mainMenuPanel);
        }

        class KickMsg : MessageBase { }
        public void KickPlayer(NetworkConnection conn) // Kick Player - niet geimplementeerd -
        {
            conn.Send(MsgKicked, new KickMsg());
        }




        public void KickedMessageHandler(NetworkMessage netMsg) // Wat de speler ziet als hij gekickt is - niet geimplementeerd -
        {
            infoPanel.Display("Kicked by Server", "Close", null);
            netMsg.conn.Disconnect();
        }

        //===================

        public override void OnStartHost() // Als je start met hosten 
        {
            base.OnStartHost();

            ChangeTo(lobbyPanel);
            if (!(backDelegate == StopHostClbk))
            {
                backDelegate = StopHostClbk;
            }
            SetServerInfo("Hosting", networkAddress);
        }

		public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) // Maken van een matchmaking server - niet geimplementeerd -
		{
			base.OnMatchCreate(success, extendedInfo, matchInfo);
            _currentMatchID = (System.UInt64)matchInfo.networkId;
		}

		public override void OnDestroyMatch(bool success, string extendedInfo) // Stopzetten van een matchmaking server - niet geimplementeerd -
		{
			base.OnDestroyMatch(success, extendedInfo);
			if (_disconnectServer)
            {
                StopMatchMaker();
                StopHost();
            }
        }

        public void OnPlayersNumberModified(int count) // Toevoegen en verwijderen van lokale spelers
        {
            _playerNumber += count;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && _playerNumber < maxPlayers);
        }


        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId) // Bij het aanmaken van de lobby als host
        {
            GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject; // Elke speler is een LobbyPlayerPrefab

            LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
            newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers); // Je kunt jezelf op ready zetten als er genoeg spelers zijn


            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }

            return obj;
        }

        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId) // Als iemand uit de server gaat
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn) // Als de server uit de lucht gaat
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers >= minPlayers);
                }
            }

        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) // Als een client in de lobby zit
        {

            if (_lobbyHooks)
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer); // Dit wordt gebruikt om data tussen de lobby en spel te sturen

            return true;
        }

        public override void OnLobbyServerPlayersReady() // Als iedereen op Ready staat
        {
			bool allready = true;
			for(int i = 0; i < lobbySlots.Length; ++i)
			{
				if(lobbySlots[i] != null)
					allready &= lobbySlots[i].readyToBegin;
			}

			if(allready)
				StartCoroutine(ServerCountdownCoroutine()); // Start met countdown
        }

        public IEnumerator ServerCountdownCoroutine() // Countdown coroutine
        {
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime) // Om flooden te voorkomen wordt er alleen een bericht gestuurd als het op een hele getal staat.
                {
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {
                            (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(0);
                }
            }

            ServerChangeScene(playScene); // Ga naar de play scene
        }

        public override void OnClientConnect(NetworkConnection conn) // Als de client connect
        {
            base.OnClientConnect(conn);

            infoPanel.gameObject.SetActive(false);

            conn.RegisterHandler(MsgKicked, KickedMessageHandler);

            if (!NetworkServer.active)
            {
                ChangeTo(lobbyPanel);
                backDelegate = StopClientClbk;
                SetServerInfo("Client", networkAddress);
            }
        }


        public override void OnClientDisconnect(NetworkConnection conn) // Als de client disconnect
        {
            base.OnClientDisconnect(conn);
            ChangeTo(mainMenuPanel); // Ga terug naar het menu
        }

        public override void OnClientError(NetworkConnection conn, int errorCode) // Als er een error is
        {
            ChangeTo(mainMenuPanel);
            infoPanel.Display("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
        }
    }
}
