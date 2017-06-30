using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyPanel;

        public InputField ipInput;

        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);

        }
        public void OnClickInstructions() // Laat Instructies zien
        {
            lobbyManager.ShowInstructions();
        }
        public void OnClickSettings() // Laat Instellingen zien
        {
            lobbyManager.ShowSettings();
        }
        public void OnClickHost() // Start met hosten
        {
            lobbyManager.StartHost();
        }

        public void OnClickJoin() // Join een lobby
        {
            lobbyManager.ChangeTo(lobbyPanel); // Verander naar de lobby UI

            lobbyManager.networkAddress = ipInput.text; // IP is de input
            lobbyManager.StartClient();

            lobbyManager.backDelegate = lobbyManager.StopClientClbk; // Stop met client zijn als je op back drukt
            lobbyManager.DisplayIsConnecting(); // Laat melding zien dat hij aan het connecten is

            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
        }

        void onEndEditIP(string text) // Als je klaar bent met een IP invoeren
        {
            if (Input.GetKeyDown(KeyCode.Return)) // Als je op Enter drukt
            {
                OnClickJoin(); // Join
            }
        }

    }
}
