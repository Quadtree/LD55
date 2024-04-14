using Godot;
using System;
using System.Linq;

public class StatusLabel : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var pc = GetTree().CurrentScene.FindChildByType<PCFireElemental>();
        Text = $"Mana: {pc.Mana:n0}     FP: {pc.FirePoints:n0}     HP: {pc.FindChildByType<Damagable>().Health:n0}";
    }
}
