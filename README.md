### API de PAYCOMET BankStore en ASP NET y C#

Este es el API de conexión con todos los servicios de PAYCOMET BankStore mediante XML, IFRAME, FULLSCREEN y JET.

### Instalación

Descarga el proyecto con la solución **API_Paycomet_cs**

Dentro de la misma encontrarás:

 - **API_Paycomet_cs**:  API con los servicios Paycomet Bankstore
 - **1 - Desktop_Client_cs**: La aplicacion cliente de escritorio, que contiene ejemplos de uso con llamadas a API (API_Paycomet_cs)
 - **2-Web_Client_cs**: La aplicacion web, que contiene dos ejemplos de compras: usuarios corrientes (se puede aplicar para ambos casos) o usuarios PCI DSS.

Para realizar pruebas con cualquira de las aplicaciones, **Desktop_Client_cs** o **Web_Client_cs** , no te olvides de establecer ese proyecto como *"Proyecto de inicio"** haciendo click derecho sobre el mismo y seleccionado la opción antes de ejecutar la solución.

### Aplicación "API_Paycomet_cs"
No es necesario alterar su funcionamiento. El listado de **métodos**(*) disponibles se encuentra al final del documento.

## 1 - Aplicación "Desktop_Client_cs"
Esto es solo un ejemplo de integración con una aplicación de consola.

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
```sh
pan => Corresponde al número de la tarjeta
expDate = Correponde a la fecha de caducidad de la tarjeta
cvv => Correponde al numero de seguridad de la tarjeta
```

De esta forma, ya podremos hacer llamadas a los métodos de la API, como por ejemplo:

```sh
BankstoreServResponse add_user = bs.AddUser(pan, expDate, cvv, ipClient);
```

## 2 - Aplicación "Web_Client_cs"
Es posible que tengas que actualizar los paquetes NuGet del proyecto si al ejecutar el mismo recibes algún tipo de error, en caso contrario omite este punto.

Para poder realizar uso, debes modificar las variables de configuración dentro del archivo **Web.config**,  con los datos de tu termial obtenidos en `https://dashboard.paycomet.com/cp_control/index.php` en el menú **Mis Productos -> Configurar productos -> Editar**
```sh
MerchantCode => Corresponde al Código de cliente
Terminal => Correpsonde al Número de terminal
Password => Corresponde a la Contraseña
JetId => Se genera dando al botón que aparece al lado del mismo, dentro del panel de administración de tu roducto, y se obtiene una cadena alfanumérica
endpoint => "https://api.paycomet.com/gateway/xml-bankstore?wsdl"
endpointUrl => "https://api.paycomet.com/gateway/ifr-bankstore?"
```

El proyecto dispone de un Controlador: **HomeController** y tres vistas: **Home, Form1, Form2**. Al iniciar el proyecto podemos probar dos integraciones, siendo **recomendada la primera**, JET-IFRAME o Formulario PCI DSS.

**La integración de JET-IFRAME la podrá realizar cualquier usuario**, pero la integración de Formulario PCI DSS solo aquellos que dispongan de autorización para el tratamiento de datos de tarjeta en sus servidores. 
Tenga en cuenta que con la integración de JET-IFRAME todos los datos se procesan en los **servidores seguros** de PAYCOMET y no tiene que preocuparse de realizar más que las llamadas.


## (*) Integración de métodos API_Paycomet_cs

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
Si tienes alguna duda o pregunta puedes escribirnos un email a [tecnico@paycomet.com]

License
----

Paycomet
