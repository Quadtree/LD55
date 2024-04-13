using Godot;
using System;

public class WardingRune : StaticBody
{
    public float Health = 10;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Health <= 0) QueueFree();
    }
}
