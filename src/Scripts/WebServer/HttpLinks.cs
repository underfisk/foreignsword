using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpPackage
{
    public class HttpLinks
    {
        /// <summary>
        /// API Token to exchange data
        /// </summary>
        public static readonly string api_token = "7uq3yLHOoT"; 

        /// <summary>
        /// Base API url
        /// </summary>
        public static readonly string base_url = "http://127.0.0.1/foreignweb/API/";

        /// <summary>
        /// Character controller url has the param option for single or all
        /// </summary>
        public static readonly string character = base_url + "characters";

        /// <summary>
        /// Create character verification if the name exists sending the char name pretended
        /// </summary>
        public static readonly string character_exists = base_url + "character_exists";

        /// <summary>
        /// Create character request posting the char data
        /// </summary>
        public static readonly string character_create = base_url + "character_create";

        /// <summary>
        /// Deete character request with char_id
        /// </summary>
        public static readonly string character_delete = base_url + "character_delete";

        /// <summary>
        /// Authentication Login url
        /// </summary>
        public static readonly string auth = base_url + "auth";

    }
}
