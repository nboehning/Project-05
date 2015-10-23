using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// @Author Jake Skov
/// @Desc Handles the export of the custom map
/// </summary>
public class ScriptMapExport : EditorWindow
{
    //Generic Variables
    public string levelName;
    public string authorName;

    //Creates Writer variables
    private string path = Application.dataPath + "Maps/Custom/";
    private StreamWriter writer;

    //Gets the Map Creator script for referanceing
    public ScriptMapCreationWindow mapCreator; 

    [SerializeField]
    public void ExportMap()
    {
        InitializeDefaults();
        AdditionalInfo();
    }

    //Writes relevent data to the file inside of Maps/Custom with the title of LevelName.txt
    void WriteToFile()
    {
        if (levelName.Contains(" "))
        {
            levelName = levelName.Replace(" ", "_");
            Debug.Log(levelName);
        }
        path = path + levelName + ".txt";

        writer = new StreamWriter(path);
        using (writer)
        {
            //Writes the level name and author
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    writer.WriteLine(levelName);
                }
                else
                {
                    writer.WriteLine(authorName);
                }
            }
            //Sets all hex data
            for (int i = 0; i < mapCreator.hexMap.GetLength(0); i++)
            {
                for (int j = 0; j < mapCreator.hexMap.GetLength(1); j++)
                {
                    writer.WriteLine(mapCreator.hexMap[i][j].hexType + "_" + i + "," + j + "_" +
                        mapCreator.hexMap[i][j].hexNum);
                }
            }
        }
    }

    //Creates default values
    void InitializeDefaults()
    {
        levelName = "Level Name";
        authorName = "Author Name";
        path = Application.dataPath + "/Maps/Custom/";
    }

    //Creates the secondary window
    void AdditionalInfo()
    {
        ScriptMapExport window = (ScriptMapExport)EditorWindow.GetWindow(typeof(ScriptMapExport));
        window.maxSize = new Vector2(250, 100);
        window.minSize = window.maxSize;
        window.Show();
    }

    //Window GUI
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Separator();

        levelName = EditorGUILayout.TextField("Level Name:\t", levelName);
        authorName = EditorGUILayout.TextField("Author Name:\t", authorName);

        EditorGUILayout.Separator();

        if(GUILayout.Button("Confirm Export"))
        {
            WriteToFile();
        }
    }
}
