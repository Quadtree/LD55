using Godot;
using System;

public class ReturnToPoolButton : Button
{
    public override void _Ready()
    {
        Connect("pressed", this, nameof(StartLevel));
    }

    void StartLevel()
    {
        GetTree().ChangeScene("res://maps/LevelStartScreen.tscn");
    }
}
