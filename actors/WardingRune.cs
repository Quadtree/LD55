using Godot;
using System;

public class WardingRune : StaticBody
{
    public float Health = 10;

    public SpatialMaterial LocalMat;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LocalMat = this.FindChildByType<MeshInstance>().GetActiveMaterial(0).Duplicate() as SpatialMaterial;
        this.FindChildByType<MeshInstance>().MaterialOverride = (LocalMat);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        LocalMat.EmissionEnergy = Health * 0.25f;

        if (Health <= 0)
        {
            CollisionLayer = 0;
            CollisionMask = 0;
        }
    }
}
