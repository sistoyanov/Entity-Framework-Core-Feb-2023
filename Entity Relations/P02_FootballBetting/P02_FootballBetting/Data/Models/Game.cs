﻿using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    public Game()
    {
        this.Bets = new HashSet<Bet>();
        this.PlayersStatistics = new HashSet<PlayerStatistic>();
    }


    [Key]
    public int GameId { get; set; }


    //[ForeignKey(nameof(HomeTeam))]
    public int HomeTeamId { get; set; }
    public virtual Team HomeTeam { get; set; } = null!;


    //[ForeignKey(nameof(AwayTeam))]
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;


    public int HomeTeamGoals { get; set; }


    public int AwayTeamGoals { get; set; }


    public DateTime DateTime { get; set; }


    public double HomeTeamBetRate { get; set; }


    public double AwayTeamBetRate { get; set; }


    public double DrawBetRate { get; set; }


    public string? Result { get; set; }


    public virtual ICollection<Bet> Bets { get; set; }


    public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; }
}
