using Godot;
using System;

public class ReturnToPoolButton : Button
{
    public override void _Ready()
    {
        if (InterLevelState.Singleton.Level == 5) Text = "Return to Title";

        Connect("pressed", this, nameof(StartLevel));
    }

    void StartLevel()
    {
        GetTree().ChangeScene("res://maps/LevelStartScreen.tscn");
    }
}
