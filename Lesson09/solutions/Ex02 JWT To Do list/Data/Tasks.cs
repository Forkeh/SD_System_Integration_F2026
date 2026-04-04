using Ex_07_To_Do_List.Models;

namespace Ex_07_To_Do_List.Data;

public static class Tasks
{
    public static List<TaskDto> TasksList { get; } =
    [
        new() { Id = 1, Task = "Learn REST APIs", Done = false, Links = TaskDto.GenerateLinks(1) },
        new() { Id = 2, Task = "Build a REST API", Done = false, Links = TaskDto.GenerateLinks(2) },
        new() { Id = 3, Task = "Test the API", Done = false, Links = TaskDto.GenerateLinks(3) },
        new() { Id = 4, Task = "Keep learning", Done = false, Links = TaskDto.GenerateLinks(4) },
        new() { Id = 5, Task = "Read about HTTP methods", Done = false, Links = TaskDto.GenerateLinks(5) },
        new() { Id = 6, Task = "Understand status codes", Done = false, Links = TaskDto.GenerateLinks(6) },
        new() { Id = 7, Task = "Practice with Postman", Done = false, Links = TaskDto.GenerateLinks(7) },
        new() { Id = 8, Task = "Add authentication", Done = false, Links = TaskDto.GenerateLinks(8) },
        new() { Id = 9, Task = "Implement pagination", Done = false, Links = TaskDto.GenerateLinks(9) },
        new() { Id = 10, Task = "Write API documentation", Done = false, Links = TaskDto.GenerateLinks(10) },
        new() { Id = 11, Task = "Add input validation", Done = false, Links = TaskDto.GenerateLinks(11) },
        new() { Id = 12, Task = "Handle errors gracefully", Done = false, Links = TaskDto.GenerateLinks(12) },
        new() { Id = 13, Task = "Add logging", Done = false, Links = TaskDto.GenerateLinks(13) },
        new() { Id = 14, Task = "Connect to a database", Done = false, Links = TaskDto.GenerateLinks(14) },
        new() { Id = 15, Task = "Deploy to the cloud", Done = false, Links = TaskDto.GenerateLinks(15) },
        new() { Id = 16, Task = "Set up CI/CD pipeline", Done = false, Links = TaskDto.GenerateLinks(16) },
        new() { Id = 17, Task = "Write unit tests", Done = false, Links = TaskDto.GenerateLinks(17) },
        new() { Id = 18, Task = "Write integration tests", Done = false, Links = TaskDto.GenerateLinks(18) },
        new() { Id = 19, Task = "Review security best practices", Done = false, Links = TaskDto.GenerateLinks(19) },
        new() { Id = 20, Task = "Refactor and clean up code", Done = false, Links = TaskDto.GenerateLinks(20) }
    ];
}