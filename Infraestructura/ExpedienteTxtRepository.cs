namespace Infraestructura;
using System.IO;
using Aplicacion.Expedientes;
using Dominio.Comun;
public class ExpedienteTxtRepository : IExpedienteRepository
{
    private const string Archivo = "expedientes.txt";
    public void AgregarExpediente(Expediente expediente)
    {
        using var sw = new StreamWriter(Archivo, true);
        sw.WriteLine(Convertir(expediente));
    }

    public IEnumerable<Expediente> ObtenerTodosLosExpedientes()
    {
        var lista = new List<Expediente>();
        if (!File.Exists(Archivo)) return lista;
        using var sr = new StreamReader(Archivo);
        while (!sr.EndOfStream)
        {
            var linea = sr.ReadLine();
            if (!string.IsNullOrWhiteSpace(linea))
            {
                lista.Add(ReconstruirExpediente(linea));
            }
        }
        return lista;
    }

    public Expediente? ObtenerExpedientePorId(Guid id)
    {
        return ObtenerTodosLosExpedientes().FirstOrDefault(e => e.Id == id);
    }

    public void ModificarExpediente(Expediente expediente)
    {
        string auxiliar = "expedientes_aux.txt";
        bool encontrado = false;

        using (var sr = new StreamReader(Archivo))
        using (var sw = new StreamWriter(auxiliar))
        {
            while (!sr.EndOfStream)
            {
                var linea = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var idEnLinea = Guid.Parse(linea.Split(',')[0]);

                if (idEnLinea == expediente.Id)
                {
                    sw.WriteLine(Convertir(expediente));
                    encontrado = true;
                }
                else
                {
                    sw.WriteLine(linea);
                }
            }
        }

        if (encontrado)
        {
            File.Delete(Archivo);
            File.Move(auxiliar, Archivo);
        }
        else
        {
            File.Delete(auxiliar);
            throw new Exception("Expediente no encontrado");
        }
    }
    public void EliminarExpediente(Guid id)
    {
        string auxiliar = "expedientes_aux.txt";
        bool encontrado = false;

        using (var sr = new StreamReader(Archivo))
        using (var sw = new StreamWriter(auxiliar))
        {
            while (!sr.EndOfStream)
            {
                var linea = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var idEnLinea = Guid.Parse(linea.Split(',')[0]);

                if (idEnLinea == id)
                {
                    encontrado = true;
                }
                else
                {
                    sw.WriteLine(linea);
                }
            }
        }
        if (encontrado)
        {
            File.Delete(Archivo);
            File.Move(auxiliar, Archivo);
        }
        else
        {
            File.Delete(auxiliar);
            throw new NotFoundException("Expediente no encontrado");
        }
    }

    private string Convertir(Expediente expediente)
    {
        return $"{expediente.Id},{expediente.ExpedienteId},{expediente.Caratula.Valor},{expediente.FechaCreacion},{expediente.FechaUltimaModificacion},{expediente.UsuarioUltimoCambio},{expediente.Estado}";
    }
    private  Expediente ReconstruirExpediente(string linea)
    {
        var datos = linea.Split(',');
        return new Expediente
        (Guid.Parse(datos[0]),
            Guid.Parse(datos[1]),
            new Caratula(datos[2]),
            DateTime.Parse(datos[3]),
            DateTime.Parse(datos[4]),
            Guid.Parse(datos[5]),
            Enum.Parse<EstadoExpediente>(datos[6]));
    }
}