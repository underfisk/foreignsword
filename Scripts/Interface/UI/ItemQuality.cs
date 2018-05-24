using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemQuality : MonoBehaviour
{

    [Serializable]
    public enum Quality : int
    {
        Poor = 0,
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5
    }

    public class UIItemQualityColor
    {
        public const string Poor = "9d9d9dff";
        public const string Common = "ffffffff";
        public const string Uncommon = "1eff00ff";
        public const string Rare = "0070ffff";
        public const string Epic = "a335eeff";
        public const string Legendary = "ff8000ff";

        public static string GetHexColor(Quality _quality)
        {
            switch (_quality)
            {
                case Quality.Poor: return Poor;
                case Quality.Common: return Common;
                case Quality.Uncommon: return Uncommon;
                case Quality.Rare: return Rare;
                case Quality.Epic: return Epic;
                case Quality.Legendary: return Legendary;
            }

            return Poor;
        }

        public static Color GetColor(Quality quality)
        {
            return CommonColorBuffer.StringToColor(GetHexColor(quality));
        }

        //TODO : The Legendary Quality will set a gem effect on the weapon!
    }
}
