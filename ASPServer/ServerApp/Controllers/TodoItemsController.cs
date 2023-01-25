using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // GET: api/TodoItem
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        if (_context.TodoItems == null) return NotFound();
        return await _context.TodoItems
                   .Select(x => ItemToDTO(x))
                   .ToListAsync();
    }

    // GET: api/TodoItem/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        if (_context.TodoItems == null) return NotFound();
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null) return NotFound();

        return ItemToDTO(todoItem);
    }

    // PUT: api/TodoItem/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItem)
    {
        if (id != todoItem.Id) return BadRequest();

        _context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/TodoItem
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
    {
        if (_context.TodoItems == null) return Problem("Entity set 'TodoContext.TodoItems'  is null.");
        var todoItem = new TodoItem
        {
            IsComplete = todoItemDTO.IsComplete,
            Name = todoItemDTO.Name
        };
        
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
    }

    // DELETE: api/TodoItem/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        if (_context.TodoItems == null) return NotFound();
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }
    
    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
        new TodoItemDTO
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
}