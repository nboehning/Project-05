using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// @author Mike Dobson
/// </summary>

public class ScriptPlayer : NetworkBehaviour {

   // [SyncVar]
    public List<GameObject> settlements;
   // [SyncVar]
    public List<GameObject> roads;

    public List<GameObject> newSettlements;

    public List<GameObject> newRoads;  

    public bool EndTurn = false;
    

    
    public int NumLumber { get; private set; }

    public int NumWool { get; private set; }

    public int NumWheat { get; private set; }

    public int NumBrick { get; private set; }

    public string PlayerName { get; set; }

    public Color PlayerColor { get; set; }

    public ScriptPlayer(string Name)
    {
		settlements = new List<GameObject> ();
		roads = new List<GameObject> ();

        PlayerName = Name;
        NumBrick = 0;
        NumLumber = 0;
        NumWheat = 0;
        NumWool = 0;
    }

    public void AddBricks(int number)    { NumBrick += number; }
    public void RemoveBricks(int number) { NumBrick -= number; }

    public void AddWheat(int number)    { NumWheat += number; }
    public void RemoveWheat(int number) { NumWheat -= number; }

    public void AddWool(int number)    { NumWool += number; }
    public void RemoveWool(int number) { NumWool -= number; }

    public void AddLumber(int number)    { NumLumber += number; }
    public void RemoveLumber(int number) { NumLumber -= number; }

    [ClientRpc]
    public void RpcSetColor(Color newColor)
    {
        PlayerColor = newColor;
    }

    [Command]
    public void CmdUpdateResources(int newBrick, int newWheat, int newWool, int newLumber)
    {
        NumBrick = newBrick;
        NumWheat = newWheat;
        NumWool = newWool;
        NumLumber = newLumber;
    }



	public void GainResources(int diceRoll)
	{
		foreach(GameObject settlement in settlements)
		{
			settlement.GetComponent<ScriptBoardCorner>().GainResources(diceRoll);
		}
		
	}
    /*
	void Update()
	{
		if (isLocalPlayer) {
			NumSettlements = settlements.Count;

			if(NumSettlements != settlements.Count)
			{
				Debug.Log ("Sending command to server...Player " 
				           + PlayerName + " has " + settlements.Count + " settlements.");
				for(int i = OldSettlementNum; i < settlements.Count ; i++)
				{
					CmdSendSettlementsToServer(settlements[i]);
				}

				OldSettlementNum = settlements.Count;
			}

            if(NumRoads != roads.Count)
            {
                Debug.Log("Sending command to server...Player "
                           + PlayerName + " has " + roads.Count + " roads");
                for (int i = OldRoadNum; i < roads.Count; i++)
                {
                    CmdSendRoadsToServer();
                }

                OldRoadNum = roads.Count;
            }
		}
	}
    */

	[Command]
	public void CmdSendSettlementsToServer()
	{
		Debug.Log ("This will run ONLY on the server.\n" +
			"Adding a new settlement to the list");
        foreach (GameObject newSettle in newSettlements)
        {
            if (newSettle.GetComponent<ScriptBoardCorner>().owner == (this))
            {
                settlements.Add(newSettle);
                RpcClientSettlementUpdate(newSettle);
            }
            else
                Debug.Log("Settlement placement denied. Another Player owns it.");
        }

        int itemsToRemove = newSettlements.Count;
        for (int i = 0; i < itemsToRemove; i++)
        {
            newSettlements.RemoveAt(0);
        }
        Debug.Log("New Road Count for Player" + PlayerName + " is " + roads.Count);

    }

    [ClientRpc]
	void RpcClientSettlementUpdate(GameObject newSettlement)
	{
        //how settlements are activated, added, etc. is added here.
        if (!isLocalPlayer)
        {
            GameObject board = GameObject.Find("GameBoard");
            ScriptBoardManager boardManager = board.GetComponent<ScriptBoardManager>();
            foreach (GameObject child in boardManager.settlements)
            {
                if (child == newSettlement)
                {
                    child.GetComponent<ScriptBoardCorner>().owner =
                        newSettlement.GetComponent<ScriptBoardCorner>().owner;
                }
            }
        }
    }

    [Command]
    public void CmdSendRoadsToServer()
    {
        Debug.Log("This will run ONLY on the server.\n" +
            "Adding a new road to the list");
        foreach (GameObject newRoad in newRoads)
        {
            if (newRoad.GetComponent<ScriptBoardEdge>().owner == (this))
            {
                roads.Add(newRoad);
                RpcClientRoadUpdate(newRoad);
            }
            else
                Debug.Log("Road placement denied. Another Player owns it.");
        }

        int itemsToRemove = newRoads.Count;
        for (int i = 0; i < itemsToRemove; i++)
        {
            newRoads.RemoveAt(0);
        }
        Debug.Log("New Road Count for Player" + PlayerName + " is " + roads.Count);

    }

    [ClientRpc]
    void RpcClientRoadUpdate(GameObject newRoad)
    {
        if(!isLocalPlayer)
        {
            GameObject board = GameObject.Find("GameBoard");
            ScriptBoardManager boardManager = board.GetComponent<ScriptBoardManager>();
            foreach (GameObject child in boardManager.roads)
            {
                if (child == newRoad)
                {
                    child.GetComponent<ScriptBoardEdge>().owner =
                        newRoad.GetComponent<ScriptBoardEdge>().owner;
                }
            }
        }
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