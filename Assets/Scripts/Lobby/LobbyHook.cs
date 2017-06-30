using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



namespace Prototype.NetworkLobby
{
    public abstract class LobbyHook : MonoBehaviour // Basisklasse om variabelen van de speler vanuit de lobby naar het spel te krijgen
    {
        public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) { }
    }

}
