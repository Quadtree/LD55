using Godot;
using System;

public class ManaBar : TextureProgress
{
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<PCFireElemental>();
        if (pc != null)
        {
            Value = pc.Mana;
        }
    }
}
