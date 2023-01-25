using Microsoft.EntityFrameworkCore;

namespace ServerApp.Models;

public class TodoContext : DbContext
{
    // 이걸 외워야한다고요
    // 그럴수도 있지
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    // 느낌표 왜 붙이는거지. 명시적인 null값 대입인가? 아닐거같은데
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}