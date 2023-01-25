namespace ServerApp.Models;

public class TodoItem
{
    // Id 속성은 관계형 데이터베이스에서 고유 키로 작동
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public string? Secret { get; set; }
}