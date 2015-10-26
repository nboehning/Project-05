using UnityEngine;
using System.Collections;

public class ScriptButtons : MonoBehaviour
{

    public void _BtnStartGame()
    {
        Application.LoadLevel("SceneGame");
    }

    public void _BtnLoadLobby()
    {
        Application.LoadLevel("SceneLobby");
    }

    public void _BtnMainMenu()
    {
        Application.LoadLevel("SceneMenu");
    }
}
