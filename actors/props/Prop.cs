using Godot;
using System;

public class Prop : MeshInstance, Actor
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("PROP!");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (this.FindChildByType<Damagable>().Health <= 0)
        {
            QueueFree();
        }
    }
}
