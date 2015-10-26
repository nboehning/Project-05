using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Network
{
    public class CatanLobbyHook : LobbyHook
    {
        // @author: Nathan
        // Links the lobby to the game scene
        public override void OnLobbyServerSceneLoadedForPlayer(UnityEngine.Networking.NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            LobbyPlayer l = lobbyPlayer.GetComponent<LobbyPlayer>();

            ScriptPlayer player = gamePlayer.GetComponent<ScriptPlayer>();

            player.PlayerName = l.name;
            player.PlayerColor = l.playerColor;
            //player.color = l.playerColor;
            //paddle.playerName = l.playerName;

            //PongManager.AddPlayer(paddle);
        }
    }
}