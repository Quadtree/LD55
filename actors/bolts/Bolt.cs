using Godot;
using System;

public class Bolt : MeshInstance
{
    [Export]
    public float HeatEffect = 0;

    [Export]
    public float DamageEffect = 0;

    public Spatial Target;

    public bool HasImpacted;

    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        LookAt(Target.GlobalTranslation, Vector3.Up);

        GlobalTranslate((Target.GlobalTranslation - GlobalTranslation).Normalized() * 20);

        if (!HasImpacted && Target.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) < 1)
        {
            HasImpacted = true;
            QueueFree();

            Flammable.AddHeat(Target, HeatEffect);
            Damagable.TakeDamage(Target, DamageEffect);
        }
    }
}
