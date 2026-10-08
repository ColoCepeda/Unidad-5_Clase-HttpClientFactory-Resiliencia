using Application.Interfaces;
using Application.Models;
using System.Net;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public class JokeService : IJokeService
    {
        private readonly HttpClient _httpClient;

        // Inyectamos la FÁBRICA, no un HttpClient creado a mano.
        // La fábrica reutiliza los handlers por debajo: evitamos agotar sockets y el DNS desactualizado.
        public JokeService(IHttpClientFactory httpClientFactory)
        {
            // "jokes" es el nombre con el que registramos el cliente en Program.cs (ahí está la BaseAddress).
            _httpClient = httpClientFactory.CreateClient("jokes");
        }

        // Versión "paso a paso", como en el apunte: request -> verificar status -> leer -> deserializar.
        public async Task<JokeDTO> GetRandomJokeAsync()
        {
            var response = await _httpClient.GetAsync("random_joke");

            response.EnsureSuccessStatusCode(); // lanza HttpRequestException si no es 2xx

            var joke = await response.Content.ReadFromJsonAsync<JokeDTO>();
            return joke ?? throw new InvalidOperationException("La API de chistes devolvió una respuesta vacía.");
        }

        // Si el id no existe, la API responde 404: lo transformamos en null para que el controller devuelva NotFound.
        public async Task<JokeDTO?> GetJokeByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"jokes/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JokeDTO>();
        }

        // Versión corta: GetFromJsonAsync hace request + verificación + deserialización en una línea.
        public async Task<List<string>> GetJokeTypesAsync()
            => await _httpClient.GetFromJsonAsync<List<string>>("types") ?? [];

        public async Task<List<JokeDTO>> GetTenRandomJokesAsync()
            => await _httpClient.GetFromJsonAsync<List<JokeDTO>>("jokes/ten") ?? [];

        public async Task<List<JokeDTO>> GetRandomJokesAsync(int cantidad)
            => await _httpClient.GetFromJsonAsync<List<JokeDTO>>($"jokes/random/{cantidad}") ?? [];

        public async Task<List<JokeDTO>> GetJokesByTypeAsync(string type)
            => await _httpClient.GetFromJsonAsync<List<JokeDTO>>($"jokes/{Uri.EscapeDataString(type)}/ten") ?? [];
    }
}
