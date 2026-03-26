using System.ComponentModel.DataAnnotations;

namespace PersonalBudgetTracker.Models;

public enum TransactionType { Income, Expense }

public class Transaction
{
    public int TransactionId { get; set; }

    [Required]
    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    [Required]
    public TransactionType Type { get; set; }

    [Required]
    public string Category { get; set; } = "";

    [MaxLength(200)]
    public string Description { get; set; } = "";

    [Required]
    public DateTime Date { get; set; } = DateTime.Today;

    
}
