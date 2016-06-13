using Api.PayTPVService;
using ApiPayTPV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ApiPayTPV_Csharp.Controllers
{
    public class PayTpvApiController : ApiController
    {
        private string MERCHANTCODE = "1br6407g";
        private string TERMINAL = "4473";
        private string PASSWORD = "fnz5kc7pxw0y4hb963qt";
        private string ENDPOINT = "https://secure.paytpv.com/gateway/xml_bankstore.php?wsdl";
        private string ENDPOINTURL = "https://secure.paytpv.com/gateway/bnkgateway.php?";
        private string JETID;
        ApiPayTPV.Paytpv_Bankstore client;

        public PayTpvApiController()
        {
            var ip = HttpContext.Current.Request.UserHostAddress;
            client = new ApiPayTPV.Paytpv_Bankstore(this.MERCHANTCODE, this.TERMINAL, this.PASSWORD, ip);
        }

        /// <summary>
        /// POST: Set Api Authentication Properties
        /// Set this property before making any other call
        /// </summary>
        /// <param name="MERCHANTCODE"></param>
        /// <param name="TERMINAL"></param>
        /// <param name="PASSWORD"></param>
        /// <param name="JETID"></param>
        /// <returns></returns>        
        public string PayTpvConfigure(string MERCHANTCODE, string TERMINAL, string PASSWORD, string JETID = null)
        {
            this.MERCHANTCODE = MERCHANTCODE;
            this.TERMINAL = TERMINAL;
            this.PASSWORD = PASSWORD;
            this.JETID = JETID;

            return "ok";
        }

        /// <summary>
        /// POST api/PayTpvApi/AddUser
        /// </summary>
        /// <param name="pan">Credit card number</param>
        /// <param name="expdate">Credit card expiry date</param>
        /// <param name="cvv">Credit card CVV number</param>
        /// <returns></returns>
        ///            
        [System.Web.Http.Route("AddUser")]
        public ApiResponse AddUser(PayTpvProperties model)
        {
            return client.AddUser(model);
        }

        public ApiResponse InfoUser(string IDPAYUSER, string TOKENPAYUSER)
        {
            var ip = HttpContext.Current.Request.UserHostAddress;
            ApiPayTPV.Paytpv_Bankstore client = new ApiPayTPV.Paytpv_Bankstore(this.MERCHANTCODE, this.TERMINAL, this.PASSWORD, ip);
            return client.AddUser(model);
        }

        #region Encryption Methods

        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        #endregion
    }
}