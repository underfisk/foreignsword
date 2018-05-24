using HelperPackage;
using System;
using System.Collections;
using System.Collections.Generic;

/**
* Main metadata class used for runtime support
* @author Enigma
* @package ForeignSword
*/

public static class GameData
{

    /// <summary>
    /// Key -> Level, Value -> EXP Needed
    /// </summary>
    private static float[] levelsExp = new float[30+1];

    public static int ServerEXP = 1; 
    private static int player_id;
    private static Character cdata;
    private static Settings gSettings;
    private static List<Item> gItems;
    private static string mname; //map name

    public static string MapName
    {
        get { return mname; }
        set { mname = value; }
    }

    public static void InitializeSettings()
    {
        gSettings = new Settings();
    }

    public static void InitializeItems()
    {
        gItems = JsonHelper.toItems();
    }

    public static void InitializeLevelEXP()
    {
        //30 Levels for now
        //The exp is multiplied by 2x the previous
        float exp = 50; //default exp
        for(int i = 1; i < levelsExp.Length; i++)
        {
            levelsExp[i] = exp;
           // ILog.toUnity($"Level : {i} and exp : {exp} ");
            exp *= 2f;
        }
    }

    public static Settings GameSettings
    {
        get { return gSettings; }
        set { gSettings = value; }
    }

    public static int PlayerID
    {
        get { return player_id; }
        set { player_id = value; }
    }

    public static Character CharacterData
    {
        get { return cdata; }
        set { cdata = value; }
    }

    public static float[] LevelsExperience
    {
        get { return levelsExp; }
        set { levelsExp = value; }
    }

   
}

