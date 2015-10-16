using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TradeUIController : MonoBehaviour 
{
    //Player Resources
    public int playerLumber, playerSheep, playerWheat, playerBrick;

    //Button Edited Variables
    public static int tradeLumber, tradeSheep, tradeWheat, tradeBrick;
    public static int recieveLumber, recieveSheep, recieveWheat, recieveBrick;

    public static bool validTrade = false;
    public TradeUIButtons buttons;

    //Text Objects
    public Text tradeLumberText, tradeSheepText, tradeWheatText, tradeBrickText;
    public Text recieveLumberText, recieveSheepText, recieveWheatText, recieveBrickText;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateUI();
        TestTrade();
    }

    void UpdateUI()
    {
        //Gifting text control
        tradeLumberText.text = "Lumber: " + tradeLumber;
        tradeSheepText.text = "Sheeps: " + tradeSheep;
        tradeWheatText.text = "Wheat: " + tradeWheat;
        tradeBrickText.text = "Brick: " + tradeBrick;

        //Reciving text control
        recieveLumberText.text = "Lumber: " + recieveLumber;
        recieveSheepText.text = "Sheeps: " + recieveSheep;
        recieveWheatText.text = "Wheat: " + recieveWheat;
        recieveBrickText.text = "Brick: " + recieveBrick;
    }

    public void TestTrade()
    {
        //Temp Variables
        bool validLumber = true;
        bool validSheep = true;
        bool validWheat = true;
        bool validBrick = true;

        if (tradeLumber > playerLumber)
        {
            tradeLumberText.color = Color.red;
            validLumber = false;
        }
        else 
        {
            tradeLumberText.color = Color.black;
        }

        if (tradeSheep > playerSheep)
        {
            tradeSheepText.color = Color.red;
            validSheep = false;
        }
        else
        {
            tradeSheepText.color = Color.black;
        }

        if (tradeWheat> playerWheat)
        {
            tradeWheatText.color = Color.red;
            validWheat = false;
        }
        else
        {
            tradeWheatText.color = Color.black;
        }

        if (tradeBrick > playerBrick)
        {
            tradeBrickText.color = Color.red;
            validBrick = false;
        }
        else
        {
            tradeBrickText.color = Color.black;
        }

        if (!validLumber || !validSheep || !validWheat || !validBrick)
        {
            validTrade = false;
            buttons.sendRequest.interactable = false;
        }
        else
        {
            buttons.sendRequest.interactable = true;
        }
    }
}
