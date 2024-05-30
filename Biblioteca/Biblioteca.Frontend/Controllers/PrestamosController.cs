using Biblioteca.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Biblioteca.Frontend.Models;
using System.Text;
using System.Reflection;

namespace Biblioteca.Frontend.Controllers
{
    public class PrestamosController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly IServicioLista _servicioLista;

        public PrestamosController(IHttpClientFactory httpClientFactory, IServicioLista servicioLista)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7055/");
            _servicioLista = servicioLista;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Prestamos");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var prestamos = JsonConvert.DeserializeObject<IEnumerable<Prestamo>>(content);
                return View("Index", prestamos);
            }

            return View(new List<Prestamo>());
        }

        public async Task<IActionResult> Create()
        {
            Prestamo prestamo = new()
            {
                //Clientes = await _servicioLista.GetListaClientes(),
                Libros = await _servicioLista.GetListaLibros(),
                EstadoPrestamo = "Pendiente"
            };
            return View(prestamo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                var email = Uri.EscapeDataString(User.Identity.Name);
                var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
                var usuarioJson = await userResponse.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
                prestamo.Usuario = usuario;
                prestamo.UsuarioId = usuario.Id;
                prestamo.EstadoPrestamo = "Pendiente";
                prestamo.FechaPrestamo = DateTime.Now;                                
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
                    ModelState.AddModelError(string.Empty, "Error al crear el prestamo.");
                }
            }
            prestamo.Libros = await _servicioLista.GetListaLibros();
            //prestamo.Clientes = await _servicioLista.GetListaClientes();
            return View(prestamo);
        }

        public async Task<IActionResult> Return(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Prestamos/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Error al obtener el prestamo";
                return RedirectToAction("Index");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var prestamo = JsonConvert.DeserializeObject<Prestamo>(jsonString);
            prestamo.Libros = await _servicioLista.GetListaLibros();
            //prestamo.Clientes = await _servicioLista.GetListaClientes();

            return View(prestamo);
        }

        [HttpPost]
        public async Task<IActionResult> Return(int id, Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
               if(prestamo.EstadoPrestamo == "Pendiente")
               {
                    prestamo.FechaDevolucion = DateTime.Now;
                    prestamo.EstadoPrestamo = "Disponible";
                    var json = JsonConvert.SerializeObject(prestamo);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PutAsync($"/api/Prestamos/{id}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["AlertMessage"] = "Libro devuelto exitosamente!!!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error al intentar realizar la devolucion.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["WarningMessage"] = "Este libro ya fue devuelto y se encuentra disponible";
                    return RedirectToAction("Index");
                }
            }

            prestamo.Libros = await _servicioLista.GetListaLibros();
            //prestamo.Clientes = await _servicioLista.GetListaClientes();
            return View(prestamo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Prestamos/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["AlertMessage"] = "Prestamo eliminado exitosamente!!!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el prestamo.";
                return RedirectToAction("Index");
            }
        }
    }
}
