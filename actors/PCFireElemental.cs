using Godot;
using System;
using System.Collections.Generic;

public class PCFireElemental : KinematicBody, Actor, HasFaction
{
    public float Mana = 50;

    public float SummonerSkill = 35;

    public float GlobalCooldown = 0;

    public bool HasBrokenFree = false;

    public float FirePoints = 0;

    public Spatial AsSpatial => this;
    public Actor AsActor => this;

    [Export]
    public float ManaRegenPerSecond = 20f;

    public const float GLOBAL_COOLDOWN = 0.75f;
    public const float FIREBALL_MANA_COST = 40;
    public const float FLAME_SLASH_MANA_COST = 20;

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

                MoveAndSlide((destVal - GlobalTranslation).Normalized() * 6);
            }
        }

        Util.SpeedUpPhysicsIfNeeded();

        GlobalCooldown = Util.Clamp(GlobalCooldown + delta, 0, 2);

        Mana = Util.Clamp(Mana + ManaRegenPerSecond * delta, 0, 100);

        var flammable = this.FindChildByType<Flammable>();
        var damagable = this.FindChildByType<Damagable>();

        if (flammable.Heat != 0)
        {
            damagable.Health += flammable.Heat;
            flammable.Heat = 0;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (Input.IsActionPressed("action") && GlobalCooldown >= 1)
        {
            var dest = Picking.PickPointAtCursor(this);

            if (dest != null)
            {
                GD.Print($"Dist={dest.Value.DistanceTo(GlobalTranslation)}");

                var validTargetArea = HasBrokenFree;

                if (!validTargetArea)
                {
                    var targetAreaPos = GetTree().CurrentScene.FindChildByType<TargetArea>().GlobalTranslation;

                    if (dest.Value.DistanceTo(targetAreaPos) > 4)
                    {
                        // we are trying to break free
                        if (Mana >= FIREBALL_MANA_COST)
                        {
                            var breakFreeRoll = Util.RandInt(0, 100);
                            if (breakFreeRoll >= SummonerSkill)
                            {
                                // we have broken free
                                HasBrokenFree = true;
                                validTargetArea = true;
                                GetTree().CurrentScene.FindChildByType<TargetArea>().QueueFree();
                            }
                            else
                            {
                                // failed to break free
                                Mana -= FIREBALL_MANA_COST;
                                GlobalCooldown = 0;
                            }
                        }
                    }
                    else
                    {
                        validTargetArea = true;
                    }
                }


                if (validTargetArea)
                {
                    if (dest.Value.DistanceTo(GlobalTranslation) < 2.5f)
                    {
                        GD.Print("Flame slash!");
                        FlameSlash();
                    }
                    else
                    {
                        GD.Print("Fireball!");
                        Fireball(dest.Value);
                    }
                }
            }
        }
    }

    public void FlameSlash()
    {
        if (Mana < FLAME_SLASH_MANA_COST) return;
        if (GlobalCooldown < GLOBAL_COOLDOWN) return;

        Mana -= FLAME_SLASH_MANA_COST;
        GlobalCooldown = 0;

        foreach (var it in this.FindChildByType<Area>().GetOverlappingBodies())
        {
            GD.Print(it);
            Flammable.AddHeat((Node)it, 150);
        }
    }

    public void Fireball(Vector3 target)
    {
        if (Mana < FIREBALL_MANA_COST) return;
        if (GlobalCooldown < GLOBAL_COOLDOWN) return;

        Mana -= FIREBALL_MANA_COST;
        GlobalCooldown = 0;

        var fb = GD.Load<PackedScene>("res://actors/Fireball.tscn").Instance<Fireball>();
        GetTree().CurrentScene.AddChild(fb);
        fb.GlobalTranslation = GlobalTranslation;
        fb.Target = target;
    }

    public int FactionId => HasBrokenFree ? 2 : 0;
}
