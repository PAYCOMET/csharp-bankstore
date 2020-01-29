using System;
using API_Paycomet_cs.Models;

namespace HttpClientSample
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //------------------------------------------------------------------------------
            //
            // Datos del terminal de la cuenta Sandbox: https://dashboard.paycomet.com/cp_control/index.php
            //
            //------------------------------------------------------------------------------
            string MerchantCode = "XXXXXXXX"; // Corresponde al Código de cliente
            string Terminal = "XXXXX"; // Número de terminal
            string Password = "XXXXXXXXXXXXXXXXXXXX"; // Contraseña
            string ipClient = "X.X.X.X"; // Ip del Cliente(final) que realizará las peticiones
            string endpoint = "https://api.paycomet.com/gateway/xml-bankstore?wsdl";
            string endpointUrl = "https://api.paycomet.com/gateway/ifr-bankstore?";

            // Creación del objeto Bankstore para llamar a la API
            Paycomet_Bankstore bs = new Paycomet_Bankstore(MerchantCode, Terminal, Password, endpoint, endpointUrl);

            //------------------------------------------------------------------------------
            //
            // Datos de la tarjeta
            //
            //------------------------------------------------------------------------------
            string pan = "XXXXXXXXXXXXXXXX";// Añade tu numero de tarjeta. Ej: "5445288852200883"
            string expDate = "XXX";// Añade la fecha de caducidad de la tarjeta. Ej: "0521"
            string cvv = "XXX";// Añade el numero de seguridad de la tarjeta. Ej "123"

            // Ejemplo de llamada AddUser: Ejecución de alta de usuario en el sistema
            // BankstoreServResponse add_user = bs.AddUser(pan, expDate, cvv, ipClient);
        }
    }
}   