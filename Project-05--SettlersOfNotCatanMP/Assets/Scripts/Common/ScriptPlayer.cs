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

    public int NumSettlements { get; set; }

	public int OldSettlementNum { get; set; }

    public ScriptPlayer(string Name)
    {
		settlements = new List<GameObject> ();
		roads = new List<GameObject> ();

        PlayerName = Name;
        NumSettlements = 0;
		OldSettlementNum = 0;
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


	public void GainResources(int diceRoll)
	{
		foreach(GameObject settlement in settlements)
		{
			settlement.GetComponent<ScriptBoardCorner>().GainResources(diceRoll);
		}
		
	}
	void Update()
	{
		if (isLocalPlayer) {
			NumSettlements = settlements.Count;

			if(NumSettlements != settlements.Count)
			{
				Debug.Log ("Sending command to server...Player " 
				           + PlayerName + " has " + settlements.Count);
				for(int i = OldSettlementNum; i < settlements.Count ; i++)
				{
					CmdSendSettlementsToServer(settlements[i]);
				}

				OldSettlementNum = settlements.Count;
			}
		}
	}

	[Command]
	void CmdSendSettlementsToServer(GameObject newSettlement)
	{
		Debug.Log ("This will run ONLY on the server.\n" +
			"Adding a new settlement to the list");
		settlements.Add (newSettlement);

		//how settlements are activated, added, etc. is added here.

		OldSettlementNum = settlements.Count;
		Debug.Log ("New Settlement Count for Player" + PlayerName + " is " + settlements.Count);

		RpcClientSettlementUpdate (newSettlement);
	}

	[ClientRpc]
	void RpcClientSettlementUpdate(GameObject newSettlement)
	{
		//how settlements are activated, added, etc. is added here.
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