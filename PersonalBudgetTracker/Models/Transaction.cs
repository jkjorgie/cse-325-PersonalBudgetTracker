using System.ComponentModel.DataAnnotations;

namespace PersonalBudgetTracker.Models;

// used to tell if a transaction is money coming in or going out
public enum TransactionType { Income, Expense }

public class Transaction
{
    public int TransactionId { get; set; }

    // links this transaction to whoever is logged in
    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    [Required]
    public string Category { get; set; } = "";

    [MaxLength(200)]
    public string Description { get; set; } = "";

    [Required]
    public DateTime Date { get; set; } = DateTime.Today;
}
