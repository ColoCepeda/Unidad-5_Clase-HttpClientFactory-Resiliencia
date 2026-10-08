using Application.Interfaces;
using Application.Models;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public class JokeService // implementá la interfaz IJokeService
    {
        // 1. Inyectá IHttpClientFactory por constructor.
        // 2. Creá el HttpClient con CreateClient("jokes") (el mismo nombre que registraste en Program.cs).

        // 3. Implementá el método de la interfaz para obtener un chiste aleatorio.

        // Después podés seguir agregando métodos para consumir otros endpoints de la API.
    }
}
