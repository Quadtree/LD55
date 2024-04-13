using Godot;
using System;

public class Flammable : Spatial
{
    public float Heat = 0;

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
        }
        else
        {
            Visible = false;
        }
    }

    public static void AddHeat(Node node, float heat)
    {
        var child = node.FindChildByType<Flammable>();
        if (child != null)
        {
            GD.Print($"Adding {heat} to {child}");
            child.Heat += heat;
        }
    }
}
