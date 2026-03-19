using Microsoft.AspNetCore.Identity;

namespace PersonalBudgetTracker.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<Transaction> Transactions { get; set; } = [];
}
