using Godot;
using System;

public class LevelOverDesc : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Text = $"You've completed your summoning! You managed to obtain {InterLevelState.Singleton.LastLevelFirePoints:n0} out of your Fire Quota of {InterLevelState.Singleton.CurrentFireQuota:n0}.";

        if (InterLevelState.Singleton.LastLevelFirePoints >= InterLevelState.Singleton.CurrentFireQuota)
        {
            
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
