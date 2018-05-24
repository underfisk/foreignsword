using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HelperPackage;
using UnityEngine;
using UnityEngine.Networking;

namespace HttpPackage
{
    public class HttpForm
    {
        /// <summary>
        /// Holds the fields data
        /// </summary>
        Dictionary<string, object> fields = new Dictionary<string, object>();

        /// <summary>
        /// Adds a new field with value of type string
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void AddField(string fieldName, string value)
        {
            fields.Add(fieldName, value);
        }

        /// <summary>
        /// Adds a new field with value of type integer
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void AddField(string fieldName, int value)
        {
            fields.Add(fieldName, value);
        }

        /// <summary>
        /// Returns the formated "Raw Post" string 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = String.Empty;
            foreach(KeyValuePair<string, object> field in fields)
            {
                str += $"{field.Key}={field.Value}&";
            }

            str += $"tkn={HttpLinks.api_token}";
            return str;
        }
    }

    public class HttpRequest
    {
        /// <summary>
        /// Holds the status code of this response
        /// </summary>
        public HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

        /// <summary>
        /// Returns the state of the request
        /// </summary>
        public bool isDone = false;

        /// <summary>
        /// Number of bytes downloaded
        /// </summary>
        public byte[] bytes;

        /// <summary>
        /// Content Length from streamReader
        /// </summary>
        public long length = 0;

        /// <summary>
        /// Content type (raw html/text, etc)
        /// </summary>
        public string type = String.Empty;

        /// <summary>
        /// Request Headers
        /// </summary>
        public string headers = String.Empty;

        /// <summary>
        /// Request URL just saved in case of the user wanted to know
        /// </summary>
        public string url = String.Empty;

        /// <summary>
        /// Holds the state of the content type
        /// </summary>
        public bool isJson = false;

        /// <summary>
        /// Returns any error if occured
        /// </summary>
        public bool isError = false;

        /// <summary>
        /// Holds the raw data from this request
        /// </summary>
        public string ContentResponse;

        
        /// <summary>
        /// Request a pure HttpRequest syncronizated which do not return but his data may be accessed on the created instance
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public void Post(string url, HttpForm data)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data.ToString());
                request.ContentLength = bytes.Length;

                Stream os = request.GetRequestStream();
                os.Write(bytes, 0, bytes.Length); //Push it out there
                os.Close();
                HttpWebResponse resp = (HttpWebResponse)request.GetResponse();

                if (resp == null) ContentResponse = String.Empty;

                StreamReader sr = new StreamReader(resp.GetResponseStream());

                //Releases the resources of the response
                resp.Close();

                //The request was successfull
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    //Set instance data
                    ContentResponse = sr.ReadToEnd().Trim();
                    headers = resp.Headers.ToString();
                    length = resp.ContentLength;
                    this.url = url;
                    this.bytes = bytes;
                    isJson = JsonHelper.isJson(ContentResponse);
                    statusCode = resp.StatusCode;
                    isError = false;
                }

            }
            catch (WebException e)
            {
                ILog.toUnity("\r Web Exception :  " + e.Status);
                isError = true;
            }
            catch(Exception e)
            {
                ILog.toUnity("\r The following exception was raised : " + e.Message);
                isError = true;
            }
            finally
            {
                isDone = true;
            }

        }


        //Old version not being used
        /*public static string POST(string _url, string args)
        {
            try
            {
                var request = WebRequest.Create(_url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(args);
                request.ContentLength = bytes.Length;
                
                Stream os = request.GetRequestStream();
                os.Write(bytes, 0, bytes.Length); //Push it out there
                os.Close();
                WebResponse resp = request.GetResponse();

                if (resp == null) return null;

                StreamReader sr = new StreamReader(resp.GetResponseStream());

                return sr.ReadToEnd().Trim();
            }
            catch(Exception ex)
            {
                ILog.toUnity(ex.ToString());
                return "error";
            }
        }*/

        /// <summary>
        /// Receives an url and returns the result
        /// </summary>
        /// <param name="_url"></param>
        /// <returns></returns>
        
        //TODO: Refactor this class if needed
        public static string GET(string _url)
        {
            string content = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }

                ILog.toUnity("Request was sucessfuly received");
            }
            catch (WebException ex)
            {
                ILog.toUnity("Request was not sucessfuly received : " + ex.ToString());
            }

            return content;
        }

    }
}
