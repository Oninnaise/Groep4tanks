using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Prototype.NetworkLobby
{
    public class LobbyPlayerList : MonoBehaviour
    {
        public static LobbyPlayerList _instance = null;

        public RectTransform playerListContentTransform;
        public GameObject warningDirectPlayServer;
        public Transform addButtonRow;

        protected VerticalLayoutGroup _layout;
        protected List<LobbyPlayer> _players = new List<LobbyPlayer>();

        public void OnEnable()
        {
            _instance = this;
            _layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
        }

        public void DisplayDirectServerWarning(bool enabled)
        {
            if(warningDirectPlayServer != null)
                warningDirectPlayServer.SetActive(enabled);
        }

        void Update() // De layout elke frame refreshen voor updates
        {
            
            if(_layout)
                _layout.childAlignment = Time.frameCount%2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
        }

        public void AddPlayer(LobbyPlayer player) // Speler toevoegen
        {
            if (_players.Contains(player))
                return;

            _players.Add(player);

            player.transform.SetParent(playerListContentTransform, false);
            addButtonRow.transform.SetAsLastSibling(); // Zet de nieuwe speler onderin

            PlayerListModified(); // Aangeven dat de lijst is aangepast
        }

        public void RemovePlayer(LobbyPlayer player) // Verwijder speler
        {
            _players.Remove(player);
            PlayerListModified(); // Aangeven dat de lijst is aangepast
        }

        public void PlayerListModified() // Wanneer de lijst aangepast is
        {
            int i = 0;
            foreach (LobbyPlayer p in _players)
            {
                p.OnPlayerListChanged(i);
                ++i;
            }
        }
    }
}
