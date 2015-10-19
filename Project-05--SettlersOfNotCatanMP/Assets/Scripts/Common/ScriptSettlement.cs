using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @author Mike Dobson
/// </summary>

public class ScriptSettlement : MonoBehaviour {

    public List<GameObject> Hexes;
    public List<GameObject> Roads;

    public ScriptPlayer player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ScriptPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CheckAdjacentHexes(int diceRoll)
    {
        for(int i = 0; i < Hexes.Count; i++)
        {
            if(Hexes[i].GetComponent<ScriptBoardHex>().hexDieValue == diceRoll)
            {
                HexType resource = Hexes[i].GetComponent<ScriptBoardHex>().resource;
                switch(resource)
                {
                    case HexType.BRICK:
                        player.AddBricks(1);
                        break;
                    case HexType.GRAIN:
                        player.AddWheat(1);
                        break;
                    case HexType.WOOD:
                        player.AddLumber(1);
                        break;
                    case HexType.WOOL:
                        player.AddWool(1);
                        break;
                }
            }
        }
    }

    public bool CheckAdjacentRoads()
    {
        foreach(GameObject road in player.roads)
        {
            for(int i = 0; i < Roads.Count; i++)
            {
                if(Roads[i] == road)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
