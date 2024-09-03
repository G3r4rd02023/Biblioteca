using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Biblioteca.Frontend.Models;
using Biblioteca.Frontend.Services;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Frontend.Controllers
{
    [Authorize]
    public class LibrosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IServicioLista _servicioLista;

        public LibrosController(IHttpClientFactory httpClientFactory, IServicioLista servicioLista)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7055/");
            _servicioLista = servicioLista;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _servicioLista.GetUsuarioByEmail(User.Identity!.Name!);
            var apiService = new ServicioToken();
            var token = await apiService.Autenticar(user);
            // Realiza la solicitud HTTP al endpoint protegido
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("/api/Libros");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var libros = JsonConvert.DeserializeObject<IEnumerable<Libro>>(content);
                return View("Index", libros);
            }

            return View(new List<Libro>());
        }

        public async Task<IActionResult> Create()
        {
            Libro libro = new()
            {
                Categorias = await _servicioLista.GetListaCategorias(),
                Estado = "Disponible"
            };
            return View(libro);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Libro libro)
        {
            //libro.Estado = "Disponible";
            var respuesta = await _httpClient.GetAsync("/api/Libros");
            var contenido = await respuesta.Content.ReadAsStringAsync();
            var libros = JsonConvert.DeserializeObject<IEnumerable<Libro>>(contenido);
            var lastCodigo = libros.OrderByDescending(x => x.Id).Select(c => c.Codigo).FirstOrDefault();
            int lastNumber = 0;
            if (!string.IsNullOrEmpty(lastCodigo) && lastCodigo.Length > 2)
            {
                int.TryParse(lastCodigo.Substring(2), out lastNumber);
            }
            libro.Codigo = $"CL{(lastNumber + 1).ToString("D5")}";

            var json = JsonConvert.SerializeObject(libro);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Libros/", content);
            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Libro creado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error al crear el libro.");
            }
            libro.Categorias = await _servicioLista.GetListaCategorias();
            return View(libro);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Libros/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener el libro.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var libro = JsonConvert.DeserializeObject<Libro>(jsonString);
            libro.Categorias = await _servicioLista.GetListaCategorias();

            return View(libro);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(libro);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Libros/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Libro actualizado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar el cliente.";
                    return RedirectToAction("Index");
                }
            }
            libro.Categorias = await _servicioLista.GetListaCategorias();
            return View(libro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Libros/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Libro actualizado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el libro.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Solicitud(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Libros/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener el libro.";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var libro = JsonConvert.DeserializeObject<Libro>(jsonString);

            var email = Uri.EscapeDataString(User.Identity.Name);
            var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
            var usuarioJson = await userResponse.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);

            Prestamo prestamo = new()
            {
                UsuarioId = usuario.Id,
                Usuario = usuario,
                Libro = libro,
                LibroId = libro.Id,
                EstadoPrestamo = "Reservado",
                FechaPrestamo = DateTime.Now
            };
            return View(prestamo);
        }

        [HttpPost]
        public async Task<IActionResult> Solicitud(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(prestamo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Prestamos/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["AlertMessage"] = "Prestamo creado exitosamente!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Error al crear el prestamo: {errorMessage}";
                    return RedirectToAction("Index");
                }
            }

            return View(prestamo);
        }
    }
}