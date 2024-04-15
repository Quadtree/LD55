using Godot;
using System;

public class AcceptSummoningButton : Button
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Connect("pressed", this, nameof(StartLevel));
    }

    void StartLevel()
    {
        Default.DifficultyOverride = InterLevelState.Singleton.Difficulty;

        string nextLevel = null;
        for (var i = 0; i < 10 && (nextLevel == null || nextLevel == InterLevelState.Singleton.LastLevelName); ++i)
        {
            if (Util.RandChance(0.5f))
                nextLevel = ("res://maps/Fort1.tscn");
            else
                nextLevel = ("res://maps/StaticLab1.tscn");
        }

        InterLevelState.Singleton.LastLevelName = nextLevel;

        GetTree().ChangeScene(nextLevel);
    }
}
