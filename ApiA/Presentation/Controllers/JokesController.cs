using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JokesController : ControllerBase
    {
        // Inyectá el servicio...

        [HttpGet("random")]
        public async Task<ActionResult<JokeDTO>> GetRandomJokeAsync()
        {
            // Completá el endpoint...
            throw new NotImplementedException();
        }

        // Creá algunos endpoints más para consumir otros endpoints de la API de chistes.
        /* Por ejemplo:
            - Chiste por id                 -> /jokes/{id}
            - Tipos de chistes              -> /types
            - Diez chistes aleatorios       -> /jokes/ten
            - N chistes aleatorios          -> /jokes/random/{n}
            - Chistes por tipo              -> /jokes/{type}/ten
        */
    }
}
