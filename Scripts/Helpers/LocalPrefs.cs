using System;
using UnityEngine;

namespace HelperPackage
{
    public class LocalPrefs : MonoBehaviour
    {
        /// <summary>
        /// Health Option Brute,Percentage, and both
        /// </summary>
        public static int Health
        {
            get { return PlayerPrefs.GetInt("HealthOption"); }
            set { PlayerPrefs.SetInt("HealthOption", value); }
        }

        /// <summary>
        /// Render show or hide and don't render
        /// </summary>
        public static int CharacterRender
        {
            get { return PlayerPrefs.GetInt("CharacterRender"); }
            set { PlayerPrefs.SetInt("CharacterRender", value); }
        }

        /// <summary>
        /// Get's time enabled or not
        /// </summary>
        public static int Time
        {
            get { return PlayerPrefs.GetInt("TimeEnabled"); }
            set { PlayerPrefs.SetInt("TimeEnabled", value); }
        }

        /// <summary>
        /// 24h format or English one (NOT DONE YET TODO)
        /// </summary>
        public static int TimeFormat
        {
            get { return PlayerPrefs.GetInt("TimeFormat"); }
            set { PlayerPrefs.SetInt("TimeFormat", value); }
        }

        public static int FPS
        {
            get { return PlayerPrefs.GetInt("FPSEnabled"); }
            set { PlayerPrefs.SetInt("FPSEnabled", value); }
        }


        public static float MusicVolume
        {
            get { return PlayerPrefs.GetFloat("MusicVolume"); }
            set { PlayerPrefs.SetFloat("MusicVolume", value); }
        }

        public static float AmbientVolume
        {
            get { return PlayerPrefs.GetFloat("AmbientVolume"); }
            set { PlayerPrefs.SetFloat("AmbientVolume", value); }
        }

        public static float DialogueVolume
        {
            get { return PlayerPrefs.GetFloat("DialogueVolume"); }
            set { PlayerPrefs.SetFloat("DialogueVolume", value); }
        }

        public static float EffectsVolume
        {
            get { return PlayerPrefs.GetFloat("EffectsVolume"); }
            set { PlayerPrefs.SetFloat("EffectsVolume", value); }
        }

        public static void SetHealthPreferences(string v)
        {
            PlayerPrefs.SetString("HealthOption", v);
        }

        public static void SetResolution(int h, int w)
        {
            PlayerPrefs.SetInt("Height", h);
            PlayerPrefs.SetInt("Width", w);
        }

        public static Vector2 GetResolution()
        {
            return new Vector2(
              PlayerPrefs.GetInt("Height"),
              PlayerPrefs.GetInt("Width")
            );
        }

        public static void SetAbilityKey(int index, KeyCode key)
        {
            PlayerPrefs.SetInt("Ability" + index, (int)key);
        }

        public static int GetAbilityKey(int index)
        {
            return PlayerPrefs.GetInt("Ability" + index);
        }



    }
}