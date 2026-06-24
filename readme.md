**SGE segunda entrega**

<!-- Para visualizar mejor la informacion junto con las capturas clickar open preview arriba a la derecha en su editor de código o Ctrl + K V -->

Datos para probar los casos de uso:

**Administrador:**

Correo: admin@unlp.com

Contraseña: admin123

ID: 9CC8895F-0952-4C50-A493-8A432FE70851

**Usuario sin permisos:**

Correo: sinpermisos@unlp.com

Contraseña: sinpermisos123

ID: 1D0FDAE9-0D10-4DF4-9EFE-A3E7F0A932C7

**Usuario con algunos permisos:**

Correo: algunos@unlp.com

Contraseña: algunos123

ID: 72475470-1BD3-4F7D-9755-99594D0E247E

**Usuario sin particularidades:**

Correo: lucas@unlp.com

Contraseña: test

ID: F154D287-ACED-4EC3-81B5-F66622238679

**Uso de la API:**

**Registrarse:**

Completar los campos con nombre, correo y contraseña como se muestra a continuación:

<img src="./Assets/register.png" width="800">

**Iniciar sesión:**

Completar los campos con nombre y contraseña, si las credenciales son válidas se recibe un token

<img src="./Assets/login_exitoso.png" width="800">

**Modificar mis datos:**

En los headers colocar Authorization Bearer -Su token- y en los campos que desea modificar colocar los nuevos datos, dejar en null los que se deseen preservar

<img src="./Assets/modificar_mis_datos.png" width="800">

**Listar usuarios:**

En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se listaran todos los usuarios registrados
<img src="./Assets/listar_usuarios.png" width="800">
**Eliminar Usuario:**

En variables ID colocar el ID del usuario que se desee eliminar. En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se dará de baja al usuario
<img src="./Assets/eliminar_usuario.png" width="800">
**Modificar Permisos:**

En variables ID colocar el ID del usuario al que se le deseen modificar los permisos. En el request colocar los permisos permitidos por la aplicacion en el formato: ["Ejemplo", "OtroEjemplo"]. En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se modificaran los permisos
<img src="./Assets/modificar_permisos.png" width="800">
**Crear un expediente:**

En el request ingresar una caratula y un id asociado al expediente. En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se creará un nuevo expediente
<img src="./Assets/crear_expediente.png" width="800">

**Listar Expedientes:**

En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se listaran todos los expedientes

<img src="./Assets/listar_expedientes.png" width="800">

**Eliminar expediente:**

En variables ID colocar el id de un expediente que se desee eliminar. En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se dara de baja el expediente
<img src="./Assets/eliminar_expediente.png" width="800">
**Modificar caratula expediente:**

En variables ID colocar el id de un expediente que se desee modificar. En el request llenar el campo con la caratula nueva. En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se concretará la modificación
<img src="./Assets/modificar_caratula_expediente.png" width="800">

**Crear trámite:**

Rellenar los campos con el id del expediente al que debe asociarse el tramite, una etiqueta valida, contenido y el id del usuario asociado. En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se creará el trámite
<img src="./Assets/crear_tramite.png" width="800">

**Listar trámites**

En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se listarán todos los tramites
<img src="./Assets/listar_tramites.png" width="800">

**Eliminar trámite**

En variables ID, colocar el id del trámite que desea eliminar. En los headers colocar Authorization Bearer -Su token-, al ser administrador o tener el permiso correspondiente se eliminará el tramite
<img src="./Assets/eliminar_tramite.png" width="800">
