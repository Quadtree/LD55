using Godot;
using System;

public class Bolt : Spatial
{
    [Export]
    public float HeatEffect = 0;

    [Export]
    public float DamageEffect = 0;

    public Vector3 Trajectory;

    public Spatial Target;

    public bool HasImpacted;

    public float TimeToLive = 4;

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        GlobalTranslate(Trajectory * 10 * delta);

        if (!HasImpacted && Target.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) < 1)
        {
            HasImpacted = true;
            QueueFree();

            Flammable.AddHeat(Target, HeatEffect);
            Damagable.TakeDamage(Target, DamageEffect);
        }

        TimeToLive -= delta;
        if (TimeToLive < 0) QueueFree();
    }

    public void AimAtPoint(Vector3 sourcePoint, Spatial target)
    {
        var targetPoint = target.GlobalTranslation;

        sourcePoint.y = 0.5f;
        targetPoint.y = 0.5f;

        GlobalTranslation = sourcePoint;
        LookAt(targetPoint, Vector3.Up);
        Target = target;

        Trajectory = (targetPoint - sourcePoint).Normalized();
    }
}
