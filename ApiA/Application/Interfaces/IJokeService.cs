using Application.Models;

namespace Application.Interfaces
{
    public interface IJokeService
    {
        Task<JokeDTO> GetRandomJokeAsync();

        // Una vez que consigas obtener un chiste aleatorio, agregá otras firmas de métodos
        // para consumir más endpoints de la API de chistes.
    }
}
