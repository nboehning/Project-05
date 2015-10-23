using UnityEngine;
using System.Collections;
using UnityStandardAssets.Network;

public class SettlersOfNotCatanLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(UnityEngine.Networking.NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer l = lobbyPlayer.GetComponent<LobbyPlayer>();

        ScriptPlayer paddle = gamePlayer.GetComponent<ScriptPlayer>();

        PongManager.AddPlayer(paddle);
    }
}
