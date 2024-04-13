using Godot;

public interface Actor
{

}

public static class ActorExtensions
{
    public static Node GetActor(this Node self)
    {
        if (self.Owner == null) return self;
        return self.Owner;
    }
}