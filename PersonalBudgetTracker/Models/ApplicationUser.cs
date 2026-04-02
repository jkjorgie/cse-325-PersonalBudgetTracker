using Microsoft.AspNetCore.Identity;

namespace PersonalBudgetTracker.Models;

public class ApplicationUser : IdentityUser
{
    // list of all transactions belonging to this user
    public ICollection<Transaction> Transactions { get; set; } = [];
}
