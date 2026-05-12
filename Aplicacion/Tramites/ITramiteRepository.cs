namespace Aplicacion.Tramites;
public interface ITramiteRepository
{
    void AgregarTramite(Tramite tramite);
    void ModificarTramite(Tramite tramite);
    void EliminarTramite(Guid id);
    Tramite? ObtenerTramitePorId(Guid id);
    IEnumerable<Tramite> ObtenerTodosLosTramites();
}