using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallController : ControllerBase
    {
        private readonly IApiBService _apiBService;

        public CallController(IApiBService apiBService)
        {
            _apiBService = apiBService;
        }

        /// <summary>Llama al endpoint inestable de la API B (con Retry + Circuit Breaker).</summary>
        [HttpGet("api-b")]
        public async Task<ActionResult<ApiBResponseDTO>> CallApiBAsync()
        {
            try
            {
                return Ok(await _apiBService.CallUnstableEndpointAsync());
            }
            catch (ExternalServiceException ex)
            {
                return StatusCode(ex.StatusCode, new { error = ex.Message });
            }
        }
    }
}
