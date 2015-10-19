using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptBoardCorner : MonoBehaviour {

    public ScriptEngine engine;
    public ScriptPlayer owner = null;
    public bool roadUp;
    public List<ScriptBoardHex> adjacentHexes = new List<ScriptBoardHex>(0);
    public List<ScriptBoardEdge> adjacentRoads = new List<ScriptBoardEdge>(0);

    void Start()
    {
        engine = GameObject.Find("GameEngine").GetComponent<ScriptEngine>();
    }

    public bool CheckValidBuild()
    {
        foreach(ScriptBoardEdge road in adjacentRoads)
        {
            if (road.owner == engine.players[0])
            {
                owner = engine.players[0];
                return true;
            }
        }
        return false;
    }

    public void GainResources(int checkValue)
    {
        foreach (ScriptBoardHex hex in adjacentHexes)
        {
            if (hex.hexDieValue == checkValue)
            {
                switch(hex.resource)
                {
                    case HexType.BRICK:
                        owner.AddBricks(1);
                        break;
                    case HexType.GRAIN:
                        owner.AddWheat(1);
                        break;
                    case HexType.WOOD:
                        owner.AddLumber(1);
                        break;
                    case HexType.WOOL:
                        owner.AddWool(1);
                        break;
                }
            }
        }
    }
}
