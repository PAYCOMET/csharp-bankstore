using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Api.PayTPVService.com.paytpv.secure;
using System.Text.RegularExpressions;
using System.Net;

namespace ApiPayTPV_Csharp.Controllers
{
    public class Paytpv_Bankstore
    {
        private Regex r = new Regex(@"/\s+/");
        private string merchantCode;
        private string terminal;
        private string password;
        private string endpoint;
        private string endpointurl;
        private string jetid;
        private string ipaddress;
        public Paytpv_Bankstore(string merchantcode, string terminal, string password, string ipaddr, string jetid = null)
        {
            this.merchantCode = merchantcode;
            this.terminal = terminal;
            this.password = password;
            this.jetid = jetid;
            this.ipaddress = ipaddr;
            this.endpoint = "https://secure.paytpv.com/gateway/xml-bankstore?wsdl";
            this.endpointurl = "https://secure.paytpv.com/gateway/ifr-bankstore?";
        }

        /// <summary>
        /// Add a card to PayTPV.  IMPORTANTES !!! This direct input must be activated by PayTPV.
        /// In default input method card for PCI-DSS compliance should be AddUserUrl or AddUserToken (method used by BankStore JET)
        /// </summary>
        /// <param name="pan">card number without spaces or dashes</param>
        /// <param name="expdate">EXPDATE expiry date of the card, expressed as "MMYY" (two-digit month and year in two digits)</param>
        /// <param name="cvv">CVC2 Card code</param>
        /// <returns>transaction response</returns>
        public stdClass AddUser(string pan, string expdate, string cvv)
        {


            stdClass result = new stdClass();
            r.Replace(pan, "");
            r.Replace(expdate, "");
            r.Replace(cvv, "");

            var signature = SHA1HashStringForUTF8String(this.merchantCode + pan + cvv + this.terminal + this.password);
            var ip = this.ipaddress;
            try
            {
                
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string dsToken, errroId;
                var ans = client.add_user(this.merchantCode, this.terminal, pan, expdate, cvv, signature, ip, "Test name", out dsToken, out errroId);


                result.data = new Dictionary<string, object>();
                result.data.Add("DS_TOKEN_USER", dsToken);
                result.data.Add("DS_IDUSER", ans);
                result.DS_ERROR_ID = errroId;
                result.RESULT = "OK";
                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
            }
            return result;
        }

