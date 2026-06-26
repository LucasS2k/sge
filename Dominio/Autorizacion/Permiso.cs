namespace Dominio.Autorizacion;
public enum Permiso
{
    //tramite
    TramiteAlta,
    TramiteBaja,
    TramiteModificacion,
    TramiteListar,
    //expediente
    ExpedienteAlta, //necesario
    ExpedienteBaja, //necesario
    ExpedienteModificacion, //necesario
    ExpedienteListar, //necesario
     //usuario
     UsuarioAlta, //innecesario
     UsuarioBaja, //admin deberia poder hacerlo sin el permiso especifico
     UsuarioModificacion, //admin deberia poder hacerlo sin el permiso especifico
     UsuarioListar //admin deberia poder hacerlo sin el permiso especifico
 }