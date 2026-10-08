using Microsoft.AspNetCore.Mvc;

namespace ApiB.Controllers
{
    /// <summary>
    /// API "inestable" para practicar resiliencia: falla a propósito las primeras N requests.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private static int _requestCount = 0;
        private static int _requestUmbral = 5; // cantidad de requests que van a fallar

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("unstable")]
        public IActionResult GetUnstableResponse()
        {
            var actual = Interlocked.Increment(ref _requestCount);

            _logger.LogWarning("Recibimos request el: {Fecha:HH:mm:ss} - contador: {Contador}", DateTime.Now, actual);

            if (actual <= _requestUmbral) // simula fallas en las primeras N requests
            {
                return StatusCode(500, "Internal Server Error simulated");
            }

            return Ok(new { message = "Success after failures", attempts = actual });
        }

        [HttpGet("reset")]
        public IActionResult Reset()
        {
            _requestCount = 0; // reseteamos el contador de requests entrantes
            _logger.LogInformation("Se reseteó el contador el: {Fecha:HH:mm:ss}", DateTime.Now);
            return Ok("Counter reset");
        }

        /// <summary>Cambia cuántas requests fallan (y resetea el contador) sin reiniciar la API.</summary>
        [HttpPost("umbral/{cantidad:int}")]
        public IActionResult SetUmbral(int cantidad)
        {
            _requestUmbral = Math.Max(0, cantidad);
            _requestCount = 0;
            _logger.LogInformation("Nuevo umbral de fallas: {Umbral} (contador reseteado)", _requestUmbral);
            return Ok(new { umbral = _requestUmbral });
        }
    }
}
