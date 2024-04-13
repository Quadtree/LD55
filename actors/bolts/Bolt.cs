using Godot;
using System;

public class Bolt : MeshInstance
{
    [Export]
    public float HeatEffect = 0;

    [Export]
    public float DamageEffect = 0;

    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
