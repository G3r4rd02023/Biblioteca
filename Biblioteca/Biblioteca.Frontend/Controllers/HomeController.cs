using Biblioteca.Frontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Biblioteca.Frontend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7055/");
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Prestamos");
            var responseLibros = await _httpClient.GetAsync("/api/Libros");
            if (response.IsSuccessStatusCode && responseLibros.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var prestamos = JsonConvert.DeserializeObject<IEnumerable<Prestamo>>(content);
                var devoluciones = prestamos.Where(p => p.EstadoPrestamo != "Pendiente");
                var pendientes = prestamos.Where(p => p.EstadoPrestamo == "Pendiente");
                ViewBag.Devoluciones = devoluciones.Count();
                ViewBag.CantidadPrestamos = prestamos.Count();
                ViewBag.Pendientes = pendientes.Count();

                var contentLibros = await responseLibros.Content.ReadAsStringAsync();
                var libros = JsonConvert.DeserializeObject<IEnumerable<Libro>>(contentLibros);
                ViewBag.Libros = libros.Count();                             
                return View("Index", new { Prestamos = prestamos, Libros = libros });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
