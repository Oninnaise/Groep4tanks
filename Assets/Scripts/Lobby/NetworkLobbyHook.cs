using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook // Wordt gebruikt om data van de lobbyspeler naar de gamespeler over te zetten
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        PlayerMove player = gamePlayer.GetComponent<PlayerMove>();

        player.color = lobby.playerColor;
    }
}
