using Godot;
using System;

public class Human : KinematicBody, Actor
{
    Vector3 BurnRun;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BurnRun = new Vector3(
                    Util.RandF(-1, 1),
                    0,
                    Util.RandF(-1, 1)
                ).Normalized();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (this.FindChildByType<Flammable>()?.IsOnFire == true)
        {
            if (Util.RandChance(delta))
            {
                BurnRun = new Vector3(
                    Util.RandF(-1, 1),
                    0,
                    Util.RandF(-1, 1)
                ).Normalized();
            }

            MoveAndSlide(BurnRun * 4);
            return;
        }
    }
}
