﻿namespace P02_FootballBetting.Data.Models;

public class PlayerStatistic
{
    public int GameId { get; set; }
    public virtual Game Game { get; set; } = null!;


    public int PlayerId { get; set; }
    public virtual Player Player { get; set; } = null!;


    public int ScoredGoals { get; set; }


    public int Assists { get; set; }


    public int MinutesPlayed { get; set; }
}
