using Biblioteca.Frontend.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Frontend.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaCategorias();

        Task<IEnumerable<SelectListItem>> GetListaLibros();

        Task<Usuario> GetUsuarioByEmail(string email);
    }
}