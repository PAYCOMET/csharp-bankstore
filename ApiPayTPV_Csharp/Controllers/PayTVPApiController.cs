using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiPayTPV_Csharp.Controllers
{
    /// <summary>
    /// PayTVP Api
    /// </summary>
    [RoutePrefix("api/PayTVP")]
    public class PayTVPApiController : ApiController
    {
        private string merchantCode = "1br6407g";
        private string terminal = "4473";
        private string password = "fnz5kc7pxw0y4hb963qt";
        private string endpoint;
        private string endpointurl;
        private string jetid = "";
        private string ipaddress = "1.1.1.1";

        public PayTVPApiController()
        {
            
        }

        /// <summary>
        /// GEt user information
        /// </summary>
        /// <param name="idpayuser">Id of user</param>
        /// <param name="tokenuser">Toekn for user </param>
        /// <returns></returns>
        [Route("InfoUser")]
        [HttpGet]
        public stdClass GetUserInfo(string idpayuser, string tokenuser)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.InfoUser(idpayuser, tokenuser);
            return tt;
        }

        /// <summary>
        /// Add a card to PayTPV.  IMPORTANTES !!! This direct input must be activated by PayTPV.
	    /// In default input method card for PCI-DSS compliance should be AddUserUrl or AddUserToken (method used by BankStore JET)
        /// </summary>
        /// <param name="pan">card number without spaces or dashes</param>
        /// <param name="expdate">EXPDATE expiry date of the card, expressed as "MMYY" (two-digit month and year in two digits)</param>
        /// <param name="cvv">CVC2 Card code</param>
        /// <returns>transaction response</returns>
        [HttpGet]
        [Route("AddUser")]
        public stdClass AddUser(string pan, string expdate, string cvv)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.AddUser(pan, expdate, cvv);
            return tt;
        }


        /// <summary>
        /// Removes a user through call soap PayTPV
        /// </summary>
        /// <param name="idpayuser">user ID PayTPV</param>
        /// <param name="tokenpayuser">User Token PayTPV</param>
        /// <returns>Object A transaction response</returns>
        [HttpPost]
        [Route("RemoveUser")]
        public stdClass RemoveUser(string idpayuser, string tokenpayuser)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.RemoveUser(idpayuser, tokenpayuser);
            return tt;
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
        /// 
        [HttpPost]
        [Route("ExecutePurchase")]
        public stdClass ExecutePurchase(string idpayuser, string tokenpayuser, string amount, string transreference, string currency, string productdescription, string owner)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ExecutePurchase(idpayuser, tokenpayuser, amount, transreference, currency, productdescription, owner);
            return tt;
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
        /// 
        [HttpPost]
        [Route("ExecutePurchaseDcc")]
        public stdClass ExecutePurchaseDcc(string idpayuser, string tokenpayuser, string amount, string transreference, string productdescription = "false", string owner = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ExecutePurchaseDcc(idpayuser, tokenpayuser, amount, transreference, productdescription, owner);
            return tt;
        }


        /// <summary>
        /// Confirm a payment by web service with DCC operational
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="dcccurrency">dcccurrency chosen currency transaction. It may be the product of PayTPV native or selected by the end user. The amount will be sent in execute_purchase_dcc if the same product and become if different.</param>
        /// <param name="dccsession">dccsession sent in the same session execute_purchase_dcc process.</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ConfirmPurchaseDcc")]
        public stdClass ConfirmPurchaseDcc(string transreference, string dcccurrency, string dccsession)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ConfirmPurchaseDcc(transreference, dcccurrency, dccsession);
            return tt;
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
        /// 
        [HttpPost]
        [Route("ExecuteRefund")]
        public stdClass ExecuteRefund(string idpayuser, string tokenpayuser, string transreference, string currency, string authcode, string amount = null)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ExecuteRefund(idpayuser, tokenpayuser, transreference, currency, authcode, amount);
            return tt;
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
        /// 
        [HttpPost]
        [Route("CreateSubscription")]
        public stdClass CreateSubscription(string pan, string expdate, string cvv, string startdate, string enddate, string transreference, string periodicity, string amount, string currency)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.CreateSubscription(pan, expdate, cvv, startdate, enddate, transreference, periodicity, amount, currency);
            return tt;
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
        /// 
        [HttpPost]
        [Route("EditSubscription")]
        public stdClass EditSubscription(string idpayuser, string tokenpayuser, string startdate, string enddate, string periodicity, string amount, string execute)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.EditSubscription(idpayuser, tokenpayuser, startdate, enddate, periodicity, amount, execute);
            return tt;
        }

        /// <summary>
        /// Deletes a subscription PayTPV on a card.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("RemoveSubscription")]
        public stdClass RemoveSubscription(string idpayuser, string tokenpayuser)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.RemoveSubscription(idpayuser, tokenpayuser);
            return tt;
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
        /// 
        [HttpPost]
        [Route("CreateSubscriptionToken")]
        public stdClass CreateSubscriptionToken(string idpayuser, string tokenpayuser, string startdate, string enddate, string transreference, string periodicity, string amount, string currency)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.CreateSubscriptionToken(idpayuser, tokenpayuser, startdate, enddate, transreference, periodicity, amount, currency);
            return tt;
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
        /// 
        [HttpPost]
        [Route("CreatePreauthorization")]
        public stdClass CreatePreauthorization(string idpayuser, string tokenpayuser, string amount, string transreference, string currency, string productdescription = "false", string owner = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.CreatePreauthorization(idpayuser, tokenpayuser, amount, transreference, currency, productdescription, owner);
            return tt;
        }

        /// <summary>
        /// Confirm a pre-authorization previously sent by web service
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("PreauthorizationConfirm")]
        public stdClass PreauthorizationConfirm(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.PreauthorizationConfirm(idpayuser, tokenpayuser, amount, transreference);
            return tt;
        }

        /// <summary>
        /// Cancels a pre-authorization previously sent by web service
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("PreauthorizationCancel")]
        public stdClass PreauthorizationCancel(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.PreauthorizationCancel(idpayuser, tokenpayuser, amount, transreference);
            return tt;
        }

        /// <summary>
        /// Confirm deferred preauthorization by web service. Once and authorized an operation deferred pre-authorization can be confirmed for the effective recovery within 72 hours; after that date, deferred pre-authorizations lose their validity.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationConfirm")]
        public stdClass DeferredPreauthorizationConfirm(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.DeferredPreauthorizationConfirm(idpayuser, tokenpayuser, amount, transreference);
            return tt;
        }

        /// <summary>
        /// Cancels a deferred preauthorization by web service.
        /// </summary>
        /// <param name="idpayuser">User ID in PayTPV</param>
        /// <param name="tokenpayuser">user Token in PayTPV</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationCancel")]
        public stdClass DeferredPreauthorizationCancel(string idpayuser, string tokenpayuser, string amount, string transreference)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.DeferredPreauthorizationCancel(idpayuser, tokenpayuser, amount, transreference);
            return tt;
        }

        /// <summary>
        /// Add a user by using web service BankStore JET
        /// </summary>
        /// <param name="jettoken">jettoken temporary user Token in PayTPV</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("AddUserToken")]
        public stdClass AddUserToken(string jettoken)
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.AddUserToken(jettoken);
            return tt;
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
        /// 
        [HttpPost]
        [Route("ExecutePurchaseUrl")]
        public stdClass ExecutePurchaseUrl(string transreference, string amount, string currency, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ExecutePurchaseUrl(transreference, amount, currency, lang, description, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("ExecutePurchaseTokenUrl")]
        public stdClass ExecutePurchaseTokenUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ExecutePurchaseTokenUrl(transreference, amount, currency, iduser, tokenuser, lang, description, secure3d);
            return tt;
        }

        /// <summary>
        /// Returns the URL to launch a add_user under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transreference">transreference unique identifier payment</param>
        /// <param name="lang">language transaction literals</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("AddUserUrl")]
        public stdClass AddUserUrl(string transreference, string lang = "ES")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.AddUserUrl(transreference, lang);
            return tt;
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
        /// 
        [HttpPost]
        [Route("CreateSubscriptionUrl")]
        public stdClass CreateSubscriptionUrl(string transreference, string amount, string currency, string startdate, string enddate, string periodicity, string lang = "ES", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.CreateSubscriptionUrl(transreference, amount, currency, startdate, enddate, periodicity, lang, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("CreateSubscriptionTokenUrl")]
        public stdClass CreateSubscriptionTokenUrl(string transreference, string amount, string currency, string startdate, string enddate, string periodicity, string iduser, string tokenuser, string lang = "ES", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.CreateSubscriptionTokenUrl(transreference, amount, currency, startdate, enddate, periodicity, iduser, tokenuser, lang, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("CreatePreauthorizationUrl")]
        public stdClass CreatePreauthorizationUrl(string transreference, string amount, string currency, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.CreatePreauthorizationUrl(transreference, amount, currency, lang, description, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("PreauthorizationConfirmUrl")]
        public stdClass PreauthorizationConfirmUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.PreauthorizationConfirmUrl(transreference, amount, currency, iduser, tokenuser, lang, description, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("PreauthorizationCancelUrl")]
        public stdClass PreauthorizationCancelUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.PreauthorizationCancelUrl(transreference, amount, currency, iduser, tokenuser, lang, description, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("ExecutePreauthorizationTokenUrl")]
        public stdClass ExecutePreauthorizationTokenUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ExecutePreauthorizationTokenUrl(transreference, amount, currency, iduser, tokenuser, lang, description, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationUrl")]
        public stdClass DeferredPreauthorizationUrl(string transreference, string amount, string currency, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.DeferredPreauthorizationUrl(transreference, amount, currency, lang, description, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationConfirmUrl")]
        public stdClass DeferredPreauthorizationConfirmUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.DeferredPreauthorizationConfirmUrl(transreference, amount, currency, iduser, tokenuser, lang, description, secure3d);
            return tt;
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
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationCancelUrl")]
        public stdClass DeferredPreauthorizationCancelUrl(string transreference, string amount, string currency, string iduser, string tokenuser, string lang = "ES", string description = "false", string secure3d = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.DeferredPreauthorizationCancelUrl(transreference, amount, currency, iduser, tokenuser, lang, description, secure3d);
            return tt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="transreference"></param>
        /// <param name="rtoken"></param>
        /// <param name="currency"></param>
        /// <param name="productdescription"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ExecutePurchaseRToken")]
        public stdClass ExecutePurchaseRToken(string amount, string transreference, string rtoken, string currency, string productdescription = "false")
        {
            Paytpv_Bankstore service = new Paytpv_Bankstore(merchantCode, terminal, password, ipaddress);
            var tt = service.ExecutePurchaseRToken(amount, transreference, rtoken, currency, productdescription);
            return tt;
        }

    }
}
