using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


namespace HelperPackage
{
    [System.Serializable]
    public class JsonHelper
    {

        /// <summary>
        /// Class used internally to parse objects into json for database update
        /// </summary>
        public class Parse
        {
            //TODO YET
            public static string Character(string name)
            {
                string js = ""; //the string 
                return "";
            }

            public static string Inventory(List<Item> inv)
            {
                string js = "";
                return "";
            }

            public static string Achivements()
            {
                return "";
            }
        }


        public static List<Item> toItems()
        {
            List<Item> tempList = new List<Item>();
            string itemsFile = FileManager.item_file_path;
            string result = FileManager.Read(itemsFile);
            ILog.toUnity("Loding items :" + result);
            if (isJson(result))
            {
                JSONNode json = JSON.Parse(result);
                for (int i = 0; i < json.Count; i++)
                {
                    tempList.Add(new Item());
                }
            }

            return tempList;
        }

        /// <summary>
        /// Parse json string to account object
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static bool toAccount(string jsonString)
        {
            JSONNode json = JSON.Parse(jsonString);
            if (json[0]["id"].AsInt != 0)
            {
                GameData.PlayerID = json[0]["id"].AsInt; //we just want 0 pos cuz it's only 1 result coming 
                ILog.toUnity("A player ID was been set, ID:" + json[0]["id"].AsInt);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Parse json string to character list objects
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<Character> toCharacters(string jsonString)
        {

            List<Character> tempChar = new List<Character>();
            JSONNode json = JSON.Parse(jsonString);

            //NOT USING JSONArr atm because my php is just sending all as nodes
            for(int i = 0; i < json.Count; i++)
            {
                //NOt complete yet
                tempChar.Add( new Character(
                    json[i]["c_id"].AsInt,
                    json[i]["c_name"].Value,
                    json[i]["c_class"].AsInt,
                    json[i]["c_level"].AsInt,
                    new Inventory()
                ));
            }

            return tempChar;
        }

        /// <summary>
        /// Returns the result of checking the string given
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static bool isJson(string js)
        {
            //we just check if starts with { and ends with }
            if (string.IsNullOrEmpty(js)) return false;

            var c = js.Trim();
            if ((c.StartsWith("{") && c.EndsWith("}")) || //For object
            (c.StartsWith("[") && c.EndsWith("]"))) //For array
            {
                return true;
            }

            return false;//false by default
        }

    }
}