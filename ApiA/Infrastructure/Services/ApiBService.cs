using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Polly.CircuitBreaker;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public class ApiBService : IApiBService
    {
        private readonly HttpClient _httpClient;

        public ApiBService(IHttpClientFactory httpClientFactory)
        {
            // Este cliente ya viene con Retry + Circuit Breaker configurados en Program.cs.
            // El servicio no se entera: simplemente hace la request.
            _httpClient = httpClientFactory.CreateClient("apiB");
        }

        public async Task<ApiBResponseDTO> CallUnstableEndpointAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/test/unstable");

                // Si llegamos acá con error, es porque se agotaron los reintentos.
                if (!response.IsSuccessStatusCode)
                {
                    throw new ExternalServiceException(
                        $"La API B respondió {(int)response.StatusCode} incluso después de los reintentos.", 502);
                }

                return await response.Content.ReadFromJsonAsync<ApiBResponseDTO>()
                       ?? throw new ExternalServiceException("La API B devolvió una respuesta vacía.", 502);
            }
            catch (BrokenCircuitException ex)
            {
                // Circuito abierto: ni siquiera se intentó llamar a la API B ("fail fast").
                throw new ExternalServiceException(
                    "La API B no está disponible (circuito abierto). Probá de nuevo en unos segundos.", 503, ex);
            }
            catch (HttpRequestException ex)
            {
                // Error de red: por ejemplo, la API B está apagada.
                throw new ExternalServiceException($"No se pudo conectar con la API B: {ex.Message}", 502, ex);
            }
        }
    }
}
