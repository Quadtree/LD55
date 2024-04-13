using Godot;
using System;
using System.Linq;

public class Fireball : MeshInstance
{
    public Vector3 Target;


    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (GlobalTranslation.DistanceSquaredTo(Target) > Mathf.Pow(0.5f, 2))
        {
            GlobalTranslation += (Target - GlobalTranslation).Normalized() * 16;
        }
        else
        {
            // detonation!
            foreach (var it in GetTree().CurrentScene.FindChildrenByType<Flammable>().Where(it => it.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) < Mathf.Pow(2.5f, 2.0f)))
            {
                it.Heat += 200;
            }
        }
    }
}
