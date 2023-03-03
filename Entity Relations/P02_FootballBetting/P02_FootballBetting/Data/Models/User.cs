using P02_FootballBetting.Data.Common;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class User
{
    public User()
    {
        this.Bets= new HashSet<Bet>();
    }


    [Key]
    public int UserId { get; set; }

    [MaxLength(ValidationConstants.UserUserNameMaxLenght)]
    public string Username { get; set; } = null!;


    [MaxLength(ValidationConstants.UserPaswwordMaxLengt)]
    public string Password { get; set; } = null!;


    [MaxLength(ValidationConstants.UserEmailMaxLengt)]
    public string Email { get; set; } = null!;


    [MaxLength(ValidationConstants.UserNameMaxLengt)]
    public string Name { get; set; } = null!;


    public decimal Balance { get; set; }


    public virtual ICollection<Bet> Bets { get; set; }
}
