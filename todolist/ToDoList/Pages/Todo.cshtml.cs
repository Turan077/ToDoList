using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Pages
{
    public class TodoModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TodoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Todo EditTodo { get; set; } = new Todo();

        public IList<Todo> Todos { get; set; } = new List<Todo>();

        public async Task OnGetAsync()
        {
            Todos = await _context.Todos.OrderByDescending(t => t.CreatedDate).ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadTodosAsync();
                return Page();
            }

            if (EditTodo.Id > 0)
            {
                // Güncelleme
                var todo = await _context.Todos.FindAsync(EditTodo.Id);
                if (todo == null) return NotFound();
                todo.Title = EditTodo.Title;
                todo.Description = EditTodo.Description;
                todo.Priority = EditTodo.Priority;
                todo.DueDate = EditTodo.DueDate;
                todo.Category = EditTodo.Category ?? "";
                await _context.SaveChangesAsync();
            }
            else
            {
                // Yeni ekleme
                EditTodo.Category = EditTodo.Category ?? "";
                _context.Todos.Add(EditTodo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return NotFound();
            EditTodo = new Todo
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Priority = todo.Priority,
                Category = todo.Category,
                DueDate = todo.DueDate
            };
            await LoadTodosAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostCancelEditAsync()
        {
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostToggleCompleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsCompleted = !todo.IsCompleted;
            todo.CompletedDate = todo.IsCompleted ? DateTime.Now : null;

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        private async Task LoadTodosAsync()
        {
            Todos = await _context.Todos.OrderByDescending(t => t.CreatedDate).ToListAsync();
        }
    }
} 