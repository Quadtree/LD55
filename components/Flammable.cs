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

    public const float FIRE_SPEED = 25;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Heat >= 100)
        {
            Visible = true;

            if (Util.RandChance(delta))
            {
                foreach (var it in GetTree().CurrentScene.FindChildrenByType<Flammable>().Where(it => it.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) < Mathf.Pow(2.5f, 2.0f)))
                {
                    if (it == this) continue;

                    it.Heat = Util.Clamp(Heat + FIRE_SPEED * Flammability, 0, 1000);
                }
            }

            Damagable.TakeDamage(this, 5 * delta);
        }
        else
        {
            Visible = false;
        }

        Heat = Util.Clamp(Heat - delta * FIRE_SPEED, 0, 1000);
    }

    public static void AddHeat(Node node, float heat)
    {
        GD.Print("ACTOR " + node.FindParentByType<Actor>());
        var child = node.GetActor().FindChildByType<Flammable>();
        if (child != null)
        {
            GD.Print($"Adding {heat} to {child}");
            child.Heat += heat;
        }
    }
}
