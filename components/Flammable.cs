using Godot;
using System;
using System.Linq;

public class Flammable : Spatial
{
    public float Heat = 0;

    [Export]
    public float IgnitionPoint = 100;

    [Export]
    public float Flammability = 0.65f;

    public bool IsOnFire => Heat >= IgnitionPoint;

    public const float FIRE_SPEED = 25;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.FindChildByName<Particles>("Fire").Visible = true;
        this.FindChildByName<Particles>("Smoke").Visible = true;
        this.FindChildByName<Particles>("Fire").Emitting = false;
        this.FindChildByName<Particles>("Smoke").Emitting = false;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (IsOnFire)
        {
            this.FindChildByType<OmniLight>().Visible = true;
            this.FindChildByName<Particles>("Fire").Emitting = true;

            if (Util.RandChance(delta))
            {
                foreach (var it in GetTree().CurrentScene.FindChildrenByType<Flammable>().Where(it => it.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) < Mathf.Pow(2.5f, 2.0f)))
                {
                    if (it == this) continue;

                    it.Heat = Util.Clamp(Heat + FIRE_SPEED * Flammability, 0, 1000);
                }
            }

            Damagable.TakeDamage(this, 10 * delta);

            var pc = GetTree().CurrentScene.FindChildByType<PCFireElemental>();
            if (pc != null)
            {
                pc.FirePoints += delta;
                pc.Mana += delta * 2;
            }
        }
        else
        {
            this.FindChildByType<OmniLight>().Visible = false;
            this.FindChildByName<Particles>("Fire").Emitting = false;
        }

        if (Heat > 5)
        {
            this.FindChildByName<Particles>("Smoke").Emitting = true;
        }
        else
        {
            this.FindChildByName<Particles>("Smoke").Emitting = false;
        }

        GlobalRotation = new Vector3(0, 0, 0);

        Heat = Util.Clamp(Heat - delta * FIRE_SPEED, 0, 1000);
    }

    public static void AddHeat(Node node, float heat)
    {
        GD.Print("ACTOR " + node.Owner);
        var child = node.GetActor().FindChildByType<Flammable>();
        if (child != null)
        {
            GD.Print($"Adding {heat} to {child}");
            child.Heat += heat;
        }
    }
}
