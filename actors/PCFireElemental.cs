using Godot;
using System;

public class PCFireElemental : KinematicBody
{
    public float Mana = 50;

    public float GlobalCooldown = 0;

    [Export]
    public float ManaRegenPerSecond = 20f;

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("move"))
        {
            var dest = Picking.PickPointAtCursor(this);

            if (dest != null)
            {
                var cam = this.FindChildByType<Camera>();

                var ct = cam.GlobalTransform;

                var destVal = dest.Value;
                destVal.y = this.GetGlobalLocation().y;

                LookAt(destVal, Vector3.Up);

                cam.GlobalTransform = ct;

                MoveAndSlide(destVal - GlobalTranslation);
            }
        }

        if (Input.IsActionPressed("action") && GlobalCooldown >= 1)
        {
            var dest = Picking.PickPointAtCursor(this);

            if (dest != null)
            {
                GD.Print($"Dist={dest.Value.DistanceTo(GlobalTranslation)}");

                if (dest.Value.DistanceTo(GlobalTranslation) < 2.5f)
                {
                    GD.Print("Flame slash!");
                    FlameSlash();
                }
                else
                {
                    GD.Print("Fireball!");
                }
            }
        }

        GlobalCooldown = Util.Clamp(GlobalCooldown + delta, 0, 1);

        Mana = Util.Clamp(Mana + ManaRegenPerSecond * delta, 0, 100);
    }

    public void FlameSlash()
    {


        // var pp = new PhysicsShapeQueryParameters();
        // pp.ShapeRid = this.FindChildByName<CollisionShape>("FlameSlashShape").Shape.GetRid();
        // pp.CollisionMask

        // GetWorld().DirectSpaceState.IntersectShape()

        if (Mana < 20) return;
        if (GlobalCooldown < 1) return;

        Mana -= 20;
        GlobalCooldown = 0;



        foreach (var it in this.FindChildByType<Area>().GetOverlappingBodies())
        {
            GD.Print(it);
        }
    }
}
