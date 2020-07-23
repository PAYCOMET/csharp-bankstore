using System;
using API_Paycomet_cs.Models;

namespace HttpClientSample
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // ################################################################################################
            // Datos del terminal de la cuenta Sandbox: https://dashboard.paycomet.com/cp_control/index.php
            // ################################################################################################
            string MerchantCode = "XXXXXXXX"; // Corresponde al Código de cliente
            string Terminal = "XXXXX"; // Número de terminal
            string Password = "XXXXXXXXXXXXXXXXXXXX"; // Contraseña
            string ipClient = "X.X.X.X"; // Ip del Cliente(final) que realizará las peticiones
            string endpoint = "https://api.paycomet.com/gateway/xml-bankstore?wsdl";
            string endpointUrl = "https://api.paycomet.com/gateway/ifr-bankstore?";

            // Creación del objeto Bankstore para llamar a la API
            Paycomet_Bankstore bs = new Paycomet_Bankstore(MerchantCode, Terminal, Password, endpoint, endpointUrl);

            // ################################################################################################
            // Datos de la tarjeta del cliente que realiza la compra
            // ################################################################################################
            string pan = "XXXXXXXXXXXXXXXX";// Añade tu numero de tarjeta. Ej: "5445288852200883"
            string expDate = "XXX";// Añade la fecha de caducidad de la tarjeta. Ej: "0521"
            string cvv = "XXX";// Añade el numero de seguridad de la tarjeta. Ej "123"


            //-------------------------------------------------------------------------------------------------
            // ################################################################################################
            // Llamadas a los métodos de la API
            // ################################################################################################
            //-------------------------------------------------------------------------------------------------

            //---------------------------------------------------------------------------------------------
            // Ejecución de Alta de Usuario en el sistema [ Necesita Preautorización previa de PAYCOMET ]
            //---------------------------------------------------------------------------------------------
            // BankstoreServResponse add_user_url = bs.AddUserUrl( transReference, "ES" );

            //------------------------------------------------------------------------------
            // Ejecución de cobro a un usuario en el sistema
            //------------------------------------------------------------------------------
            // BankstoreServResponse execute_purchase_url = bs.ExecutePurchaseUrl( transReference, amount, currency, lang, description, secure3D, scoring );

            //------------------------------------------------------------------------------
            // Ejecución de cobro a un usuario en el sistema
            //------------------------------------------------------------------------------
            // BankstoreServResponse create_subscription_url = bs.CreateSubscriptionUrl( transReference, amount, currency, startDate, endDate, periodicity, lang, secure3D, scoring );

            //----------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un execute_purchase_token bajo IFRAME / Fullscreen
            //----------------------------------------------------------------------------------
            // BankstoreServResponse execute_purchase_token_url = bs.ExecutePurchaseTokenUrl( transReference, amount, currency, idUser, tokenUser, lang, description, secure3D, scoring );

            //------------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un create_subscrition_token bajo IFRAME / Fullscreen
            //------------------------------------------------------------------------------------
            // BankstoreServResponse create_subscrition_token_url = bs.CreateSubscriptionTokenUrl( transReference, amount, currency, startDate, endDate, periodicity, idUser, tokenUser, lang, secure3D, scoring );

            //------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un create_preauthorization bajo IFRAME / Fullscreen
            //------------------------------------------------------------------------------
            // BankstoreServResponse create_preauthorization_url = bs.CreatePreauthorizationUrl( transReference, amount, currency, lang, description, secure3D, scoring );

            //---------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un preauthorization_confirm bajo IFRAME / Fullscreen
            //---------------------------------------------------------------------------------
            // BankstoreServResponse preauthorization_confirm_url = bs.PreauthorizationConfirmUrl( transReference, amount, currency, idUser, tokenUser, lang, description, secure3D );

            //---------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un preauthorization_cancel bajo IFRAME / Fullscreen
            //---------------------------------------------------------------------------------
            // BankstoreServResponse preauthorization_cancel_url = bs.PreauthorizationCancelUrl( transReference, amount, currency, idUser, tokenUser, lang, description, secure3D );

            //-------------------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un execute_preauthorization_token bajo IFRAME / Fullscreen
            //-------------------------------------------------------------------------------------------
            // BankstoreServResponse execute_preauthorization_token = bs.CreatePreauthorizationTokenUrl( transReference, amount, currency, idUser, tokenUser, lang, description, secure3D, scoring );

            //-----------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un deferred_preauthorization bajo IFRAME / Fullscreen
            //-----------------------------------------------------------------------------------
            // BankstoreServResponse deferred_preauthorization = bs.DeferredPreauthorizationUrl( transReference, amount, currency, lang, description, secure3D, scoring );

            //--------------------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un deferred_preauthorization_confirm bajo IFRAME / Fullscreen
            //--------------------------------------------------------------------------------------------
            // BankstoreServResponse deferred_preauthorization_confirm = bs.DeferredPreauthorizationConfirmUrl( transReference, amount, currency, idUser, tokenUser, lang, description, secure3D );

            //-------------------------------------------------------------------------------------------
            // Devuelve la URL para lanzar un deferred_preauthorization_cancel bajo IFRAME / Fullscreen
            //-------------------------------------------------------------------------------------------
            // BankstoreServResponse deferred_preauthorization_cancel = bs.DeferredPreauthorizationCancelUrl( transReference, amount, currency, idUser, tokenUser, lang, description, secure3D );

        }
    }
}