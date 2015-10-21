using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// @author Mike Dobson
/// </summary>

public class ScriptPlayer : NetworkBehaviour {

    [SyncVar]
    public List<GameObject> settlements;
    [SyncVar]
    public List<GameObject> roads;

    public List<GameObject> newSettlements;

    public List<GameObject> newRoads;  

    public bool EndTurn = false;
    
    public void GainResources(int diceRoll)
    {
        foreach(GameObject settlement in settlements)
        {
            settlement.GetComponent<ScriptBoardCorner>().GainResources(diceRoll);
        }
    }
    
    public int NumLumber
    {
        get;
        private set;
    }

    public int NumWool
    {
        get;
        private set;
    }

    public int NumWheat
    {
        get;
        private set;
    }

    public int NumBrick
    {
        get;
        private set;
    }

    public string PlayerName
    {
        get;
        set;
    }

    public int NumSettlements
    {
        get;
        set;
    }

    public ScriptPlayer(string Name)
    {
        PlayerName = Name;
        NumSettlements = 0;
        NumBrick = 0;
        NumLumber = 0;
        NumWheat = 0;
        NumWool = 0;
    }

    public void AddBricks(int number)
    {
        NumBrick += number;
    }

    public void RemoveBricks(int number)
    {
        NumBrick -= number;
    }

    public void AddWheat(int number)
    {
        NumWheat += number;
    }

    public void RemoveWheat(int number)
    {
        NumWheat -= number;
    }

    public void AddWool(int number)
    {
        NumWool += number;
    }

    public void RemoveWool(int number)
    {
        NumWool -= number;
    }

    public void AddLumber(int number)
    {
        NumLumber += number;
    }

    public void RemoveLumber(int number)
    {
        NumLumber -= number;
    }

    [Client]
    public void TransmitRoads(List<GameObject> roadListToSync)
    {
        roads = roadListToSync;
    }

    [Client]
    public void TransmitSettlements(List<GameObject> settlementListToSync)
    {
        settlements = settlementListToSync;
    }
}