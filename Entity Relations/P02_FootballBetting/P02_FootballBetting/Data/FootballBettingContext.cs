using Microsoft.EntityFrameworkCore;

namespace P02_FootballBetting.Data;

public class FootballBettingContext : DbContext
{
	public FootballBettingContext()
	{

	}

	public FootballBettingContext(DbContextOptions<FootballBettingContext> options) : base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{

	}
}
