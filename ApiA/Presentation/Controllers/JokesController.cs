using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JokesController : ControllerBase
    {
        private readonly IJokeService _jokeService;

        public JokesController(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        [HttpGet("random")]
        public async Task<ActionResult<JokeDTO>> GetRandomJokeAsync()
        {
            var joke = await _jokeService.GetRandomJokeAsync();
            return Ok(joke);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<JokeDTO>> GetJokeByIdAsync(int id)
        {
            var joke = await _jokeService.GetJokeByIdAsync(id);
            return joke is null ? NotFound($"No existe un chiste con id {id}") : Ok(joke);
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<string>>> GetJokeTypesAsync()
            => Ok(await _jokeService.GetJokeTypesAsync());

        [HttpGet("ten")]
        public async Task<ActionResult<List<JokeDTO>>> GetTenRandomJokesAsync()
            => Ok(await _jokeService.GetTenRandomJokesAsync());

        [HttpGet("random/{cantidad:int}")]
        public async Task<ActionResult<List<JokeDTO>>> GetRandomJokesAsync(int cantidad)
        {
            if (cantidad < 1)
                return BadRequest("La cantidad debe ser mayor a 0.");

            return Ok(await _jokeService.GetRandomJokesAsync(cantidad));
        }

        [HttpGet("type/{type}")]
        public async Task<ActionResult<List<JokeDTO>>> GetJokesByTypeAsync(string type)
            => Ok(await _jokeService.GetJokesByTypeAsync(type));
    }
}
