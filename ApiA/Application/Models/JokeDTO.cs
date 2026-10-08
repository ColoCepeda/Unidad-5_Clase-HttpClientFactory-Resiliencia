namespace Application.Models
{
    /// <summary>
    /// Formato de un chiste tal como lo devuelve la Official Joke API:
    /// { "type": "general", "setup": "...", "punchline": "...", "id": 1 }
    /// No hace falta [JsonPropertyName]: ReadFromJsonAsync/GetFromJsonAsync
    /// usan las opciones "web", que ignoran mayúsculas/minúsculas.
    /// </summary>
    public class JokeDTO
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Setup { get; set; } = string.Empty;
        public string Punchline { get; set; } = string.Empty;
    }
}
