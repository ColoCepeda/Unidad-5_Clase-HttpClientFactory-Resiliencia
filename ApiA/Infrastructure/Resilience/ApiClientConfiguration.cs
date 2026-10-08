namespace Infrastructure.Resilience
{
    /// <summary>
    /// Parámetros de las estrategias de resiliencia. Se instancia en Program.cs.
    /// Cambiá estos valores para ver distintos comportamientos.
    /// </summary>
    public class ApiClientConfiguration
    {
        // ---------- Retry ----------
        /// <summary>Cantidad máxima de reintentos (sin contar el intento original).</summary>
        public int RetryCount { get; set; } = 3;

        /// <summary>Espera entre reintentos, en segundos.</summary>
        public double RetryDelayInSeconds { get; set; } = 2;

        /// <summary>true = espera exponencial (2s, 4s, 8s...). false = espera fija.</summary>
        public bool UseExponentialBackoff { get; set; } = false;

        // ---------- Circuit Breaker ----------
        /// <summary>Proporción de fallas (0 a 1) que abre el circuito. 0.5 = 50%.</summary>
        public double FailureRatio { get; set; } = 0.5;

        /// <summary>Mínimo de requests dentro de la ventana antes de evaluar la proporción (mínimo 2).</summary>
        public int MinimumThroughput { get; set; } = 4;

        /// <summary>Ventana de tiempo en la que se cuentan las fallas, en segundos.</summary>
        public double SamplingDurationInSeconds { get; set; } = 30;

        /// <summary>Tiempo que el circuito permanece abierto, en segundos.</summary>
        public double BreakDurationInSeconds { get; set; } = 15;
    }
}
