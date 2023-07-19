
public struct WorldObject 
{
    public(int, int) position { get; }
    public string type { get; }

    public WorldObject((int, int) position, string type)
    {
        this.position = position;
        this.type = type;
    }
}
