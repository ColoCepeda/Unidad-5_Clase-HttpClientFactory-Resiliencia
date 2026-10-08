namespace Application.Models
{
    /// <summary>
    /// Respuesta exitosa de la API B: { "message": "...", "attempts": 6 }
    /// </summary>
    public class ApiBResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public int Attempts { get; set; }
    }
}
