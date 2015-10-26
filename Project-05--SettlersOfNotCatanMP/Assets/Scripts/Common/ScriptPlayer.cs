using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// @author Mike Dobson. additions by Victor Haskins.
/// </summary>

public class ScriptPlayer : NetworkBehaviour {

    public List<GameObject> settlements;

    public List<GameObject> roads;

    public List<GameObject> newSettlements;

    public List<GameObject> newRoads;  

    public bool EndTurn = false;

    [HideInInspector]
    public int curPlayer = 0;

    public int NumSettlements { get; set; }
    
    public int NumLumber { get; private set; }

    public int NumWool { get; private set; }

    public int NumWheat { get; private set; }

    public int NumBrick { get; private set; }

    public string PlayerName { get; set; }
    //added for use with multiplayer.
    public Color PlayerColor { get; set; }

    public ScriptPlayer(string Name)
    {
		settlements = new List<GameObject> ();
		roads = new List<GameObject> ();

        PlayerName = Name;
        NumSettlements = 0;
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

    //added by Victor Haskins
    //available for use with Trade phase to update resources.
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

    void Start()
    {
        GameObject.Find("GameEngine").GetComponent<ScriptEngine>().curPlayer = curPlayer;
    }


    //function added by Victor Haskins
    //adds a list of new settlements to the original list.
	[Command]
	public void CmdSendSettlementsToServer()
	{
		Debug.Log ("This will run ONLY on the server.\n" +
			"Adding a new settlement(s) to the list");
        foreach (GameObject newSettle in newSettlements)
        {
            //if the owner of the list item is the script checking it...
            if (newSettle.GetComponent<ScriptBoardCorner>().owner == (this))
            {
                //add to main list
                settlements.Add(newSettle);
                //call recipient function so all other screens can be updated.
                RpcClientSettlementUpdate(newSettle);
            }
            else
                Debug.Log("Settlement placement denied. Another Player owns it.");
        }
        //remove every individual node from the temp list
        int itemsToRemove = newSettlements.Count;
        for (int i = 0; i < itemsToRemove; i++)
        {
            newSettlements.RemoveAt(0);
        }
        //updates settlement count for win condition check
        NumSettlements = settlements.Count;
        Debug.Log("New Settlement Count for Player" + 
            PlayerName + " is " + settlements.Count);

    }

    //function added by Victor Haskins
    [ClientRpc]
	void RpcClientSettlementUpdate(GameObject newSettlement)
	{
        //how settlements are activated, added, etc. is added here.
        //for all other players...
        if (!isLocalPlayer)
        {
            //find settlement array on gameboard
            GameObject board = GameObject.Find("GameBoard");
            ScriptBoardManager boardManager = board.GetComponent<ScriptBoardManager>();
            //check each item in list...
            foreach (GameObject child in boardManager.settlements)
            {
                //if the array item matches the settlement parameter passed in
                //set the owner to the respective player.
                if (child == newSettlement)
                {
                    child.GetComponent<ScriptBoardCorner>().owner =
                        newSettlement.GetComponent<ScriptBoardCorner>().owner;
                }
            }
        }
    }

    //function added by Victor Haskins
    [Command]
    public void CmdSendRoadsToServer()
    {
        Debug.Log("This will run ONLY on the server.\n" +
            "Adding a new road to the list");
        foreach (GameObject newRoad in newRoads)
        {
            //for each new road in the temp roads list to add,
            //if the owner is the the one checking it...
            if (newRoad.GetComponent<ScriptBoardEdge>().owner == (this))
            {
                //add to main list and...
                roads.Add(newRoad);
                //send to all other players' screens
                RpcClientRoadUpdate(newRoad);
            }
            else
                Debug.Log("Road placement denied. Another Player owns it.");
        }
        //and finally remove all elements from the temp list.
        int itemsToRemove = newRoads.Count;
        for (int i = 0; i < itemsToRemove; i++)
        {
            newRoads.RemoveAt(0);
        }
        Debug.Log("New Road Count for Player" + PlayerName + " is " + roads.Count);

    }

    //function added by Victor Haskins
    [ClientRpc]
    void RpcClientRoadUpdate(GameObject newRoad)
    {
        //for all other players...
        if(!isLocalPlayer)
        {
            //find settlement array held by game board.
            GameObject board = GameObject.Find("GameBoard");
            ScriptBoardManager boardManager = board.GetComponent<ScriptBoardManager>();
            //for each road in the array
            foreach (GameObject child in boardManager.roads)
            {
                //if the child road matches the parameter road...
                if (child == newRoad)
                {
                    //set the road's owner to the appropriate player.
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