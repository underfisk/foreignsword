using System;
using UnityEngine;

namespace HelperPackage
{
    public class Settings : MonoBehaviour
    {
        private string operating_system;
        private string HWID;
        private int cpu_frequency;
        private int cpu_count;
        private string gpu;
        private int resolution_h, resolution_w;
        private string cpu;
        private bool hasNetwork; //check if can connect with the server
        private int total_ram;
        private int app_usage_ram; //get our process usage ram on this app
        private float sfx_music_value;
        private float sfx_dialog_value;
        private float sfx_combat_value;
        private bool playOnline; //check if he want's to play online or not to save the data in our servers
                                 //this is not a setting private string platformNick; //the username like blizzard nick for our platform


        /// <summary>
        /// init the settings config being holded here
        /// </summary>
        public Settings()
        {
            operating_system = SystemInfo.operatingSystem;
            HWID = SystemInfo.deviceUniqueIdentifier;
            gpu = SystemInfo.graphicsDeviceName; //also format to show the GPU memory SystemInfo.graphicsMemoryCard
            cpu = SystemInfo.processorType;
            cpu_frequency = SystemInfo.processorFrequency;
            cpu_count = SystemInfo.processorCount;
            total_ram = SystemInfo.systemMemorySize;
        }

        public string OperativeSystem
        {
            get { return operating_system; }
        }

        public string HardwareID
        {
            get { return HWID; }
        }

        public string CPU
        {
            get { return cpu; }
        }

        public string GPU
        {
            get { return gpu; }
        }

        public int MemoryRAM
        {
            get { return total_ram; }
        }
        /// <summary>
        /// prefs save some of this configs
        /// </summary>
        public void SaveSettings()
        {


        }
        //this configs will be saved in the prefs (only some because theres many of them that will be retrieved when the app starts)
    }
}