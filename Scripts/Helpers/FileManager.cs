using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;

public static class FileManager
{
    //class used for serialize, deserealize the json / csv and to return concrete results
    public static readonly string FilePath;
    public static readonly string item_file_path = Application.streamingAssetsPath + "Assets/Share/items.json";
    public static readonly string monster_file_path = Application.streamingAssetsPath + "Assets/Share/monsters.json";
  
    /// <summary>
    /// Reads a file text and returns the output
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string Read(string file)
    {
        return System.IO.File.ReadAllText(file);
    }

    public static void Write(string t)
    {
        
    }
}