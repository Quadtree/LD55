using Godot;
using System;

public class TimeLeftLabel : Label
{
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<PCFireElemental>();
        if (pc != null)
        {
            Text = $"Time: {pc.FindChildByType<Damagable>().Health / PCFireElemental.DECAY_RATE:n0}";
        }
    }
}
