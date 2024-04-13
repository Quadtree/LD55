using Godot;
using System;

public class UpgradePointsLabel : Label
{
    Func<string> Templater;

    public override void _Ready()
    {
        var txt = Text;
        Templater = () => txt.Replace("<PTS>", InterLevelState.Singleton.AvailableUpgradePoints.ToString());
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Text = Templater();
    }
}
