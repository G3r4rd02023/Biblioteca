using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Biblioteca.Frontend.Models;

namespace Biblioteca.Frontend.Services
{
    public class ServicioLista : IServicioLista
    {
        private readonly HttpClient _httpClient;

        public ServicioLista(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7055/");
        }

        public async Task<IEnumerable<SelectListItem>> GetListaCategorias()
        {
            var response = await _httpClient.GetAsync("/api/Categorias");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var categorias = JsonConvert.DeserializeObject<IEnumerable<Categoria>>(content);
                var listaCategorias = categorias.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                }).ToList();

                listaCategorias.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione una Categoria"
                });
                return listaCategorias;
            }

            return [];
        }

        public async Task<IEnumerable<SelectListItem>> GetListaLibros()
        {
            var response = await _httpClient.GetAsync("/api/Libros");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var libros = JsonConvert.DeserializeObject<IEnumerable<Libro>>(content);
                var listaLibros = libros.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Titulo
                }).ToList();
                listaLibros.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione un Libro"
                });
                return listaLibros;
            }

            return [];
        }

        public static List<SelectListItem> GetSelectList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(e => new SelectListItem
                       {
                           Value = e.ToString(),
                           Text = e.ToString()
                       })
                       .ToList();
        }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
            var usuarioJson = await userResponse.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            return usuario!;
        }
    }
}