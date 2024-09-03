using Biblioteca.Frontend.Models;
using Newtonsoft.Json;
using System.Text;

namespace Biblioteca.Frontend.Services
{
    public class ServicioToken
    {
        public async Task<string> Autenticar(Usuario usuario)
        {
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:7055/");

            var credenciales = new Usuario()
            {
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                Clave = usuario.Clave,
                FechaCreacion = usuario.FechaCreacion,
                RolUsuario = usuario.RolUsuario,
                Estado = usuario.Estado,
            };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
            var response = await cliente.PostAsync("api/Autenticacion/Validar", content);
            var json = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<Credencial>(json);
            var token = resultado!.Token;
            return token;
        }
    }
}