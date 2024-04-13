using Godot;

public interface Actor
{

}

public static class ActorExtensions
{
    public static Actor GetActor(this Node self)
    {
        if (self is Actor) return (Actor)self;
        return self.FindParentByType<Actor>();
    }
}