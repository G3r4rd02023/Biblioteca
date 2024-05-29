using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Frontend.Services
{
    public interface IServicioLista
    {
        Task<IEnumerable<SelectListItem>> GetListaCategorias();

        Task<IEnumerable<SelectListItem>> GetListaClientes();

        Task<IEnumerable<SelectListItem>> GetListaLibros();
       
    }
}
