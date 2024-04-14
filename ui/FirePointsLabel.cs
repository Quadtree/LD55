using Godot;
using System;

public class FirePointsLabel : Label
{
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<PCFireElemental>();
        if (pc != null)
        {
            Text = $"Fire Points: {pc.FirePoints:n0}";
        }
    }
}
