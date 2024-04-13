using Godot;
using System;

public class Bolt : MeshInstance
{
    [Export]
    public float HeatEffect = 0;

    [Export]
    public float DamageEffect = 0;

    public Vector3 Trajectory;

    public Spatial Target;

    public bool HasImpacted;

    public float TimeToLive;

    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GlobalTranslate(Trajectory * 20 * delta);

        if (!HasImpacted && Target.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) < 1)
        {
            HasImpacted = true;
            QueueFree();

            Flammable.AddHeat(Target, HeatEffect);
            Damagable.TakeDamage(Target, DamageEffect);
        }
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
