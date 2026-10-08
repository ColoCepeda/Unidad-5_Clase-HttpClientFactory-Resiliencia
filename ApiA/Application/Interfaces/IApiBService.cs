using Application.Models;

namespace Application.Interfaces
{
    public interface IApiBService
    {
        Task<ApiBResponseDTO> CallUnstableEndpointAsync();
    }
}
