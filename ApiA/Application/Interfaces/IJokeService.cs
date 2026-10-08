using Application.Models;

namespace Application.Interfaces
{
    public interface IJokeService
    {
        Task<JokeDTO> GetRandomJokeAsync();
        Task<JokeDTO?> GetJokeByIdAsync(int id);
        Task<List<string>> GetJokeTypesAsync();
        Task<List<JokeDTO>> GetTenRandomJokesAsync();
        Task<List<JokeDTO>> GetRandomJokesAsync(int cantidad);
        Task<List<JokeDTO>> GetJokesByTypeAsync(string type);
    }
}
