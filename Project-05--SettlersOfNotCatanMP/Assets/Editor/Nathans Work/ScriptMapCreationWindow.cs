using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptMapCreationWindow : EditorWindow
{

    // Hex map variables
    private ScriptHex[][] hexMap = new ScriptHex[15][];
    private int selectedRow = 7;
    private int selectedColumn = 7;


    // Drawing of map variables
    private int numRows = 15;
    private int hexEdgeLength = 40;
    private Rect labelPosition;
    private float xOffset = 45f;
    private float yOffset = 200f;
    private string labelText;
    private Vector2 hexCenter;
    private const float MAGIC_EDGE_DISTANCE_CONVERT = 0.86602540378f;   //Mathf.Sqrt(0.75f) & -Sin(Mathf.PI * 4 / 3) (apparently) Marshallllllll

    // Hex data variables
    string[] intPopupString = { "1", "2", "3", "4", "5", "6" };
    int[] intPopup = { 1, 2, 3, 4, 5, 6 };

    private string[] hexTypeOptions = {"Wood", "Grain", "Wool", "Brick", "None"};
    private int hexTypeIndex;
    
    // Variables for heuristics
    private int numUnsetType = 1;
    private int numUnsetRollNum = 1;
    private int numWool;
    private int numWood;
    private int numBrick;
    private int numWheat;
    private int numOnes;
    private int numTwos;
    private int numThrees;
    private int numFours;
    private int numFives;
    private int numSixes;

    [MenuItem("Tools/Create New Map")]
    private static void Init()
    {
        // Get existing open window or if none, make a new one:
        ScriptMapCreationWindow window = (ScriptMapCreationWindow)EditorWindow.GetWindow(typeof(ScriptMapCreationWindow));
        window.position = new Rect(100, 50, 1250, 1000);
        window.maxSize = new Vector2(1250, 1000);
        window.minSize = window.maxSize;
        window.InitializeVariables();
        window.Show();
    }

    void OnGUI()
    {
        xOffset = 265f;
        yOffset = 50f;

        GUIStyle style = EditorStyles.label;

        style.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.84f), GUILayout.Height(position.height - 50f));

        // @author: MARSHALL AND HIS MATH GODLINESS, plus nathan

        #region Map
        // Done so that it will create the vertical field to correct height/width
        EditorGUILayout.LabelField("");

        // Loop through and draw all of the hexes
        for (int i = 0; i < numRows; i++)
        {
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    for (int j = 0; j < 8 + i; j++)
                    {
                        labelText = string.Format("({0},{1})\n", i, j) + hexMap[i][j].hexType.ToString().ToLower() + "\n" + hexMap[i][j].hexNum;
                        labelPosition = new Rect(xOffset, yOffset, 40f, 40f);
                        hexCenter.x = xOffset + 20f;
                        hexCenter.y = yOffset + 13f;

                        if (hexMap[i][j].isActive)
                        {
                            #region data change check
                            // Check to modify the number of each dice roll if it's been changed
                            if (hexMap[i][j].oldHexNum != hexMap[i][j].hexNum)
                            {
                                switch (hexMap[i][j].hexNum)
                                {
                                    case 1:
                                        numOnes++;
                                       break;
                                    case 2:
                                        numTwos++;
                                        break;
                                    case 3:
                                        numThrees++;
                                        break;
                                    case 4:
                                        numFours++;
                                        break;
                                    case 5:
                                        numFives++;
                                        break;
                                    case 6:
                                        numSixes++;
                                        break;
                                }
                                switch (hexMap[i][j].oldHexNum)
                                {
                                    case 1:

                                        numOnes--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 2:
                                        numTwos--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 3:
                                        numThrees--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 4:
                                        numFours--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 5:
                                        numFives--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    case 6:
                                        numSixes--;
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        break;
                                    default:
                                        hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                        numUnsetRollNum--;
                                        break;
                                }
                            }

                            // Check to modify the number of the hex types if it's been changed
                            if (hexMap[i][j].hexType != hexMap[i][j].prevHexType)
                            {
                                switch (hexMap[i][j].hexType)
                                {
                                    case HexType.BRICK:
                                        numBrick++;
                                        break;
                                    case HexType.GRAIN:
                                        numWheat++;
                                        break;
                                    case HexType.WOOD:
                                        numWood++;
                                        break;
                                    case HexType.WOOL:
                                        numWool++;
                                        break;
                                }
                                switch (hexMap[i][j].prevHexType)
                                {
                                    case HexType.BRICK:
                                        numBrick--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    case HexType.GRAIN:
                                        numWheat--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    case HexType.WOOD:
                                        numWood--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    case HexType.WOOL:
                                        numWool--;
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        break;
                                    default:
                                        hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                        numUnsetType--;
                                        break;

                                }
                            }
                            #endregion
                            
                            if (hexMap[i][j].hexType != HexType.NONE)
                            {
                                TurnOnActive(i, j);
                            }
                            if (GUI.Button(labelPosition, labelText, style))
                            {

                                selectedRow = i;
                                selectedColumn = j;

                            }
                        }
                        else
                        {
                            EditorGUI.LabelField(labelPosition, string.Format("({0},{1})\n", i, j));
                        }

                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        #region draw the hex
                        // Vertical lines

                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.black, 1f, false);
                        }


                        // Bottom of hex
                        hex.hexCorners[0].x += 17f;
                        hex.hexCorners[1].x += 17f;
                        hex.hexCorners[2].x += 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.black, 1f, false);
                        }

                        // Top of hex
                        hex.hexCorners[3].x -= 17f;
                        hex.hexCorners[4].x -= 17f;
                        hex.hexCorners[5].x -= 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.black, 1f, false);
                        }

                        #endregion

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (i + 8);
                    xOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset += (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * MAGIC_EDGE_DISTANCE_CONVERT - 1;
                    break;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    for (int j = 0; j < 22 - i; j++)
                    {
                        labelText = string.Format("({0},{1})\n", i, j) + hexMap[i][j].hexType.ToString().ToLower() + "\n" + hexMap[i][j].hexNum;
                        labelPosition = new Rect(xOffset, yOffset, 40f, 40f);
                        hexCenter.x = xOffset + 20f;
                        hexCenter.y = yOffset + 13f;

                        #region data change check
                        // Check to modify the number of each dice roll if it's been changed
                        if (hexMap[i][j].oldHexNum != hexMap[i][j].hexNum)
                        {
                            switch (hexMap[i][j].hexNum)
                            {
                                case 1:
                                    numOnes++;
                                    break;
                                case 2:
                                    numTwos++;
                                    break;
                                case 3:
                                    numThrees++;
                                    break;
                                case 4:
                                    numFours++;
                                    break;
                                case 5:
                                    numFives++;
                                    break;
                                case 6:
                                    numSixes++;
                                    break;
                            }
                            switch (hexMap[i][j].oldHexNum)
                            {
                                case 1:

                                    numOnes--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 2:
                                    numTwos--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 3:
                                    numThrees--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 4:
                                    numFours--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 5:
                                    numFives--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                case 6:
                                    numSixes--;
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    break;
                                default:
                                    hexMap[i][j].oldHexNum = hexMap[i][j].hexNum;
                                    numUnsetRollNum--;
                                    break;
                            }
                        }

                        // Check to modify the number of the hex types if it's been changed
                        if (hexMap[i][j].hexType != hexMap[i][j].prevHexType)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    numBrick++;
                                    break;
                                case HexType.GRAIN:
                                    numWheat++;
                                    break;
                                case HexType.WOOD:
                                    numWood++;
                                    break;
                                case HexType.WOOL:
                                    numWool++;
                                    break;
                            }
                            switch (hexMap[i][j].prevHexType)
                            {
                                case HexType.BRICK:
                                    numBrick--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                case HexType.GRAIN:
                                    numWheat--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                case HexType.WOOD:
                                    numWood--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                case HexType.WOOL:
                                    numWool--;
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    break;
                                default:
                                    hexMap[i][j].prevHexType = hexMap[i][j].hexType;
                                    numUnsetType--;
                                    break;

                            }
                        }

                        #endregion

                        if (hexMap[i][j].isActive)
                        {
                            if (hexMap[i][j].hexType != HexType.NONE)
                            {
                                TurnOnActive(i, j);
                            }
                            if (GUI.Button(labelPosition, labelText, style))
                            {

                                selectedRow = i;
                                selectedColumn = j;
                            }
                        }
                        else
                        {
                            EditorGUI.LabelField(labelPosition, string.Format("({0},{1})\n", i, j));
                        }

                        ScriptHex hex = new ScriptHex(hexCenter, hexEdgeLength);

                        #region draw the hex
                        // Vertical lines

                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[2], hex.hexCorners[3], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[5], Color.black, 1f, false);
                        }


                        // Bottom of hex
                        hex.hexCorners[0].x += 17f;
                        hex.hexCorners[1].x += 17f;
                        hex.hexCorners[2].x += 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[0], hex.hexCorners[1], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[1], hex.hexCorners[2], Color.black, 1f, false);
                        }

                        // Top of hex
                        hex.hexCorners[3].x -= 17f;
                        hex.hexCorners[4].x -= 17f;
                        hex.hexCorners[5].x -= 17f;
                        if (hexMap[i][j].isActive)
                        {
                            switch (hexMap[i][j].hexType)
                            {
                                case HexType.BRICK:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.red, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.red, 1f, true);
                                    break;
                                case HexType.GRAIN:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.yellow, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.yellow, 1f, true);
                                    break;
                                case HexType.WOOD:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.green, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.green, 1f, true);
                                    break;
                                case HexType.WOOL:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.white, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.white, 1f, true);
                                    break;
                                default:
                                    Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.blue, 1f, true);
                                    Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.blue, 1f, true);
                                    break;
                            }
                        }
                        else
                        {
                            Drawing.DrawLine(hex.hexCorners[3], hex.hexCorners[4], Color.black, 1f, false);
                            Drawing.DrawLine(hex.hexCorners[4], hex.hexCorners[5], Color.black, 1f, false);
                        }

                        #endregion

                        xOffset += hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f;
                    }
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (22 - i);
                    xOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Cos(Mathf.PI * (4f / 3f)));
                    yOffset -= (hexEdgeLength * MAGIC_EDGE_DISTANCE_CONVERT * 2f) * (Mathf.Sin(Mathf.PI * (4f / 3f))) + 1;
                    break;
            }
        }

        #endregion

        EditorGUILayout.EndVertical();

        #region Map Data
        EditorGUILayout.BeginVertical("box", GUILayout.Width(position.width * 0.08f), GUILayout.Height(position.height - 50f));

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Selected Hex Data", EditorStyles.boldLabel);

        // Displays the selected hexes coordinates
        EditorGUILayout.LabelField(string.Format("Hex Coordinate: ({0},{1})", selectedRow, selectedColumn), GUILayout.Width(130f));

        // Area to select the hex roll value
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Roll Value: ", GUILayout.Width(64f));
        hexMap[selectedRow][selectedColumn].hexNum = EditorGUILayout.IntPopup(hexMap[selectedRow][selectedColumn].hexNum, intPopupString, intPopup, GUILayout.Width(35f));

        EditorGUILayout.EndHorizontal();

        // Area to select the hex type
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Hex Type: ", GUILayout.Width(62f));
        hexTypeIndex = EditorGUILayout.Popup(hexTypeIndex, hexTypeOptions, GUILayout.Width(55f));

        EditorGUILayout.EndHorizontal();

        #endregion

        // Switch to convert from the string array to the hex type based off the index
        switch (hexTypeIndex)
        {
            case 0:
                hexMap[selectedRow][selectedColumn].hexType = HexType.WOOD;
                hexTypeIndex = 5;
                break;
            case 1:
                hexMap[selectedRow][selectedColumn].hexType = HexType.GRAIN;
                hexTypeIndex = 5;
                break;
            case 2:
                hexMap[selectedRow][selectedColumn].hexType = HexType.WOOL;
                hexTypeIndex = 5;
                break;
            case 3:
                hexMap[selectedRow][selectedColumn].hexType = HexType.BRICK;
                hexTypeIndex = 5;
                break;
            case 4:
                hexMap[selectedRow][selectedColumn].hexType = HexType.NONE;
                hexTypeIndex = 5;
                break;
        }

        SetAvailableHexTypes();
        SetAvailableRollNums();

        #region Heuristic Display
        float heuristicOffset = 175f;
        float heuristicX = 1060f;
        // Displays the heuristics to the designer
        Rect heuristicLabelRect = new Rect(heuristicX, heuristicOffset, 100f, 15f);
        EditorGUI.LabelField(heuristicLabelRect, "Amount Placed", EditorStyles.boldLabel);
        heuristicOffset += 17f;

        Rect numBrickLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numBrickLabelRect, "Brick: " + numBrick);
        heuristicOffset += 17f;

        Rect numWheatLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numWheatLabelRect, "Grain: " + numWheat);
        heuristicOffset += 17f;

        Rect numWoodLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numWoodLabelRect, "Wood: " + numWood);
        heuristicOffset += 17f;

        Rect numWoolLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numWoolLabelRect, "Wool: " + numWool);
        heuristicOffset += 17f;

        Rect typeUnsetLabelRect = new Rect(heuristicX, heuristicOffset, 150f, 15f);
        EditorGUI.LabelField(typeUnsetLabelRect, "Hex Types Unset: " + numUnsetType, EditorStyles.boldLabel);
        heuristicOffset += 17f;

        Rect numOneLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numOneLabelRect, "1's: " + numOnes);
        heuristicOffset += 17f;

        Rect numTwoLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numTwoLabelRect, "2's: " + numTwos);
        heuristicOffset += 17f;

        Rect numThreeLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numThreeLabelRect, "3's: " + numThrees);
        heuristicOffset += 17f;

        Rect numFourLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numFourLabelRect, "4's: " + numFours);
        heuristicOffset += 17f;

        Rect numFiveLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numFiveLabelRect, "5's: " + numFives);
        heuristicOffset += 17f;

        Rect numSixLabelRect = new Rect(heuristicX, heuristicOffset, 65f, 15f);
        EditorGUI.LabelField(numSixLabelRect, "6's: " + numSixes);
        heuristicOffset += 17f;

        Rect rollUnsetLabelRect = new Rect(heuristicX, heuristicOffset, 150f, 15f);
        EditorGUI.LabelField(rollUnsetLabelRect, " Roll Values Unset: " + numUnsetRollNum, EditorStyles.boldLabel);

        heuristicOffset += 30f;
        #endregion

        #region Export Data Button
        Rect exportDataButtonRect = new Rect(1078f, heuristicOffset, 100f, 25f);
        Color oldColor = GUI.color;

        // Check to make sure that at least one of each resource exist on the game map
        // Also check to make sure that the number of unset roll numbers is the same as the unset types
        // to ensure that all of the hexes that are given a hex type also have a valid number associated with that hex
        if (numBrick >= 1 && numWood >= 1 && numWheat >= 1 && numWool >= 1 && numUnsetRollNum == numUnsetType)
        {
            GUI.color = Color.green;
            if (GUI.Button(exportDataButtonRect, "Export Map"))
            {
                Debug.Log("Exports the map!");
            }

            GUI.color = oldColor;
        }

        #endregion

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    void SetAvailableHexTypes()
    {

        // Finds the highest and lowest of all counts
        int highestCount = Math.Max(Math.Max(numBrick, numWood), Math.Max(numWheat, numWool));
        int lowestCount = Math.Min(Math.Min(numBrick, numWood), Math.Min(numWheat, numWool));

        // Removes hexTypes from string array accordingly
        if (highestCount - lowestCount >= 2)
        {
            // private string[] hexTypeOptions = { "Wood", "Grain", "Wool", "Brick", "None" };
            if (numBrick == highestCount)
            {
                hexTypeOptions[3] = "";
            }
            if (numWood == highestCount)
            {
                hexTypeOptions[0] = "";
            }
            if (numWheat == highestCount)
            {
                hexTypeOptions[1] = "";
            }
            if (numWool == highestCount)
            {
                hexTypeOptions[2] = "";
            }
        }
        else
        {
            hexTypeOptions[0] = "Wood";
            hexTypeOptions[1] = "Grain";
            hexTypeOptions[2] = "Wool";
            hexTypeOptions[3] = "Brick";
        }
    }

    // Implementation of the roll heuristic difference of 2
    void SetAvailableRollNums()
    {
        // Finds the highest and lowest of all the heuristic values
        int highestCount = Math.Max(Math.Max(Math.Max(numOnes, numTwos), numThrees),Math.Max(Math.Max(numFours, numFives), numSixes));
        int lowestCount = Math.Min(Math.Min(Math.Min(numOnes, numTwos), numThrees), Math.Min(Math.Min(numFours, numFives), numSixes));

        // Sets showing string array accordingly
        if (highestCount - lowestCount >= 2)
        {
            if (numOnes == highestCount)
            {
                intPopupString[0] = "";
            }
            if (numTwos == highestCount)
            {
                intPopupString[1] = "";
            }
            if (numThrees == highestCount)
            {
                intPopupString[2] = "";
            }
            if (numFours == highestCount)
            {
                intPopupString[3] = "";
            }
            if (numFives == highestCount)
            {
                intPopupString[4] = "";
            }
            if (numSixes == highestCount)
            {
                intPopupString[5] = "";
            }
        }
        else
        {
            intPopupString[0] = "1";
            intPopupString[1] = "2";
            intPopupString[2] = "3";
            intPopupString[3] = "4";
            intPopupString[4] = "5";
            intPopupString[5] = "6";
        }
    }

    void InitializeVariables()
    {
        hexMap[0] = new ScriptHex[8];
        hexMap[1] = new ScriptHex[9];
        hexMap[2] = new ScriptHex[10];
        hexMap[3] = new ScriptHex[11];
        hexMap[4] = new ScriptHex[12];
        hexMap[5] = new ScriptHex[13];
        hexMap[6] = new ScriptHex[14];
        hexMap[7] = new ScriptHex[15];
        hexMap[8] = new ScriptHex[14];
        hexMap[9] = new ScriptHex[13];
        hexMap[10] = new ScriptHex[12];
        hexMap[11] = new ScriptHex[11];
        hexMap[12] = new ScriptHex[10];
        hexMap[13] = new ScriptHex[9];
        hexMap[14] = new ScriptHex[8];
       
        for (int i = 0; i < hexMap.Length; i++)
        {
            
            for (int j = 0; j < hexMap[i].Length; j++)
            {
                hexMap[i][j] = new ScriptHex();
            }
        }

        hexMap[7][7].isActive = true;
        hexMap[7][7].hexType = HexType.WOOD;
        hexMap[7][7].hexNum = 3;
        TurnOnActive(7, 7);

    }

    void TurnOnActive(int startRow, int startColumn)
    {
        // Series of checks to make sure you don't active a hex that isn't there

        // Check directly right
        if (startColumn + 1 < hexMap[startRow].Length)
        {
            if (!hexMap[startRow][startColumn + 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow][startColumn + 1].isActive = true;
            }
        }

        // Check directly left
        if (startColumn - 1 >= 0)
        {
            if (!hexMap[startRow][startColumn - 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow][startColumn - 1].isActive = true;
            }            
        }

        // First row of map
        if (startRow == 0)
        {
            // Check down right
            if (startColumn + 1 < hexMap[startRow + 1].Length && startRow + 1 < hexMap.Length)
            {
                if (!hexMap[startRow + 1][startColumn + 1].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow + 1][startColumn + 1].isActive = true;
                }
            }

            // Check down left
            if (startColumn >= 0 && startRow + 1 < hexMap.Length)
            {
                if (!hexMap[startRow + 1][startColumn].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow + 1][startColumn].isActive = true;
                }
            }
        }

        // First column of top half
        else if (startColumn == 0 && startRow < 7)
        {
            // Down left
            if (!hexMap[startRow + 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn].isActive = true;
            }

            // Down right
            if (!hexMap[startRow + 1][startColumn + 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn + 1].isActive = true;
            }

            // Up right
            if (!hexMap[startRow - 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow - 1][startColumn].isActive = true;
            }
        }

        // Top half of map
        else if (startRow < 7)
        {
            // Down right
            if (!hexMap[startRow + 1][startColumn + 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn + 1].isActive = true;
            }


            // Down left
            if (!hexMap[startRow + 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn].isActive = true; ;
            }
            

            // Check up left
            if (startColumn >= 0 && startRow - 1 >= 0)
            {
                if (!hexMap[startRow - 1][startColumn - 1].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow - 1][startColumn - 1].isActive = true; ;
                }
            }

            // Check up right
            if (startColumn + 1 < hexMap[startRow - 1].Length && startRow > 0)
            {
                if (!hexMap[startRow - 1][startColumn].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow - 1][startColumn].isActive = true; ;
                }
            }
        }

        // Special case for right most middle row
        else if (startColumn == hexMap[startRow].Length - 1 && startRow == 7)
        {

            // Down left
            if (!hexMap[startRow + 1][startColumn - 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn - 1].isActive = true; ;
            }

            // Up left
            if (!hexMap[startRow - 1][startColumn - 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow - 1][startColumn - 1].isActive = true; ;
            }

        }

        // Special case for left most middle row
        else if (startColumn == 0 && startRow == 7)
        {

            // Up right
            if (!hexMap[startRow - 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow - 1][startColumn].isActive = true; ;
            }

            // Down right
            if (!hexMap[startRow + 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn].isActive = true; ;
            }

        }

        // Middle row of map
        else if (startRow == 7)
        {
            // Down right
            if (!hexMap[startRow + 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn].isActive = true; ;
            }

            // Down left
            if (!hexMap[startRow + 1][startColumn - 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn - 1].isActive = true; ;
            }

            // Up left
            if (!hexMap[startRow - 1][startColumn - 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow - 1][startColumn - 1].isActive = true; ;
            }

            // Up right
            if (!hexMap[startRow - 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow - 1][startColumn].isActive = true; ;
            }
        }

        // Bottom row of map
        else if (startRow == hexMap.Length - 1)
        {
            // Check up left
            if (startColumn >= 0)
            {
                if (!hexMap[startRow - 1][startColumn].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow - 1][startColumn].isActive = true; ;
                }
            }

            // Check up right
            if (startColumn + 1 < hexMap[startRow - 1].Length)
            {
                if (!hexMap[startRow - 1][startColumn + 1].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow - 1][startColumn + 1].isActive = true; ;
                }
            }
        }

        // First column of bottom half
        else if (startColumn == 0 && startRow > 7)
        {
            // Up left
            if (!hexMap[startRow - 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow - 1][startColumn].isActive = true; ;
            }

            // Up right
            if (!hexMap[startRow - 1][startColumn + 1].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow - 1][startColumn + 1].isActive = true; ;
            }

            // Down right
            if (!hexMap[startRow + 1][startColumn].isActive)
            {
                numUnsetRollNum++;
                numUnsetType++;
                hexMap[startRow + 1][startColumn].isActive = true; ;
            }

        }

        // Bottom half of map
        else if (startColumn <= hexMap[startRow].Length - 1 && startRow > 7)
        {
            // Check down right
            if (startColumn < hexMap[startRow + 1].Length && startRow + 1 < hexMap.Length)
            {
                if (!hexMap[startRow + 1][startColumn].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow + 1][startColumn].isActive = true; ;
                }
            }

            // Check down left
            if (startColumn - 1 >= 0 && startRow + 1 < hexMap.Length)
            {
                if (!hexMap[startRow + 1][startColumn - 1].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow + 1][startColumn - 1].isActive = true; ;
                }
            }

            // Check up left
            if (startColumn >= 0 && startRow - 1 >= 0)
            {
                if (!hexMap[startRow - 1][startColumn].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow - 1][startColumn].isActive = true; ;
                }
            }

            // Check up right
            if (startColumn + 1 < hexMap[startRow - 1].Length && startRow > 0)
            {
                if (!hexMap[startRow - 1][startColumn + 1].isActive)
                {
                    numUnsetRollNum++;
                    numUnsetType++;
                    hexMap[startRow - 1][startColumn + 1].isActive = true; ;
                }
            }
        }
    }
}