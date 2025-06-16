using System;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir")]
        public string Title { get; set; } = "";

        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
        public string Description { get; set; } = "";

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? CompletedDate { get; set; }

        public Priority Priority { get; set; } = Priority.Medium;

        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = "";
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }
} 