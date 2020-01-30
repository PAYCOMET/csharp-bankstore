API de PAYCOMET BankStore en ASP.NET y C#
=====================

Este es el API de conexión con todos los servicios de PAYCOMET BankStore mediante XML, IFRAME, FULLSCREEN y JET.

### Instalación

Descarga el proyecto con la solución **API_Paycomet_cs**

Dentro de la misma encontrarás:

 - **API_Paycomet_cs**: La API con los servicios Paycomet
 - **Desktop_Client_cs**: La aplicacion cliente de escritorio, que contiene ejemplos de uso con llamadas a la API 

### Aplicación "API_Paycomet_cs"
No es necesario alterar su funcionamiento.

### Aplicación "Desktop_Client_cs"
Podrá utilizar cualquier tipo de aplicación con lenguaje C#, esto es solo un ejemplo de integración con una aplicación de consola.
Para realizar pruebas, establece este proyecto como "Proyecto de inicio" haciendo click derecho sobre el mismo y seleccionado la opción antes de ejecutar la solución.

Dentro de esta aplicación se encuentra el archivo **Program.cs**, tendrás que configurar las variables con los datos de tu termial obtenidos en `https://dashboard.paycomet.com/cp_control/index.php` en el menú **Mis Productos -> Configurar productos -> Editar**
```sh
MerchantCode => Corresponde al Código de cliente
Terminal => Correpsonde al Número de terminal
Password => Corresponde a la Contraseña
ipClient => Es la Ip del Cliente(final) que realizará las peticiones
endpoint => "https://api.paycomet.com/gateway/xml-bankstore?wsdl"
endpointUrl => "https://api.paycomet.com/gateway/ifr-bankstore?"
```

Con la configuración anterior, ya podrás crear un objeto de tipo **Paycomet_Bankstore**

```sh
Paycomet_Bankstore bs = new Paycomet_Bankstore(MerchantCode, Terminal, Password, endpoint, endpointUrl);
```

También tendrás que añadir los datos de tu tarjeta para que pueda ser tokenizada, los datos son los siguientes:

[Tarjetas de prueba: `https://docs.paycomet.com/es/cards/testcards`]
```sh
pan => Corresponde al número de la tarjeta
expDate = Correponde a la fecha de caducidad de la tarjeta
cvv => Correponde al numero de seguridad de la tarjeta
```

De esta forma, ya podremos hacer llamadas a los métodos de la API, como por ejemplo:

```sh
BankstoreServResponse add_user = bs.AddUser(pan, expDate, cvv, ipClient);
```


### Integración de métodos API_Paycomet_cs

| Método | Descripción |
| ------ | ------ |
| AddUser | Ejecución de alta de usuario en el sistema |
| InfoUser | Información del usuario |
| RemoveUser | Eliminación del usuario |
| ExecutePurchase | Ejecución de cobro a Usuario en el sistema |
| ExecutePurchaseDcc | Ejecución de cobro a Usuario en el sistema por DCC |
| ConfirmPurchaseDcc | Confirmación de moneda en pago DCC |
| ExecuteRefund | Devolución de cobro a usuario en el sistema |
| CreateSubscription | Ejecución de alta de suscripción en el sistema |
| EditSubscription | Modificación de suscripción en el sistema|
| RemoveSubscription | Eliminación de Suscripción |
| CreateSubscriptionToken | Ejecución de alta de suscripción en el sistema con USERID Y TOKENID |
| CreatePreauthorization | Creación de una preautorización a usuario en el sistema |
| PreauthorizationConfirm | Confirmación de una preautorización a usuario en el sistema |
| PreauthorizationCancel | Cancelación de una preautorización a usuario en el sistema |
| DeferredPreauthorizationConfirm | Confirmación de una preautorización diferida a usuario en el sistema |
| DeferredPreauthorizationCancel | Cancelación de una preautorización diferida a usuario en el sistema |
| AddUserToken | Ejecución de alta de usuario en el sistema mediante Token |
| ExecutePurchaseRToken | Ejecución de Cobro a un usuario por Referencia |
| AddUserUrl | Devuelve la url para iniciar una ejecuación de Alta de Usuario en el sistema |
| ExecutePurchaseUrl | Devuelve la url para iniciar una ejecución de cobro en el sistema (Alta implícita de Usuario en el sistema) |
| CreateSubscriptionUrl | Devuelve la url para iniciar un de alta de suscripción en el sistema (Alta implícita de Usuario en el sistema)|
| ExecutePurchaseTokenUrl | Devuelve la url para iniciar una ejecución de cobro existente|
| CreateSubscriptionTokenUrl | Devuelve la url para iniciar una ejecución de Alta de Suscripción a un usuario existente |
| CreatePreauthorizationUrl | Devuelve la url para iniciar una ejecución de Alta de Preautorización (Alta Implícita de Usuario en el sistema) |
| PreauthorizationConfirmUrl | Devuelve la url para iniciar una ejecución de Confirmación de Preautorización |
| PreauthorizationCancelUrl | Devuelve la url para iniciar una ejecución de Cancelación de Preautorización |
| ExecutePreauthorizationTokenUrl | Devuelve la url para iniciar un alta de Preautorización a un usuario existente |
| DeferredPreauthorizationUrl | Devuelve la url para iniciar una ejecución de alta de preautorización diferida (alta implícita de usuario en el sistema)|
| DeferredPreauthorizationConfirmUrl | Devuelve la url para iniciar una ejecución de Confirmación de Preautorización Diferida |
| DeferredPreauthorizationCancelUrl | Devuelve la url para iniciar una ejecución de Cancelación de Preautorización Diferida |

### Documentación

Enlace a la documentación: `https://docs.paycomet.com/es/documentacion/introduccion`

### Soporte
Si tienes alguna duda o pregunta no tienes más que escribirnos un email a [tecnico@paycomet.com]

License
----

Paycomet


