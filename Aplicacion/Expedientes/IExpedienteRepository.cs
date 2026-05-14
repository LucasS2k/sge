namespace Aplicacion.Expedientes;
public interface IExpedienteRepository
{
    void AgregarExpediente(Expediente expediente);
    void ModificarExpediente(Expediente expediente);
    void EliminarExpediente(Guid id);
    void ModificarCaratula(string nuevaCaratula);
    Expediente? ObtenerExpedientePorId(Guid id);
    IEnumerable<Expediente> ObtenerTodosLosExpedientes();
}