using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Infrastructure.Resilience
{
    /// <summary>
    /// Configuración de las estrategias de resiliencia para los HttpClient.
    /// El orden importa: la primera estrategia que se agrega es la más externa.
    ///   request -> [Retry] -> [Circuit Breaker] -> API B
    /// </summary>
    public static class HttpResiliencePolicies
    {
        public static void Configure(ResiliencePipelineBuilder<HttpResponseMessage> pipeline, ApiClientConfiguration config)
        {
            // ---------- 1) RETRY ----------
            // Por defecto reintenta ante errores transitorios: 5xx, 408, 429, HttpRequestException y timeouts.
            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = config.RetryCount,
                Delay = TimeSpan.FromSeconds(config.RetryDelayInSeconds),
                BackoffType = config.UseExponentialBackoff ? DelayBackoffType.Exponential : DelayBackoffType.Constant,
                UseJitter = false, // sin variación aleatoria, para que los tiempos sean fáciles de seguir en clase

                OnRetry = args =>
                {
                    Log(ConsoleColor.Yellow,
                        $"[Retry] Reintento #{args.AttemptNumber + 1} en {args.RetryDelay.TotalSeconds}s. Motivo: {GetFailureReason(args.Outcome)}");
                    return ValueTask.CompletedTask;
                }
            });

            // ---------- 2) CIRCUIT BREAKER ----------
            // Si dentro de SamplingDuration hubo al menos MinimumThroughput requests
            // y la proporción de fallas es >= FailureRatio, el circuito se ABRE durante BreakDuration.
            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                FailureRatio = config.FailureRatio,
                MinimumThroughput = config.MinimumThroughput,
                SamplingDuration = TimeSpan.FromSeconds(config.SamplingDurationInSeconds),
                BreakDuration = TimeSpan.FromSeconds(config.BreakDurationInSeconds),

                OnOpened = args =>
                {
                    Log(ConsoleColor.Red,
                        $"[Circuit Breaker] Circuito ABIERTO por {args.BreakDuration.TotalSeconds}s. Motivo: {GetFailureReason(args.Outcome)}");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    Log(ConsoleColor.Cyan,
                        "[Circuit Breaker] Circuito SEMIABIERTO. Probando con una request...");
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    Log(ConsoleColor.Green,
                        "[Circuit Breaker] Circuito CERRADO. Operación normal reanudada.");
                    return ValueTask.CompletedTask;
                }
            });
        }

        private static string GetFailureReason(Outcome<HttpResponseMessage> outcome)
        {
            if (outcome.Exception is not null)
                return outcome.Exception.Message;

            if (outcome.Result is not null)
                return $"HTTP {(int)outcome.Result.StatusCode} ({outcome.Result.StatusCode})";

            return "Error desconocido";
        }

        // Logs "caseros" con color, para seguir fácilmente lo que pasa en la consola.
        private static readonly object _consoleLock = new();

        private static void Log(ConsoleColor color, string message)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss} {message}");
                Console.ResetColor();
            }
        }
    }
}
