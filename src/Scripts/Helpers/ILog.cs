using System;
using System.IO;
using UnityEngine;

//ILog => Internal Log
namespace HelperPackage
{
    /// <summary>
    /// Debug enumerator to filter the messages
    /// </summary>
    public enum LType
    {
        Default,
        Success,
        Error,
        Warning,
        Exception,
        Destroyed,
        Processing
    }

    public class ILog : MonoBehaviour
    {

        /// <summary>
        /// user logs folder, use unity get current path
        /// </summary>
        public static readonly string log_path = @"unitypath\logs\";

        /// <summary>
        /// This function just prints out a message for Unity Editor
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="type"></param>
        public static void toUnity(string _msg, LType type = LType.Default)
        {
            switch (type)
            {
                case LType.Success : Debug.Log($"[{DateTime.Now}] : <color=green> {_msg} </color>"); break;
                case LType.Error: Debug.LogError($"[{DateTime.Now}] :  <color=red> {_msg} </color>}"); break;
                case LType.Warning: Debug.LogWarning($"[{DateTime.Now}] Warning: {_msg}"); break;
                case LType.Exception: Debug.LogWarning($"[{DateTime.Now}] Exception: {_msg}"); break;
                case LType.Destroyed: Debug.Log($"[{DateTime.Now}] : <color=orange>{_msg} </color> "); break;
                case LType.Processing: Debug.Log($"[{DateTime.Now}] : <color=yellow>{_msg} </color> "); break;
                default: Debug.Log($"[{DateTime.Now}] : <color=white> {_msg} </color>"); break;
            }
        }

        /// <summary>
        /// This function creates a file with a log message
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="_file"></param>
        /// <param name="_new"></param>
        public static void toFile(string _msg, string _file = "", bool _new = false)
        {
            bool dirExists = Directory.Exists(log_path);
            if (!dirExists)
                Directory.CreateDirectory(log_path);

            if (!_new)
            {
                if (!String.IsNullOrEmpty(_file))
                {
                    try
                    {
                        using (var sw = new StreamWriter(log_path + _file))
                        {
                            sw.Write(_msg);
                            sw.Close();
                            toUnity("New log entry was been registered to file:" + _file);
                        }
                    }
                    catch (IOException e)
                    {
                        toUnity(e.ToString(),LType.Exception);
                    }
                }
                else
                {
                    throw new Exception("There are no file name given as parameter to be opened, nor new file instruction!");
                }
            }
            else
            {
                var _filename = File.Create("logg"); //file name is the current timestamp
                using (var sw = new StreamWriter(log_path + _file))
                {
                    sw.Write(_msg);
                    sw.Close();
                    toUnity("New log entry was been registered to the new file:" + _filename);
                }
            }
        }


        /// <summary>
        /// This function just sends a post to our webserver as a report
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="_url"></param>
        public static void toDatabase(string _msg, string _url)
        {
            string title = "Unity Report Log", body = _msg;
            //var response = Http.POST(_url, $"title={title}+body={body}");
            toUnity("New log entry to the database was been registered");
        }

    }
}
                    
                 

