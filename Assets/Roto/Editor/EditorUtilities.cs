using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorUtilities
{


    /// <summary>
    /// Adds a menu option to unity to create a blank configuration json file
    /// </summary>
    [MenuItem("Tools/Roto/Create Configuration File")]
    private static void CreateConfigurationFileFromMenu()
    {
        string configurationFileName = Application.streamingAssetsPath + "/config.json";

        // creates streaming assets directory if necessary
        Directory.CreateDirectory(Application.streamingAssetsPath);

        // only do something if a "config.json" file doesn't exists in StreamingAssets
        if (!File.Exists(configurationFileName))
        {
            using (FileStream fs = File.Create(configurationFileName))
            {
                byte[] startingText = new System.Text.UTF8Encoding(true).GetBytes("{\n\t\"timeout-seconds\": 40\n}");
                fs.Write(startingText, 0, startingText.Length);
            }

            AssetDatabase.ImportAsset("Assets/StreamingAssets/config.json");

            Debug.Log("File successfully created.");
        }
        else
        {
            Debug.Log(configurationFileName + " already exists!");
        }
    }

}
