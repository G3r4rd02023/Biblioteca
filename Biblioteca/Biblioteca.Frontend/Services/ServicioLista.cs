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

        public async Task<IEnumerable<SelectListItem>> GetListaClientes()
        {
            var response = await _httpClient.GetAsync("/api/Clientes");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<IEnumerable<Cliente>>(content);
                var listaClientes = clientes.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.NombreCompleto
                }).ToList();
                listaClientes.Insert(0, new SelectListItem
                {
                    Value = "",
                    Text = "Seleccione un Cliente"
                });
                return listaClientes;
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
    }
}
