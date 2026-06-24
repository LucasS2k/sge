namespace Aplicacion.Usuarios;
using System.Security.Cryptography;
using System.Text;

public static class Hash
{
    public static string Calcular(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static bool Verificar(string textoPlano, string hashAlmacenado)
        => Calcular(textoPlano) == hashAlmacenado;
}

public interface ITokenProvider
{
    string GenerarToken(Dominio.Usuarios.Usuario usuario);
}