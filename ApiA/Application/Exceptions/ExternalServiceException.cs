namespace Application.Exceptions
{
    /// <summary>
    /// Error al comunicarse con una API externa.
    /// Así el controller no necesita conocer detalles de HttpClient ni de Polly.
    /// </summary>
    public class ExternalServiceException : Exception
    {
        public int StatusCode { get; }

        public ExternalServiceException(string message, int statusCode, Exception? inner = null)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}
