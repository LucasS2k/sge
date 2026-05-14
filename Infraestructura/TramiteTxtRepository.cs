namespace Infraestructura;

using System.IO;
using Aplicacion.Tramites;
using Dominio.Tramites;
using Dominio.Comun;

public class TramiteTxtRepository : ITramiteRepository
{
    private const string Archivo = "tramites.txt";
    public void AgregarTramite(Tramite tramite)
    {
        using var sw = new StreamWriter(Archivo, true);
        sw.WriteLine(Convertir(tramite));
    }

    public IEnumerable<Tramite> ObtenerTodosLosTramites()
    {
        var lista = new List<Tramite>();
        if (!File.Exists(Archivo)) return lista;
        using var sr = new StreamReader(Archivo);
        while (!sr.EndOfStream)
        {
            var linea = sr.ReadLine();
            if (!string.IsNullOrWhiteSpace(linea))
            {
                lista.Add(ReconstruirTramite(linea));
            }
        }
        return lista;
    }

    public IEnumerable<Tramite> ObtenerTramitesPorExpedienteId(Guid expedienteId)
{
    var lista = new List<Tramite>();
    if (!File.Exists(Archivo)) return lista;

    using var sr = new StreamReader(Archivo);
    while (!sr.EndOfStream)
    {
        var linea = sr.ReadLine();
        if (string.IsNullOrWhiteSpace(linea)) continue;
        var datos = linea.Split(',');
        var idExpEnLinea = Guid.Parse(datos[1]); 
        if (idExpEnLinea == expedienteId)
        {
            lista.Add(ReconstruirTramite(linea));
        }
    }
    return lista;
}

    public Tramite? ObtenerTramitePorId(Guid id)
    {
        return ObtenerTodosLosTramites().FirstOrDefault(t => t.Id == id); //si no lo encuentra da un null que es mas util
    }

    public void ModificarTramite(Tramite tramite)
    {
        string auxiliar = "tramites_aux.txt";
        bool encontrado = false;

        using (var sr = new StreamReader(Archivo))
        using (var sw = new StreamWriter(auxiliar))
        {
            while (!sr.EndOfStream)
            {
                var linea = sr.ReadLine();
                if (string.IsNullOrWhiteSpace(linea)) continue;

                var idEnLinea = Guid.Parse(linea.Split(',')[0]);

                if (idEnLinea == tramite.Id)
                {
                    sw.WriteLine(Convertir(tramite));
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
             throw new NotFoundException("Tramite no encontrado");
        }
    }
    public void EliminarTramite(Guid id)
    {
        string auxiliar = "tramites_aux.txt";
        bool encontrado = false;

        using var sr = new StreamReader(Archivo);
        using var sw = new StreamWriter(auxiliar);
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
        } else
        {
            File.Delete(auxiliar);
             throw new NotFoundException("Tramite no encontrado");
        }
    }

    private static string Convertir(Tramite t)
    {
        return $"{t.Id},{t.ExpedienteId},{t.Etiqueta},{t.Contenido},{t.FechaCreacion},{t.UsuarioUltimoCambio}";
    }

    private Tramite ReconstruirTramite(string linea)
    {
        var datos = linea.Split(',');
        return new Tramite(
            Guid.Parse(datos[0]),
            Guid.Parse(datos[1]),
            Enum.Parse<EstadoTramite>(datos[2]),
            ContenidoTramite.Parse(datos[3]), 
            DateTime.Parse(datos[4]),
            Guid.Parse(datos[5])
        );
    }
}