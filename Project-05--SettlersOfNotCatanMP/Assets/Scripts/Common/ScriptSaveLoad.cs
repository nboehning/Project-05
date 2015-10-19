using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// @author Marshall Mason
/// </summary>
public class ScriptSaveLoad : MonoBehaviour
{
    public List<ScriptBoardHex> hexes;
    public Text playerInput;
    ScriptEngine engine;
    public static bool LoadGame = false;
    public static string LoadLocation = null;
    void Start()
    {
        engine = this.gameObject.GetComponent<ScriptEngine>();
        if (LoadGame)
        {
            Load();
            LoadGame = false;
            LoadLocation = null;
        }
    }


    public void Save()
    {
        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.dataPath + "/Save_Data/" + playerInput.text + "/gameInformation.txt"))
        {
            writer.WriteLine(engine.CurrentState);
            //loop through hexes
            writer.WriteLine(hexes.Count);
            foreach (ScriptBoardHex hex in hexes)
            {
                //export type and value
                writer.Write(hex.resource + " ");
                writer.WriteLine(hex.hexDieValue);
                writer.WriteLine(hex.transform.position.x + "," + hex.transform.position.y + "'" + 
                                 hex.transform.position.z);
            }
            

            //loop through players
            writer.WriteLine(engine.players.Count);
            foreach (ScriptPlayer player in engine.players)
            {
                writer.Write(player.NumLumber + ",");
                writer.Write(player.NumBrick + ",");
                writer.Write(player.NumWheat + ",");
                writer.Write(player.NumWool + ",");
                writer.Write(player.NumSettlements + ",");
                writer.Write(player.roads.Count + ",");
                writer.WriteLine(player.PlayerName);
                
                foreach (GameObject settlement in player.settlements)
                {
                    writer.WriteLine(settlement.transform.position.x
                                    + "," + settlement.transform.position.y
                                    + "," + settlement.transform.position.z);
                }

                foreach (GameObject road in player.roads)
                {
                    writer.WriteLine(road.transform.position.x
                                    + "," + road.transform.position.y
                                    + "," + road.transform.position.z);
                }
            }
        }
    }

    public void Load()
    {
        if (LoadLocation == null)
        {
            Debug.Log("No Load Location");
        }
        else
        {
            using(System.IO.StreamReader reader = new System.IO.StreamReader(Application.dataPath + "/Save_Game/" + LoadLocation + "/gameInformation.text"))
            {
                switch (reader.ReadLine())
                {
                    case "PHASE1":
                        engine.LoadTransition(GameState.PHASE1, "goto phase 1");
                        break;
                    case "PHASE2":
                        engine.LoadTransition(GameState.PHASE2, "goto phase 2");
                        break;
                    case "PHASE3":
                        engine.LoadTransition(GameState.PHASE3, "goto phase 3");
                        break;
                    case "PHASE4":
                        engine.LoadTransition(GameState.PHASE4, "goto phase 4");
                        break;
                }
                
                int numHexes = System.Convert.ToInt32(reader.ReadLine());
                for (int i = 0; i < numHexes; i++)
                {
                    string[] hexValues = reader.ReadLine().Split(' ');
                    string[] hexCoords = reader.ReadLine().Split(',');
                    float hexX = System.Convert.ToSingle(hexCoords[0]);
                    float hexY = System.Convert.ToSingle(hexCoords[1]);
                    float hexZ = System.Convert.ToSingle(hexCoords[2]);

                    Vector3 hexPos = new Vector3(hexX, hexY, hexZ);
                    Collider[] hexes = Physics.OverlapSphere(hexPos, .01f);
                    foreach (Collider hex in hexes)
                    {
                        ScriptBoardHex hexScript = hex.GetComponent<ScriptBoardHex>();
                        switch (hexValues[0])
                        {
                            case "WOOD":
                                hexScript.resource = HexType.WOOD;
                                break;
                            case "WOOL":
                                hexScript.resource = HexType.WOOL;
                                break;
                            case "BRICK":
                                hexScript.resource = HexType.BRICK;
                                break;
                            case "GRAIN":
                                hexScript.resource = HexType.GRAIN;
                                break;
                        }
                        hexScript.hexDieValue = System.Convert.ToInt32(hexValues[1]);
                    }
                }

                int numPlayers = System.Convert.ToInt32(reader.ReadLine());
                for (int i = 0; i < numPlayers; i++)
                {
                    //writer.Write(player.NumLumber + ",");
                    //writer.Write(player.NumBrick + ",");
                    //writer.Write(player.NumWheat + ",");
                    //writer.Write(player.NumWool + ",");
                    //writer.Write(player.NumSettlements + ",");
                    //writer.Write(player.roads.Count + ",");
                    //writer.WriteLine(player.PlayerName);
                    string[] playerStats = reader.ReadLine().Split(',');
                    ScriptPlayer tempPlayer = new ScriptPlayer(playerStats[6]);
                    tempPlayer.AddLumber(System.Convert.ToInt32(playerStats[0]));
                    tempPlayer.AddBricks(System.Convert.ToInt32(playerStats[1]));
                    tempPlayer.AddWheat(System.Convert.ToInt32(playerStats[2]));
                    tempPlayer.AddWool(System.Convert.ToInt32(playerStats[3]));
                    tempPlayer.NumSettlements = System.Convert.ToInt32(playerStats[4]);
                    int numRoads = System.Convert.ToInt32(playerStats[5]);

                    //foreach (GameObject settlement in player.settlements)
                    //{
                    //    writer.WriteLine(settlement.transform.position.x
                    //                    + "," + settlement.transform.position.y
                    //                    + "," + settlement.transform.position.z);
                    //}
                    for (int j = 0; j < tempPlayer.NumSettlements; j++)
                    {
                        string[] cornerCoords = reader.ReadLine().Split(',');
                        float cornerX = System.Convert.ToSingle(cornerCoords[0]);
                        float cornerY = System.Convert.ToSingle(cornerCoords[1]);
                        float cornerZ = System.Convert.ToSingle(cornerCoords[2]);

                        Vector3 cornerPos = new Vector3(cornerX, cornerY, cornerZ);
                        Collider[] corners = Physics.OverlapSphere(cornerPos, .01f);
                        foreach (Collider corner in corners)
                        {
                            ScriptBoardCorner cornerScript = corner.GetComponent<ScriptBoardCorner>();
                            cornerScript.owner = tempPlayer;
                        }
                    }

                    //foreach (GameObject road in player.roads)
                    //{
                    //    writer.WriteLine(road.transform.position.x
                    //                    + "," + road.transform.position.y
                    //                    + "," + road.transform.position.z);
                    //}
                    for (int j = 0; j < numRoads; j++)
                    {
                        string[] roadCoords = reader.ReadLine().Split(',');
                        float roadX = System.Convert.ToSingle(roadCoords[0]);
                        float roadY = System.Convert.ToSingle(roadCoords[1]);
                        float roadZ = System.Convert.ToSingle(roadCoords[2]);

                        Vector3 roadPos = new Vector3(roadX, roadY, roadZ);
                        Collider[] roads = Physics.OverlapSphere(roadPos, .01f);
                        foreach (Collider road in roads)
                        {
                            ScriptBoardEdge roadScript = road.GetComponent<ScriptBoardEdge>();
                            roadScript.owner = tempPlayer;
                        }
                    }
                }
            }
        }
    }
}
