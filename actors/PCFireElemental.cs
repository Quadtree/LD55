using Godot;
using System;

public class PCFireElemental : Spatial
{
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
                LookAt(dest.Value, Vector3.Up);
            }
        }
    }
}
