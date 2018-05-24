using System;
using HelperPackage;

namespace HttpPackage
{
    public class HttpHandler
    {
        public static string HandleResponse(string response)
        {
            switch(response)
            {
                case "0x1f":
                    ILog.toUnity("The character does not exist/Internal server error", LType.Error);
                    return "The character does not exist/Internal server error";
                case "0x2f":
                    ILog.toUnity("Login details are wrong or internal server error",LType.Error);
                    return "Invalid Credentials or Internal server error";
            }
            return "";
        }        
    }
}