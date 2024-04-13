using Godot;
using System;

public class PCFireElemental : KinematicBody
{
    public float Mana = 100;

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

        Mana = Util.Clamp(Mana + ManaRegenPerSecond * delta, 0, 100);
    }
}
