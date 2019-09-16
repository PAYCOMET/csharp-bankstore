using System.Web.Http;
using System.Configuration;
using Api.PayCOMETService;
using Api.PayCOMETService.Responses;

namespace ApiPayCOMET_Csharp.Controllers
{
    /// <summary>
    /// PAYCOMET Api
    /// </summary>
    [RoutePrefix("api/PAYCOMET")]
    public class PayCOMETApiController : ApiController
    {
        private string merchantCode = ConfigurationManager.AppSettings["MerchantCodeTest"];
        private string terminal = ConfigurationManager.AppSettings["TerminalTest"];
        private string password = ConfigurationManager.AppSettings["PasswordTest"];
        private string ipAddress = ConfigurationManager.AppSettings["IPTest"];
        private string jetId = ConfigurationManager.AppSettings["JetIdTest"];

        public PayCOMETApiController()
        {

        }

        /// <summary>
        /// GEt user information
        /// </summary>
        /// <param name="idPayUser">Id of user</param>
        /// <param name="tokenUser">Toekn for user </param>
        /// <returns></returns>
        [Route("InfoUser")]
        [HttpGet]
        public BankstoreServResponse GetUserInfo(string idPayUser, string tokenUser)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.InfoUser(idPayUser, tokenUser);
            return servResponse;
        }

