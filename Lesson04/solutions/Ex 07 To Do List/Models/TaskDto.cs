using System.Text.Json.Serialization;

namespace Ex_07_To_Do_List.Models;

public class TaskDto
{
    public int Id { get; set; }
    public string Task { get; set; }
    public bool Done { get; set; }
    [JsonPropertyName("_links")]
    public List<Link> Links { get; set; } = [];

    public static List<Link> GenerateLinks(int id)
    {
        const string baseUrl = "/tasks";
        return new List<Link>
        {
            new() { Rel = "self", Href = $"{baseUrl}/{id}", Method = "GET" },
            new() { Rel = "update", Href = $"{baseUrl}/{id}", Method = "PUT" },
            new() { Rel = "delete", Href = $"{baseUrl}/{id}", Method = "DELETE" },
            new() { Rel = "collection", Href = baseUrl, Method = "GET" }
        };
    }
}