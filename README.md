# Biblioteca
Sistema para biblioteca

Instrucciones para implementar JWT

Backend 

Instalar Paquete
Microsoft.AspNetCore.Authentication.JwtBearer

Program.cs

 builder.Services.AddAuthentication(config =>
 {
     config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 }).AddJwtBearer(config =>
 {
     config.RequireHttpsMetadata = false;
     config.SaveToken = true;
     config.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         ValidateIssuer = false,
         ValidateAudience = false,
         ValidateLifetime = true,
         ClockSkew = TimeSpan.Zero,
         IssuerSigningKey = new SymmetricSecurityKey
         (Encoding.UTF8.GetBytes(key!))
     };
 });

app.UseAuthentication();
''''''''''''''''''''''''''''''
AutenticacionController


using Biblioteca.Backend.Data;
using Biblioteca.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biblioteca.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly string secretKey;
        private readonly DataContext _context;

        public AutenticacionController(IConfiguration config, DataContext context)
        {
            secretKey = config.GetSection("Jwt").GetValue<string>("key")!;
            _context = context;
        }

        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Usuario request)
        {
            var usuario = _context.Usuarios.SingleOrDefault(u => u.Correo == request.Correo);

            if (usuario != null && usuario.Clave == request.Clave)
            {
                var keyBytes = Encoding.ASCII.GetBytes(secretKey);
                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, request.Correo));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokencreado = tokenHandler.WriteToken(tokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = tokencreado });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { token = "" });
            }
        }
    }
}
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

FrontEnd

 public class Credencial
 {
     public string Token { get; set; } = null!;
 }


'''''''''''''''''''''''''''''''''''''''''''''''''
Interface IServicioLista

Task<Usuario> GetUsuarioByEmail(string email);

Clase ServicioLista

 public async Task<Usuario> GetUsuarioByEmail(string email)
 {
     var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
     var usuarioJson = await userResponse.Content.ReadAsStringAsync();
     var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
     return usuario!;
 }
'''''''''''''''''''''''''''''''''''''''''''''''''

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

'''''''''''''''''''''''''''''''''''''''''''''
ServicioToken

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
