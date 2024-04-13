using Godot;
using System;

public class Prop : MeshInstance, Actor
{
    public Spatial AsSpatial => this;

    public override void _Ready()
    {
        GD.Print("PROP!");
    }

    public override void _Process(float delta)
    {
        if (this.FindChildByType<Damagable>().Health <= 0)
        {
            QueueFree();
        }
    }
}
