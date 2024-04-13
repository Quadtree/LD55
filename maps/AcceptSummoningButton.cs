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

        if (Util.RandChance(0.5f))
            GetTree().ChangeScene("res://maps/Fort1.tscn");
        else
            GetTree().ChangeScene("res://maps/StaticLab1.tscn");
    }
}