        /// <summary>
        /// Returns the user information stored in a call PayTPV by soap
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">tokenpayuser user Token in PayTPV</param>
        /// <returns>transaction response</returns>
        public stdClass InfoUser(string idpayuser, string tokenpayuser)
        {

            stdClass result = new stdClass();
            r.Replace(idpayuser, "");
            r.Replace(tokenpayuser, "");

            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + this.password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string dscardBrand, dsCardType, card1CountryISO3, cardExpiryDate, errroId;
                var ans = client.info_user(this.merchantCode, this.terminal, idpayuser, tokenpayuser, signature, ip, out errroId, out dscardBrand, out dsCardType, out card1CountryISO3, out cardExpiryDate);
                if (Convert.ToInt32(errroId) > 0)
                {
                    result.DS_ERROR_ID = errroId;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_CARD_BRAND", dscardBrand);
                    result.data.Add("DS_CARD_TYPE", dsCardType);
                    result.data.Add("DS_CARD_I_COUNTRY_ISO3", card1CountryISO3);
                    result.data.Add("DS_EXPIRYDATE", cardExpiryDate);
                    result.data.Add("DS_MERCHANT_PAN", ans);
                    result.RESULT = "OK";
                }

            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Removes a user through call soap PayTPV
        /// </summary>
        /// <param name="idpayuser">user ID PayTPV</param>
        /// <param name="tokenpayuser">User Token PayTPV</param>
        /// <returns>Object A transaction response</returns>
        public stdClass RemoveUser(string idpayuser, string tokenpayuser)
        {
            stdClass result = new stdClass();
            r.Replace(idpayuser, "");
            r.Replace(tokenpayuser, "");
            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + this.password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID;
                var ans = client.remove_user(merchantCode, terminal, idpayuser, tokenpayuser, signature, ip, out DS_ERROR_ID);
                result.data = new Dictionary<string, object>();
                result.data["DS_RESPONSE"] = ans;
                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;                    
                    result.RESULT = "KO";
                }
                else
                {
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Execute a web service payment
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">unique identifier payment</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="productdescription">Product Description Product Description</param>
        /// <param name="owner">owner Cardholder</param>
        /// <returns>transaction response</returns>
        public stdClass ExecutePurchase(string idpayuser, string tokenpayuser, string amount, string transreference, string currency, string productdescription, string owner)
        {
            stdClass result = new stdClass();
            r.Replace(idpayuser, "");
            r.Replace(tokenpayuser, "");
            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + amount + transreference + this.password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE;
                var ans = client.execute_purchase(this.merchantCode, this.terminal, idpayuser, 
                    tokenpayuser, ref amount, ref transreference, ref currency, signature, 
                    this.ipaddress, productdescription, owner, "0", out DS_MERCHANT_CARDCOUNTRY, out DS_RESPONSE, out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_MERCHANT_CURRENCY", currency);
                    result.data.Add("DS_MERCHANT_AUTHCODE", ans);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Execute a web service payment with DCC operational
        /// </summary>
        /// <param name="idpayuser">idpayuser User ID in PayTPV</param>
        /// <param name="tokenpayuser">tokenpayuser user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="productdescription">Product Description Product Description</param>
        /// <param name="owner">owner Cardholder</param>
        /// <returns>transaction response</returns>
        public stdClass ExecutePurchaseDcc(string idpayuser, string tokenpayuser, string amount, string transreference, string productdescription = "false", string owner = "false")
        {
            stdClass result = new stdClass();
            r.Replace(idpayuser, "");
            r.Replace(tokenpayuser, "");
            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + amount + transreference + this.password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_DCC_SESSION, DS_MERCHANT_DCC_CURRENCY, DS_MERCHANT_DCC_CURRENCYISO3, DS_MERCHANT_DCC_CURRENCYNAME;
                string DS_MERCHANT_DCC_EXCHANGE, DS_MERCHANT_DCC_AMOUNT, DS_MERCHANT_DCC_MARKUP, DS_MERCHANT_DCC_CARDCOUNTRY, DS_RESPONSE;
                var ans = client.execute_purchase_dcc(
                     this.merchantCode,
                     this.terminal,
                     idpayuser,
                     tokenpayuser,
                    ref amount,
                    ref transreference,
                     signature,
                     ip,
                     productdescription,
                     owner,
                    out  DS_MERCHANT_DCC_SESSION,
                    out  DS_MERCHANT_DCC_CURRENCY,
                    out  DS_MERCHANT_DCC_CURRENCYISO3,
                    out  DS_MERCHANT_DCC_CURRENCYNAME,
                    out  DS_MERCHANT_DCC_EXCHANGE,
                    out  DS_MERCHANT_DCC_AMOUNT,
                    out  DS_MERCHANT_DCC_MARKUP,
                    out  DS_MERCHANT_DCC_CARDCOUNTRY,
                    out  DS_RESPONSE,
                    out  DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_MERCHANT_DCC_CURRENCY", DS_MERCHANT_DCC_CURRENCY);
                    result.data.Add("DS_MERCHANT_DCC_SESSION", DS_MERCHANT_DCC_SESSION);
                    result.data.Add("DS_MERCHANT_DCC_CURRENCYISO3", DS_MERCHANT_DCC_CURRENCYISO3);
                    result.data.Add("DS_MERCHANT_DCC_CURRENCYNAME", DS_MERCHANT_DCC_CURRENCYNAME);
                    result.data.Add("DS_MERCHANT_DCC_EXCHANGE", DS_MERCHANT_DCC_EXCHANGE);
                    result.data.Add("DS_MERCHANT_DCC_AMOUNT", DS_MERCHANT_DCC_AMOUNT);
                    result.data.Add("DS_MERCHANT_DCC_MARKUP", DS_MERCHANT_DCC_MARKUP);
                    result.data.Add("DS_MERCHANT_DCC_CARDCOUNTRY", DS_MERCHANT_DCC_CARDCOUNTRY);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", ans);
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Confirm a payment by web service with DCC operational
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="dcccurrency">dcccurrency chosen currency transaction. It may be the product of PayTPV native or selected by the end user. The amount will be sent in execute_purchase_dcc if the same product and become if different.</param>
        /// <param name="dccsession">dccsession sent in the same session execute_purchase_dcc process.</param>
        /// <returns>transaction response</returns>
        public stdClass ConfirmPurchaseDcc(string transreference, string dcccurrency, string dccsession)
        {
            stdClass result = new stdClass();
            var signature = SHA1HashStringForUTF8String(this.merchantCode + this.terminal + transreference + dcccurrency + dccsession);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY;
                string DS_MERCHANT_CURRENCY, DS_RESPONSE;
                var ans = client.confirm_purchase_dcc(this.merchantCode, this.terminal, ref transreference, dcccurrency, dccsession, signature,
                    out  DS_MERCHANT_CURRENCY, out  DS_MERCHANT_AUTHCODE, out DS_MERCHANT_CARDCOUNTRY, out DS_RESPONSE, out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);
                    result.data.Add("DS_MERCHANT_CURRENCY", DS_MERCHANT_CURRENCY);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_AMOUNT", ans);
                    result.data.Add("DS_MERCHANT_CURRENCY", dcccurrency);
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Executes a return of a payment web service
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="authcode">AuthCode de la operación original a devolver</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <returns>transaction response</returns>
        public stdClass ExecuteRefund(string idpayuser, string tokenpayuser, string transreference, string currency, string authcode, string amount = null)
        {
            stdClass result = new stdClass();
            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + authcode + transreference + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID;

                var ans = client.execute_refund(this.merchantCode, this.terminal, idpayuser, tokenpayuser, ref authcode, ref transreference, ref currency, signature, ip, amount, out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", currency);
                    result.data.Add("DS_MERCHANT_AUTHCODE", authcode);
                    result.data.Add("DS_RESPONSE", ans);
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Create a subscription in PayTPV on a card.  IMPORTANTES !!! This direct input must be activated by PayTPV.
        /// In default input method card for PCI-DSS compliance should be CreateSubscriptionUrl or CreateSubscriptionToken
        /// </summary>
        /// <param name="pan">card number without spaces or dashes</param>
        /// <param name="expdate">EXPDATE expiry date of the card, expressed as "MMYY" (two-digit month and year in two digits)</param>
        /// <param name="cvv">CVC2 Card code</param>
        /// <param name="startdate">startdate date subscription start yyyy-mm-dd</param>
        /// <param name="enddate">enddate Date End subscription yyyy-mm-dd</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <returns>transaction response</returns>
        public stdClass CreateSubscription(string pan, string expdate, string cvv, string startdate, string enddate, string transreference, string periodicity, string amount, string currency)
        {
            stdClass result = new stdClass();
            r.Replace(pan, ""); r.Replace(expdate, ""); r.Replace(cvv, "");

            var signature = SHA1HashStringForUTF8String(this.merchantCode + pan + cvv + this.terminal + amount + currency + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_TOKEN_USER, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY;

                var ans = client.create_subscription(
                    this.merchantCode,
                    this.terminal,
                    pan,
                    expdate,
                    cvv,
                    startdate,
                    enddate,
                    ref transreference,
                    periodicity,
                    ref amount,
                    ref currency,
                    signature,
                    ip,
                     "0",
                    "DS_MERCHANT_CARDHOLDERNAME",
                     "DS_MERCHANT_SCORING",
                    out DS_TOKEN_USER,
                    out DS_MERCHANT_AUTHCODE,
                     out DS_MERCHANT_CARDCOUNTRY,
                     out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_TOKEN_USER", DS_TOKEN_USER);
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);
                    result.data.Add("DS_IDUSER", ans);
                    result.data.Add("DS_SUBSCRIPTION_AMOUNT", amount);
                    result.data.Add("DS_SUBSCRIPTION_ORDER", transreference);
                    result.data.Add("DS_SUBSCRIPTION_CURRENCY", currency);
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Modifies a subscription PayTPV on a card.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="startdate">startdate date subscription start yyyy-mm-dd</param>
        /// <param name="enddate">enddate Date End subscription yyyy-mm-dd</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="execute">EXECUTE If the registration process involves the payment of the first installment value DS_EXECUTE should be 1. If you only want to discharge from the subscription without being paid the first installment (will run with the parameters sent) its value must be 0.</param>
        /// <returns>transaction response</returns>
        public stdClass EditSubscription(string idpayuser, string tokenpayuser, string startdate, string enddate, string periodicity, string amount, string execute)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + amount + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_CARDCOUNTRY, DS_MERCHANT_AUTHCODE, DS_SUBSCRIPTION_CURRENCY;

                var ans = client.edit_subscription(this.merchantCode, this.terminal, ref idpayuser, ref tokenpayuser,
                    startdate, enddate, periodicity, ref amount, signature, execute, ip, out DS_SUBSCRIPTION_CURRENCY, out DS_MERCHANT_AUTHCODE,
                    out DS_MERCHANT_CARDCOUNTRY, out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();

                    result.data.Add("DS_IDUSER", idpayuser);
                    result.data.Add("DS_TOKEN_USER", tokenpayuser);
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);
                    result.data.Add("DS_IDUSER", ans);
                    result.data.Add("DS_SUBSCRIPTION_AMOUNT", amount);
                    result.data.Add("DS_SUBSCRIPTION_ORDER", ans);
                    result.data.Add("DS_SUBSCRIPTION_CURRENCY", DS_SUBSCRIPTION_CURRENCY);
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Deletes a subscription PayTPV on a card.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <returns>transaction response</returns>
        public stdClass RemoveSubscription(string idpayuser, string tokenpayuser)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID;

                var ans = client.remove_subscription(this.merchantCode, this.terminal, idpayuser, tokenpayuser, signature, ip, out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();

                    result.data.Add("DS_RESPONSE", ans);
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Create a subscription in PayTPV on a previously tokenized card.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="startdate">startdate date subscription start yyyy-mm-dd</param>
        /// <param name="enddate">enddate Date End subscription yyyy-mm-dd</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <returns>transaction response</returns>
        public stdClass CreateSubscriptionToken(string idpayuser, string tokenpayuser, string startdate, string enddate, string transreference, string periodicity, string amount, string currency)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + amount + currency + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_CARDCOUNTRY;

                var ans = client.create_subscription_token(this.merchantCode, this.terminal, ref idpayuser, ref tokenpayuser, startdate, enddate, ref transreference, periodicity, ref amount, ref currency,
                    signature, ip, "DS_MERCHANT_SCORING", out DS_MERCHANT_CARDCOUNTRY, out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_IDUSER", idpayuser);
                    result.data.Add("DS_TOKEN_USER", tokenpayuser);
                    result.data.Add("DS_SUBSCRIPTION_AMOUNT", amount);
                    result.data.Add("DS_SUBSCRIPTION_ORDER", transreference);
                    result.data.Add("DS_SUBSCRIPTION_CURRENCY", currency);
                    result.data.Add("DS_MERCHANT_AUTHCODE", ans);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);

                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Create a pre-authorization by web service
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="productdescription">Product Description Product Description</param>
        /// <param name="owner">owner Cardholder</param>
        /// <returns>transaction response</returns>
        public stdClass CreatePreauthorization(string idpayuser, string tokenpayuser, string amount, string transreference, string currency, string productdescription = "false", string owner = "false")
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + amount + transreference + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE, DS_MERCHANT_AMOUNT, DS_MERCHANT_ORDER, DS_MERCHANT_CURRENCY, DS_MERCHANT_AUTHCODE;

                var ans = client.create_preauthorization
                    (this.merchantCode, this.terminal, idpayuser, tokenpayuser, ref  amount, ref  transreference,
                    ref  currency, signature, ip, productdescription, owner, "DS_MERCHANT_SCORING", out  DS_MERCHANT_CARDCOUNTRY, out  DS_RESPONSE, out  DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", currency);
                    result.data.Add("DS_MERCHANT_AUTHCODE", ans);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);

                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Confirm a pre-authorization previously sent by web service
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        public stdClass PreauthorizationConfirm(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + transreference + amount + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE, DS_MERCHANT_AMOUNT, DS_MERCHANT_ORDER, DS_MERCHANT_CURRENCY;

                var ans = client.preauthorization_confirm
                    (this.merchantCode, this.terminal, idpayuser, tokenpayuser, ref amount, ref transreference, signature, ip,
                    out DS_MERCHANT_AUTHCODE, out DS_MERCHANT_CARDCOUNTRY, out DS_RESPONSE, out DS_ERROR_ID);


                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", ans);
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);

                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Cancels a pre-authorization previously sent by web service
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        public stdClass PreauthorizationCancel(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + transreference + amount + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE, DS_MERCHANT_AMOUNT, DS_MERCHANT_ORDER, DS_MERCHANT_CURRENCY;

                var ans = client.preauthorization_cancel(this.merchantCode, this.terminal, idpayuser, tokenpayuser, ref amount, ref transreference, signature, ip, out DS_MERCHANT_AUTHCODE, out DS_MERCHANT_CARDCOUNTRY,
                    out DS_RESPONSE, out DS_ERROR_ID);



                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", ans);
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);

                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Confirm deferred preauthorization by web service. Once and authorized an operation deferred pre-authorization can be confirmed for the effective recovery within 72 hours; after that date, deferred pre-authorizations lose their validity.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        public stdClass DeferredPreauthorizationConfirm(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + transreference + amount + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE, DS_MERCHANT_AMOUNT, DS_MERCHANT_ORDER, DS_MERCHANT_CURRENCY;

                var ans = client.deferred_preauthorization_confirm(this.merchantCode, this.terminal, idpayuser, tokenpayuser, ref amount, ref transreference, signature, ip, out DS_MERCHANT_AUTHCODE, out DS_MERCHANT_CARDCOUNTRY,
                    out DS_RESPONSE, out DS_ERROR_ID);



                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", ans);
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);

                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Cancels a deferred preauthorization by web service.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        public stdClass DeferredPreauthorizationCancel(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + idpayuser + tokenpayuser + this.terminal + transreference + amount + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE, DS_MERCHANT_AMOUNT, DS_MERCHANT_ORDER, DS_MERCHANT_CURRENCY;

                var ans = client.deferred_preauthorization_cancel(this.merchantCode, this.terminal, idpayuser, tokenpayuser, ref amount, ref transreference, signature, ip, out DS_MERCHANT_AUTHCODE, out DS_MERCHANT_CARDCOUNTRY,
                    out DS_RESPONSE, out DS_ERROR_ID);



                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", ans);
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);

                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        // NOT FOUND
        public stdClass ExecutePurchaseRToken(string amount, string transreference, string rtoken, string currency, string productdescription = "false")
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + this.terminal + amount + transreference + rtoken + password);
            var ip = this.ipaddress;

            try
            {

                var res = Api.PayTPVService.SoapAPIProxy.Execute_purchase_rtoken(signature,merchantCode,terminal, amount, transreference, rtoken, currency, productdescription);


                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE, DS_MERCHANT_AMOUNT, DS_MERCHANT_ORDER, DS_MERCHANT_CURRENCY;


                

                var ans = ""; // NNOT FOUND : execute_purchase_rtoken
                DS_ERROR_ID = "";
                DS_RESPONSE = "";
                DS_MERCHANT_AUTHCODE = "";
                DS_MERCHANT_CARDCOUNTRY = "";

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_RESPONSE", DS_RESPONSE);
                    result.data.Add("DS_MERCHANT_AMOUNT", amount);
                    result.data.Add("DS_MERCHANT_ORDER", transreference);
                    result.data.Add("DS_MERCHANT_CURRENCY", ans);
                    result.data.Add("DS_MERCHANT_AUTHCODE", DS_MERCHANT_AUTHCODE);
                    result.data.Add("DS_MERCHANT_CARDCOUNTRY", DS_MERCHANT_CARDCOUNTRY);

                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /**
    	* INTEGRATION BANKSTORE JET ---------------------------------------------- ----->
	    */


        /// <summary>
        /// Add a user by using web service BankStore JET
        /// </summary>
        /// <param name="jettoken">jettoken temporary user Token in PayTPV</param>
        /// <returns>transaction response</returns>
        public stdClass AddUserToken(string jettoken)
        {
            stdClass result = new stdClass();


            var signature = SHA1HashStringForUTF8String(this.merchantCode + jettoken + this.jetid + this.terminal + password);
            var ip = this.ipaddress;

            try
            {
                PAYTPV_BankStoreGatewayService client = new PAYTPV_BankStoreGatewayService();
                string DS_ERROR_ID, DS_MERCHANT_AUTHCODE, DS_MERCHANT_CARDCOUNTRY, DS_RESPONSE, DS_MERCHANT_AMOUNT, DS_MERCHANT_ORDER, DS_MERCHANT_CURRENCY, DS_TOKEN_USER;

                var ans = client.add_user_token(merchantCode, this.terminal, jettoken, this.jetid, signature, ip, out DS_TOKEN_USER, out DS_ERROR_ID);

                if (Convert.ToInt32(DS_ERROR_ID) > 0)
                {
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "KO";
                }
                else
                {
                    result.data = new Dictionary<string, object>();
                    result.data.Add("DS_IDUSER", ans);
                    result.data.Add("DS_TOKEN_USER", DS_TOKEN_USER);
                    result.DS_ERROR_ID = DS_ERROR_ID;
                    result.RESULT = "OK";
                }
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a execute_purchase under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass ExecutePurchaseUrl(string transreference, string amount, string currency, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 1;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;

                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }

                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a execute_purchase_token under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="iduser">iduser unique identifier system registered user.</param>
        /// <param name="tokenuser">tokenuser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass ExecutePurchaseTokenUrl(string transreference, string amount, string currency, string iduser, string tokenuser,
            string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 109;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;
                operation.IdUser = iduser;
                operation.TokenUser = tokenuser;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }

                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a add_user under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="lang">language transaction literals</param>
        /// <returns>transaction response</returns>
        public stdClass AddUserUrl(string transreference, string lang = "ES")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 107;
                operation.Reference = transreference;
                operation.Language = lang;

                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a create_subscription under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="startdate">startdate date subscription start yyyy-mm-dd</param>
        /// <param name="enddate">enddate Date End subscription yyyy-mm-dd</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass CreateSubscriptionUrl(string transreference, string amount, string currency, string startdate, string enddate,
            string periodicity, string lang = "ES", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 9;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Periodicity = periodicity;
                operation.StartDate = startdate;
                operation.EndDate = enddate;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }

                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a create_subscription_token under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="startdate">startdate date subscription start yyyy-mm-dd</param>
        /// <param name="enddate">enddate Date End subscription yyyy-mm-dd</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="iduser">iduser unique identifier system registered user.</param>
        /// <param name="tokenuser">tokenuser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass CreateSubscriptionTokenUrl(string transreference, string amount, string currency, string startdate, string enddate,
            string periodicity, string iduser, string tokenuser, string lang = "ES", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 110;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Periodicity = periodicity;
                operation.StartDate = startdate;
                operation.EndDate = enddate;
                operation.IdUser = iduser;
                operation.TokenUser = tokenuser;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }

                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a create_preauthorization under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass CreatePreauthorizationUrl(string transreference, string amount, string currency, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 3;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }

                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a preauthorization_confirm under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="iduser">iduser unique identifier system registered user.</param>
        /// <param name="tokenuser">tokenuser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass PreauthorizationConfirmUrl(string transreference, string amount, string currency, string iduser,
            string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 6;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;
                operation.IdUser = iduser;
                operation.TokenUser = tokenuser;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }
                var check_user_exist = this.InfoUser(operation.IdUser, operation.TokenUser);
                if (Convert.ToInt32(check_user_exist.DS_ERROR_ID) != 0)
                {
                    return check_user_exist;
                }


                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a preauthorization_cancel under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="iduser">iduser unique identifier system registered user.</param>
        /// <param name="tokenuser">tokenuser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass PreauthorizationCancelUrl(string transreference, string amount, string currency, string iduser,
            string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 4;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;
                operation.IdUser = iduser;
                operation.TokenUser = tokenuser;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }
                var check_user_exist = this.InfoUser(operation.IdUser, operation.TokenUser);
                if (Convert.ToInt32(check_user_exist.DS_ERROR_ID) != 0)
                {
                    return check_user_exist;
                }


                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a execute_preauthorization_token under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="iduser">iduser unique identifier system registered user.</param>
        /// <param name="tokenuser">tokenuser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass ExecutePreauthorizationTokenUrl(string transreference, string amount, string currency, string iduser,
            string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 111;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;
                operation.IdUser = iduser;
                operation.TokenUser = tokenuser;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }
                var check_user_exist = this.InfoUser(operation.IdUser, operation.TokenUser);
                if (Convert.ToInt32(check_user_exist.DS_ERROR_ID) != 0)
                {
                    return check_user_exist;
                }


                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a deferred_preauthorization under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass DeferredPreauthorizationUrl(string transreference, string amount, string currency, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 13;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;


                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }
						
			   
                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a deferred_preauthorization_confirm under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="iduser">iduser unique identifier system registered user.</param>
        /// <param name="tokenuser">tokenuser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass DeferredPreauthorizationConfirmUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 16;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;
                operation.IdUser = iduser;
                operation.TokenUser = tokenuser;

                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }
                var check_user_exist = this.InfoUser(operation.IdUser, operation.TokenUser);
                if (Convert.ToInt32(check_user_exist.DS_ERROR_ID) != 0)
                {
                    return check_user_exist;
                }


                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        /// <summary>
        /// Returns the URL to launch a deferred_preauthorization_cancel under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="iduser">iduser unique identifier system registered user.</param>
        /// <param name="tokenuser">tokenuser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3d">secure3d Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        public stdClass DeferredPreauthorizationCancelUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            stdClass result = new stdClass();
            OperationData operation = new OperationData();
            try
            {
                operation.Type = 14;
                operation.Reference = transreference;
                operation.Amount = amount;
                operation.Currency = currency;
                operation.Language = lang;
                operation.Concept = description;
                operation.IdUser = iduser;
                operation.TokenUser = tokenuser;

                if (secure3d != "false")
                {
                    operation.Secure3D = secure3d;
                }
                var check_user_exist = this.InfoUser(operation.IdUser, operation.TokenUser);
                if (Convert.ToInt32(check_user_exist.DS_ERROR_ID) != 0)
                {
                    return check_user_exist;
                }


                operation.Hash = this.GenerateHash(operation, operation.Type); //generate hash
                string lastrequest = ComposeURLParams(operation, operation.Type);

                result = CheckUrlError(lastrequest);
                result.data["URL_REDIRECT"] = (this.endpointurl + lastrequest);

                if (Convert.ToInt32(result.DS_ERROR_ID) > 0)
                {
                    result.RESULT = "KO";

                }
                else
                {
                    result.RESULT = "OK";
                }
                return result;
            }
            catch (Exception e)
            {
                result.DS_ERROR_ID = "1011";
                result.RESULT = "KO";
                return result;
            }
            return result;
        }

        //------------------------------------------------------------------------------------

        private string GenerateHash(OperationData operationdata, int operationtype)
        {
            string hash = "false";
            using (MD5 md5Hash = MD5.Create())
            {
                switch (operationtype)
                {
                    case 1: // Authorization (execute_purchase)
                        hash = md5(md5Hash, this.merchantCode + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        operationdata.Currency + md5(md5Hash, this.password));
                        break;
                    case 3:// Preauthorization
                        hash = md5(md5Hash, this.merchantCode + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        operationdata.Currency + md5(md5Hash, this.password));
                        break;
                    case 6: // Confirmación de Preauthorization
                        hash = md5(md5Hash, this.merchantCode + operationdata.IdUser + operationdata.TokenUser + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        md5(md5Hash, this.password));
                        break;
                    case 4: // Cancelación de Preauthorization
                        hash = md5(md5Hash, this.merchantCode + operationdata.IdUser + operationdata.TokenUser + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        md5(md5Hash, this.password));
                        break;
                    case 9:// Subscription
                        hash = md5(md5Hash, this.merchantCode + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        operationdata.Currency + md5(md5Hash, this.password));
                        break;

                    case 107: // Add_user
                        hash = md5(md5Hash, this.merchantCode + this.terminal + operationtype + operationdata.Reference + md5(md5Hash, this.password));
                        break;
                    case 109: // execute_purchase_token
                        hash = md5(md5Hash, this.merchantCode + operationdata.IdUser + operationdata.TokenUser + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                         operationdata.Currency + md5(md5Hash, this.password));
                        break;
                    case 110: // create_subscription_token
                        hash = md5(md5Hash, this.merchantCode + operationdata.IdUser + operationdata.TokenUser + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                         operationdata.Currency + md5(md5Hash, this.password));
                        break;
                    case 111:// create_preauthorization_token
                        hash = md5(md5Hash, this.merchantCode + operationdata.IdUser + operationdata.TokenUser + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                         operationdata.Currency + md5(md5Hash, this.password));
                        break;
                    case 13:// Preauthorization Diferida
                        hash = md5(md5Hash, this.merchantCode + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        operationdata.Currency + md5(md5Hash, this.password));
                        break;
                    case 16: // Confirmación de Preauthorization Diferida
                        hash = md5(md5Hash, this.merchantCode + operationdata.IdUser + operationdata.TokenUser + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        md5(md5Hash, this.password));
                        break;
                    case 14: // Cancelación de Preauthorization Diferida
                        hash = md5(md5Hash, this.merchantCode + operationdata.IdUser + operationdata.TokenUser + this.terminal + operationtype + operationdata.Reference + operationdata.Amount +
                        md5(md5Hash, this.password));
                        break;
                }
            }
            return hash;
        }

        private string ComposeURLParams(OperationData operationdata, int operationtype)
        {
            string secureurlhash = "false";
            SortedDictionary<string, string> data = new SortedDictionary<string, string>();
            data["MERCHANT_MERCHANTCODE"] = this.merchantCode;
            data["MERCHANT_TERMINAL"] = this.terminal;
            data["OPERATION"] = operationtype.ToString();
            data["LANGUAGE"] = operationdata.Language;
            data["MERCHANT_MERCHANTSIGNATURE"] = operationdata.Hash;
            data["URLOK"] = operationdata.UrlOk;
            data["URLKO"] = operationdata.UrlKo;
            data["MERCHANT_ORDER"] = operationdata.Reference;
            if (operationdata.Secure3D != "false")
            {
                data["3DSECURE"] = operationdata.Secure3D;
            }
            data["MERCHANT_AMOUNT"] = operationdata.Amount;
            if (operationdata.Concept != "")
            {
                data["MERCHANT_PRODUCTDESCRIPTION"] = operationdata.Concept;
            }

            if ((int)operationtype == 1)
            {					// Authorization (execute_purchase)
                data["MERCHANT_CURRENCY"] = operationdata.Currency;
                data["MERCHANT_SCORING"] = operationdata.Scoring;
            }
            else if ((int)operationtype == 3)
            {			// Preauthorization
                data["MERCHANT_CURRENCY"] = operationdata.Currency;
                data["MERCHANT_SCORING"] = operationdata.Scoring;
            }
            else if ((int)operationtype == 6)
            {			// Confirmación de Preauthorization
                data["IDUSER"] = operationdata.IdUser;
                data["TOKEN_USER"] = operationdata.TokenUser;
            }
            else if ((int)operationtype == 4)
            {			// Cancelación de Preauthorization
                data["IDUSER"] = operationdata.IdUser;
                data["TOKEN_USER"] = operationdata.TokenUser;
            }
            else if ((int)operationtype == 9)
            {			// Subscription
                data["MERCHANT_CURRENCY"] = operationdata.Currency;
                data["SUBSCRIPTION_STARTDATE"] = operationdata.StartDate;
                data["SUBSCRIPTION_ENDDATE"] = operationdata.EndDate;
                data["SUBSCRIPTION_PERIODICITY"] = operationdata.Periodicity;
                data["MERCHANT_SCORING"] = operationdata.Scoring;
            }
            else if ((int)operationtype == 109)
            {			// execute_purchase_token
                data["IDUSER"] = operationdata.IdUser;
                data["TOKEN_USER"] = operationdata.TokenUser;
                data["MERCHANT_CURRENCY"] = operationdata.Currency;
                data["MERCHANT_SCORING"] = operationdata.Scoring;
            }
            else if ((int)operationtype == 110)
            {			// create_subscription_token
                data["IDUSER"] = operationdata.IdUser;
                data["TOKEN_USER"] = operationdata.TokenUser;
                data["MERCHANT_CURRENCY"] = operationdata.Currency;
                data["SUBSCRIPTION_STARTDATE"] = operationdata.StartDate;
                data["SUBSCRIPTION_ENDDATE"] = operationdata.EndDate;
                data["SUBSCRIPTION_PERIODICITY"] = operationdata.Periodicity;
                data["MERCHANT_SCORING"] = operationdata.Scoring;
            }
            else if ((int)operationtype == 111)
            {			// create_preauthorization_token
                data["IDUSER"] = operationdata.IdUser;
                data["TOKEN_USER"] = operationdata.TokenUser;
                data["MERCHANT_SCORING"] = operationdata.Scoring;
                data["MERCHANT_CURRENCY"] = operationdata.Currency;
            }
            else if ((int)operationtype == 13)
            {			// Deferred Preauthorization
                data["MERCHANT_CURRENCY"] = operationdata.Currency;
                data["MERCHANT_SCORING"] = operationdata.Scoring;
            }
            else if ((int)operationtype == 16)
            {			// Deferred Confirmación de Preauthorization
                data["IDUSER"] = operationdata.IdUser;
                data["TOKEN_USER"] = operationdata.TokenUser;
            }
            else if ((int)operationtype == 14)
            {			// Deferred  Cancelación de Preauthorization
                data["IDUSER"] = operationdata.IdUser;
                data["TOKEN_USER"] = operationdata.TokenUser;
            }

            string content = "";
            foreach (var k in data.Keys)
            {
                if (content != "")
                    content += @"&";

                content += HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(data[k]);
            }
            MD5 mdhash = MD5.Create();

            data["VHASH"] = SHA512HashStringForUTF8String(md5(mdhash, content + md5(mdhash, this.password)));
            //krsort(data);
            //var dd = new Dictionary<string, string>();
            data.Reverse();
            foreach (var k in data.Keys)
            {
                if (secureurlhash != "")
                    secureurlhash += @"&";

                secureurlhash += HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(data[k]);
            }

            return secureurlhash;
        }

        private stdClass CheckUrlError(string urlgen)
        {
            stdClass response = new stdClass();
            response.DS_ERROR_ID = "1023";
            using (WebClient web = new WebClient())
            {
                try
                {
                    string data = web.DownloadString(urlgen);
                    if (data.Contains("Error"))
                    {
                        response.DS_ERROR_ID = "1021";
                    }
                    else
                    {
                        response.DS_ERROR_ID = "0";
                    }
                }
                catch (HttpException he)
                {
                    response.DS_ERROR_ID = "1021";
                    response.RESULT = "KO";
                }
                catch (Exception ee)
                {
                    response.DS_ERROR_ID = "1021";
                }


            }
            return response;
        }

        #region Utility Methods
        /// <summary>
        /// MD5 hash creator
        /// </summary>
        /// <param name="md5Hash">md5 Hash</param>
        /// <param name="input">input string to be converted to hashcode</param>
        /// <returns>transaction response</returns>
        static string md5(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        private static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }
        private static string SHA512HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA512.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        private static string HexStringFromBytes(byte[] bytes)
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

    public class stdClass
    {
        public string RESULT { get; set; }
        public string DS_ERROR_ID { get; set; }
        public Dictionary<string, object> data { get; set; }
    }

    public class OperationData
    {
        public int Type { get; set; }
        public string Language { get; set; }
        public string Hash { get; set; }
        public string UrlOk { get; set; }
        public string UrlKo { get; set; }
        public string Reference { get; set; }
        public string Secure3D { get; set; }
        public string Amount { get; set; }
        public string Concept { get; set; }
        public string Currency { get; set; }
        public string Scoring { get; set; }
        public string IdUser { get; set; }
        public string TokenUser { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Periodicity { get; set; }

    }
}