        /// <summary>
        /// Add a card to PayCOMET.  IMPORTANTES !!! This direct input must be activated by PayCOMET.
        /// In default input method card for PCI-DSS compliance should be AddUserUrl or AddUserToken (method used by BankStore JET)
        /// </summary>
        /// <param name="pan">card number without spaces or dashes</param>
        /// <param name="expDate">EXPDATE expiry date of the card, expressed as "MMYY" (two-digit month and year in two digits)</param>
        /// <param name="cvv">CVC2 Card code</param>
        /// <returns>transaction response</returns>
        [HttpGet]
        [Route("AddUser")]
        public BankstoreServResponse AddUser(string pan, string expDate, string cvv)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.AddUser(pan, expDate, cvv);
            return servResponse;
        }


        /// <summary>
        /// Removes a user through call soap PayCOMET
        /// </summary>
        /// <param name="idPayUser">user ID PayCOMET</param>
        /// <param name="tokenPayUser">User Token PayCOMET</param>
        /// <returns>Object A transaction response</returns>
        [HttpPost]
        [Route("RemoveUser")]
        public BankstoreServResponse RemoveUser(string idPayUser, string tokenPayUser)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.RemoveUser(idPayUser, tokenPayUser);
            return servResponse;
        }

        /// <summary>
        /// Execute a web service payment
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">unique identifier payment</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="productDescription">Product Description Product Description</param>
        /// <param name="owner">owner Cardholder</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ExecutePurchase")]
        public BankstoreServResponse ExecutePurchase(string idPayUser, string tokenPayUser, string amount, string transReference, string currency, string productDescription = null, string owner = null, string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ExecutePurchase(idPayUser, tokenPayUser, amount, transReference, currency, productDescription, owner, scoring);
            return servResponse;
        }

        /// <summary>
        /// Execute a web service payment with DCC operational
        /// </summary>
        /// <param name="idPayUser">idPayUser User ID in PayCOMET</param>
        /// <param name="tokenPayUser">tokenPayUser user Token in PayCOMET</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="productDescription">Product Description Product Description</param>
        /// <param name="owner">owner Cardholder</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ExecutePurchaseDcc")]
        public BankstoreServResponse ExecutePurchaseDcc(string idPayUser, string tokenPayUser, string amount, string transReference, string productDescription = null, string owner = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ExecutePurchaseDcc(idPayUser, tokenPayUser, amount, transReference, productDescription, owner);
            return servResponse;
        }


        /// <summary>
        /// Confirm a payment by web service with DCC operational
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="dccCurrency">dccCurrency chosen currency transaction. It may be the product of PayCOMET native or selected by the end user. The amount will be sent in execute_purchase_dcc if the same product and become if different.</param>
        /// <param name="dccSession">dccSession sent in the same session execute_purchase_dcc process.</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ConfirmPurchaseDcc")]
        public BankstoreServResponse ConfirmPurchaseDcc(string transReference, string dccCurrency, string dccSession)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ConfirmPurchaseDcc(transReference, dccCurrency, dccSession);
            return servResponse;
        }

        /// <summary>
        /// Executes a return of a payment web service
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="authCode">AuthCode de la operación original a devolver</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ExecuteRefund")]
        public BankstoreServResponse ExecuteRefund(string idPayUser, string tokenPayUser, string transReference, string currency, string authCode, string amount = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ExecuteRefund(idPayUser, tokenPayUser, transReference, currency, authCode, amount);
            return servResponse;
        }

        /// <summary>
        /// Create a subscription in PayCOMET on a card.  IMPORTANTES !!! This direct input must be activated by PayCOMET.
        /// In default input method card for PCI-DSS compliance should be CreateSubscriptionUrl or CreateSubscriptionToken
        /// </summary>
        /// <param name="pan">card number without spaces or dashes</param>
        /// <param name="expDate">EXPDATE expiry date of the card, expressed as "MMYY" (two-digit month and year in two digits)</param>
        /// <param name="cvv">CVC2 Card code</param>
        /// <param name="startDate">startDate date subscription start yyyy-mm-dd</param>
        /// <param name="endDate">endDate Date End subscription yyyy-mm-dd</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("CreateSubscription")]
        public BankstoreServResponse CreateSubscription(string pan, string expDate, string cvv, string startDate, string endDate, string transReference, string periodicity, string amount, string currency, string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.CreateSubscription(pan, expDate, cvv, startDate, endDate, transReference, periodicity, amount, currency, scoring);
            return servResponse;
        }

        /// <summary>
        /// Modifies a subscription PayCOMET on a card.
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="startDate">startDate date subscription start yyyy-mm-dd</param>
        /// <param name="endDate">endDate Date End subscription yyyy-mm-dd</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="execute">EXECUTE If the registration process involves the payment of the first installment value DS_EXECUTE should be 1. If you only want to discharge from the subscription without being paid the first installment (will run with the parameters sent) its value must be 0.</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("EditSubscription")]
        public BankstoreServResponse EditSubscription(string idPayUser, string tokenPayUser, string startDate, string endDate, string periodicity, string amount, string execute)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.EditSubscription(idPayUser, tokenPayUser, startDate, endDate, periodicity, amount, execute);
            return servResponse;
        }

        /// <summary>
        /// Deletes a subscription PayCOMET on a card.
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("RemoveSubscription")]
        public BankstoreServResponse RemoveSubscription(string idPayUser, string tokenPayUser)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.RemoveSubscription(idPayUser, tokenPayUser);
            return servResponse;
        }

        /// <summary>
        /// Create a subscription in PayCOMET on a previously tokenized card.
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="startDate">startDate date subscription start yyyy-mm-dd</param>
        /// <param name="endDate">endDate Date End subscription yyyy-mm-dd</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("CreateSubscriptionToken")]
        public BankstoreServResponse CreateSubscriptionToken(string idPayUser, string tokenPayUser, string startDate, string endDate, string transReference, string periodicity, string amount, string currency, string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.CreateSubscriptionToken(idPayUser, tokenPayUser, startDate, endDate, transReference, periodicity, amount, currency, scoring);
            return servResponse;
        }

        /// <summary>
        /// Create a pre-authorization by web service
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="productDescription">Product Description Product Description</param>
        /// <param name="owner">owner Cardholder</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("CreatePreauthorization")]
        public BankstoreServResponse CreatePreauthorization(string idPayUser, string tokenPayUser, string amount, string transReference, string currency, string productDescription = null, string owner = null, string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.CreatePreauthorization(idPayUser, tokenPayUser, amount, transReference, currency, productDescription, owner, scoring);
            return servResponse;
        }

        /// <summary>
        /// Confirm a pre-authorization previously sent by web service
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("PreauthorizationConfirm")]
        public BankstoreServResponse PreauthorizationConfirm(string idPayUser, string tokenPayUser, string amount, string transReference)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.PreauthorizationConfirm(idPayUser, tokenPayUser, amount, transReference);
            return servResponse;
        }

        /// <summary>
        /// Cancels a pre-authorization previously sent by web service
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("PreauthorizationCancel")]
        public BankstoreServResponse PreauthorizationCancel(string idPayUser, string tokenPayUser, string amount, string transReference)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.PreauthorizationCancel(idPayUser, tokenPayUser, amount, transReference);
            return servResponse;
        }

        /// <summary>
        /// Confirm deferred preauthorization by web service. Once and authorized an operation deferred pre-authorization can be confirmed for the effective recovery within 72 hours; after that date, deferred pre-authorizations lose their validity.
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationConfirm")]
        public BankstoreServResponse DeferredPreauthorizationConfirm(string idPayUser, string tokenPayUser, string amount, string transReference)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.DeferredPreauthorizationConfirm(idPayUser, tokenPayUser, amount, transReference);
            return servResponse;
        }

        /// <summary>
        /// Cancels a deferred preauthorization by web service.
        /// </summary>
        /// <param name="idPayUser">User ID in PayCOMET</param>
        /// <param name="tokenPayUser">user Token in PayCOMET</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationCancel")]
        public BankstoreServResponse DeferredPreauthorizationCancel(string idPayUser, string tokenPayUser, string amount, string transReference)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.DeferredPreauthorizationCancel(idPayUser, tokenPayUser, amount, transReference);
            return servResponse;
        }

        /// <summary>
        /// Add a user by using web service BankStore JET
        /// </summary>
        /// <param name="jetToken">jetToken temporary user Token in PayCOMET</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("AddUserToken")]
        public BankstoreServResponse AddUserToken(string jetToken)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress, jetId);
            var servResponse = serviceAPI.AddUserToken(jetToken);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a execute_purchase under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ExecutePurchaseUrl")]
        public BankstoreServResponse ExecutePurchaseUrl(string transReference, string amount, string currency, string lang = "ES", string description = null, string secure3D = "0", string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ExecutePurchaseUrl(transReference, amount, currency, lang, description, secure3D, scoring);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a execute_purchase_token under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="idUser">idUser unique identifier system registered user.</param>
        /// <param name="tokenUser">tokenUser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ExecutePurchaseTokenUrl")]
        public BankstoreServResponse ExecutePurchaseTokenUrl(string transReference, string amount, string currency, string idUser, string tokenUser, string lang = "ES", string description = null, string secure3D = "0", string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ExecutePurchaseTokenUrl(transReference, amount, currency, idUser, tokenUser, lang, description, secure3D, scoring);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a add_user under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="lang">language transaction literals</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("AddUserUrl")]
        public BankstoreServResponse AddUserUrl(string transReference, string lang = "ES")
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.AddUserUrl(transReference, lang);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a create_subscription under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="startDate">startDate date subscription start yyyy-mm-dd</param>
        /// <param name="endDate">endDate Date End subscription yyyy-mm-dd</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("CreateSubscriptionUrl")]
        public BankstoreServResponse CreateSubscriptionUrl(string transReference, string amount, string currency, string startDate, string endDate, string periodicity, string lang = "ES", string secure3D = "0", string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.CreateSubscriptionUrl(transReference, amount, currency, startDate, endDate, periodicity, lang, secure3D, scoring);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a create_subscription_token under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="startDate">startDate date subscription start yyyy-mm-dd</param>
        /// <param name="endDate">endDate Date End subscription yyyy-mm-dd</param>
        /// <param name="periodicity">periodicity Frequency of subscription. In days.</param>
        /// <param name="idUser">idUser unique identifier system registered user.</param>
        /// <param name="tokenUser">tokenUser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("CreateSubscriptionTokenUrl")]
        public BankstoreServResponse CreateSubscriptionTokenUrl(string transReference, string amount, string currency, string startDate, string endDate, string periodicity, string idUser, string tokenUser, string lang = "ES", string secure3D = "0", string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.CreateSubscriptionTokenUrl(transReference, amount, currency, startDate, endDate, periodicity, idUser, tokenUser, lang, secure3D, scoring);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a create_preauthorization under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("CreatePreauthorizationUrl")]
        public BankstoreServResponse CreatePreauthorizationUrl(string transReference, string amount, string currency, string lang = "ES", string description = null, string secure3D = "0", string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.CreatePreauthorizationUrl(transReference, amount, currency, lang, description, secure3D, scoring);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a preauthorization_confirm under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="idUser">idUser unique identifier system registered user.</param>
        /// <param name="tokenUser">tokenUser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("PreauthorizationConfirmUrl")]
        public BankstoreServResponse PreauthorizationConfirmUrl(string transReference, string amount, string currency, string idUser, string tokenUser, string lang = "ES", string description = null, string secure3D = "0")
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.PreauthorizationConfirmUrl(transReference, amount, currency, idUser, tokenUser, lang, description, secure3D);
            return servResponse;
        }


        /// <summary>
        /// Returns the URL to launch a preauthorization_cancel under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="idUser">idUser unique identifier system registered user.</param>
        /// <param name="tokenUser">tokenUser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("PreauthorizationCancelUrl")]
        public BankstoreServResponse PreauthorizationCancelUrl(string transReference, string amount, string currency, string idUser, string tokenUser, string lang = "ES", string description = null, string secure3D = "0")
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.PreauthorizationCancelUrl(transReference, amount, currency, idUser, tokenUser, lang, description, secure3D);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a execute_preauthorization_token under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="idUser">idUser unique identifier system registered user.</param>
        /// <param name="tokenUser">tokenUser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("ExecutePreauthorizationTokenUrl")]
        public BankstoreServResponse ExecutePreauthorizationTokenUrl(string transReference, string amount, string currency, string idUser, string tokenUser, string lang = "ES", string description = null, string secure3D = "0", string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ExecutePreauthorizationTokenUrl(transReference, amount, currency, idUser, tokenUser, lang, description, secure3D, scoring);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a deferred_preauthorization under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <param name="scoring">Risk scoring value of the transaction. Between 0 and 100</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationUrl")]
        public BankstoreServResponse DeferredPreauthorizationUrl(string transReference, string amount, string currency, string lang = "ES", string description = null, string secure3D = "0", string scoring = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.DeferredPreauthorizationUrl(transReference, amount, currency, lang, description, secure3D, scoring);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a deferred_preauthorization_confirm under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="idUser">idUser unique identifier system registered user.</param>
        /// <param name="tokenUser">tokenUser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationConfirmUrl")]
        public BankstoreServResponse DeferredPreauthorizationConfirmUrl(string transReference, string amount, string currency, string idUser, string tokenUser, string lang = "ES", string description = null, string secure3D = "0")
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.DeferredPreauthorizationConfirmUrl(transReference, amount, currency, idUser, tokenUser, lang, description, secure3D);
            return servResponse;
        }

        /// <summary>
        /// Returns the URL to launch a deferred_preauthorization_cancel under IFRAME / Fullscreen
        /// </summary>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="idUser">idUser unique identifier system registered user.</param>
        /// <param name="tokenUser">tokenUser token code associated to IDUSER.</param>
        /// <param name="lang">language transaction literals</param>
        /// <param name="description">description Operation description</param>
        /// <param name="secure3D">secure3D Force operation 0 = No 1 = Safe and secure by 3DSecure</param>
        /// <returns>transaction response</returns>
        /// 
        [HttpPost]
        [Route("DeferredPreauthorizationCancelUrl")]
        public BankstoreServResponse DeferredPreauthorizationCancelUrl(string transReference, string amount, string currency, string idUser, string tokenUser, string lang = "ES", string description = null, string secure3D = "0")
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.DeferredPreauthorizationCancelUrl(transReference, amount, currency, idUser, tokenUser, lang, description, secure3D);
            return servResponse;
        }

        /// <summary>
        /// Executes a payment for web service with the "payment by reference" for the migration to PayComet
        /// </summary>
        /// <param name="amount">Amount of payment 1 € = 100</param>
        /// <param name="transReference">transReference unique identifier payment</param>
        /// <param name="rToken">Original card reference stored in old system</param>
        /// <param name="currency">currency identifier transaction currency</param>
        /// <param name="productDescription">description Operation description</param>
        /// <returns>transaction response</returns>
        [HttpPost]
        [Route("ExecutePurchaseRToken")]
        public BankstoreServResponse ExecutePurchaseRToken(string amount, string transReference, string rToken, string currency, string productDescription = null)
        {
            Paycomet_Bankstore serviceAPI = new Paycomet_Bankstore(merchantCode, terminal, password, ipAddress);
            var servResponse = serviceAPI.ExecutePurchaseRToken(amount, transReference, rToken, currency, productDescription);
            return servResponse;
        }

    }
}
