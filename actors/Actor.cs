using Godot;

public interface Actor
{
    Spatial AsSpatial { get; }
}

public static class ActorExtensions
{
    public static Node GetActor(this Node self)
    {
        if (self is Actor) return self;
        return (Node)self.FindParentByType<Actor>();
    }
}