using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Biblioteca.Frontend.Models;
using System.Text;


namespace Biblioteca.Frontend.Controllers
{
    public class ClientesController : Controller
    {

        private readonly HttpClient _httpClient;

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7055/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Clientes");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<IEnumerable<Cliente>>(content);
                return View("Index", clientes);
            }

            return View(new List<Cliente>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _httpClient.GetAsync("/api/Clientes");
                var contenido = await respuesta.Content.ReadAsStringAsync();
                var clientes = JsonConvert.DeserializeObject<IEnumerable<Cliente>>(contenido);
                var lastCodigo = clientes.OrderByDescending(x => x.Id).Select(c => c.Codigo).FirstOrDefault();
                int lastNumber = 0;
                if (!string.IsNullOrEmpty(lastCodigo) && lastCodigo.Length > 2)
                {
                    int.TryParse(lastCodigo.Substring(2), out lastNumber);
                }
                cliente.Codigo = $"CL{(lastNumber + 1).ToString("D5")}";

                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Clientes/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Cliente creado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear el cliente.");
                    TempData["ErrorMessage"] = "Ocurrió un error al intentar crear el cliente!!!";
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ModelErrors"] = string.Join("\n", errors);
            }
            return View(cliente);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Clientes/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener el cliente.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var cliente = JsonConvert.DeserializeObject<Cliente>(jsonString);

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(cliente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Clientes/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Cliente actualizado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar el cliente.";
                    return RedirectToAction("Index");
                }
            }
            return View(cliente);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Clientes/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Cliente eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el cliente";
                return RedirectToAction("Index");
            }
        }
    }
}
