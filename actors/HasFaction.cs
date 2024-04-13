using Godot;

interface HasFaction
{
    int FactionId { get; }

    Vector3 GlobalTranslation { get; }

    Actor AsActor { get; }
}