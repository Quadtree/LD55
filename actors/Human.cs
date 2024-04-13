using Godot;
using System;
using System.Linq;

public class Human : KinematicBody, Actor, HasFaction, DifficultyIncreasing
{
    enum BoltType
    {
        FireBolt,
        WaterBolt,
        WoodBolt
    }

    [Export]
    public bool CanCastFireBolt;

    [Export]
    public bool CanCastWaterBolt;

    [Export]
    public bool CanCastWoodBolt;

    [Export]
    public bool IsSummoner;

    [Export]
    public int SummonerSkill = 35;

    public int EffectiveSummonerSkill => (IsSummoner && this.FindChildByType<Damagable>().Health > 0) ? SummonerSkill : 0;

    [Export]
    public int FactionId { get; set; }

    public float BoltCharge;

    public Spatial AsSpatial => this;
    public Actor AsActor => this;

    [Export]
    public float DifficultyAdded { get; set; } = 2;

    public Actor MainTarget;

    Vector3 BurnRun;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        BurnRun = new Vector3(
                    Util.RandF(-1, 1),
                    0,
                    Util.RandF(-1, 1)
                ).Normalized();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        if (this.FindChildByType<Damagable>().Health <= 0)
        {
            GlobalRotation = new Vector3(0, GlobalRotation.y, Mathf.Pi / 2);
            return;
        }

        BoltCharge += delta;

        if (this.FindChildByType<Flammable>()?.IsOnFire == true)
        {
            if (Util.RandChance(delta))
            {
                BurnRun = new Vector3(
                    Util.RandF(-1, 1),
                    0,
                    Util.RandF(-1, 1)
                ).Normalized();
            }

            MoveTowards(GlobalTranslation + BurnRun * 10);
            return;
        }

        if (Util.RandChance(delta))
        {
            Scan();
        }

        if (MainTarget != null)
        {
            if ((MainTarget.AsSpatial.FindChildByType<Damagable>()?.Health ?? 0) <= 0)
            {
                MainTarget = null;
            }
            else
            {
                if (MainTarget.AsSpatial.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) > 4 * 4)
                {
                    // we're too far away. get closer
                    MoveTowards(MainTarget.AsSpatial.GlobalTranslation);
                }
                else
                {
                    if (CanCastFireBolt && !(MainTarget is PCFireElemental))
                    {
                        CastBoltAt(MainTarget, BoltType.FireBolt);
                    }
                    else if (CanCastWaterBolt && MainTarget is PCFireElemental)
                    {
                        CastBoltAt(MainTarget, BoltType.WaterBolt);
                    }
                    else if (CanCastWoodBolt)
                    {
                        CastBoltAt(MainTarget, BoltType.WoodBolt);
                    }
                    else if (CanCastWaterBolt)
                    {
                        CastBoltAt(MainTarget, BoltType.WaterBolt);
                    }
                }
            }
        }
    }

    void Scan()
    {
        var possibleTargets = GetTree().CurrentScene.FindChildrenByType<HasFaction>()
            .Where(it => it.FactionId != this.FactionId)
            .Where(it => it.AsActor.AsSpatial.FindChildByType<Damagable>().Health > 0);

        if (possibleTargets.Any())
        {
            MainTarget = possibleTargets.MaxBy(it =>
            {
                var threat = 100.0f;

                threat -= it.GlobalTranslation.DistanceSquaredTo(GlobalTranslation);

                return threat;
            }) as Actor;
        }
    }

    void MoveTowards(Vector3 v3)
    {
        v3.y = 0.0f;

        //LookAt(v3, Vector3.Up);

        MoveAndSlide((v3 - GlobalTranslation).Normalized() * 4);
    }

    void CastBoltAt(Actor target, BoltType type)
    {
        if (BoltCharge <= 1.5f) return;
        BoltCharge = 0;

        GD.Print($"Casting {type} at {target}");

        var bolt = GD.Load<PackedScene>($"res://actors/bolts/{type}.tscn").Instance<Bolt>();
        GetTree().CurrentScene.AddChild(bolt);
        bolt.AimAtPoint(GlobalTranslation, target.AsSpatial);
    }
}
