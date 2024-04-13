using Godot;
using System;

public class Damagable : Node
{
    float Health = 100;

    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {

    }

    public static void TakeDamage(Node node, float damage)
    {
        var child = node.GetActor().FindChildByType<Damagable>();
        if (child != null)
        {
            child.Health -= damage;
        }
    }
}
