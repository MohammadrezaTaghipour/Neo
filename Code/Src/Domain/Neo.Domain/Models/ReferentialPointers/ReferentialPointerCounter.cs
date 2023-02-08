namespace Neo.Domain.Models.ReferentialPointers;

public class ReferentialPointerCounter
{
    public ReferentialPointerCounter(Guid id, string pointerType)
    {
        Id = id;
        PointerType = pointerType;
        Counter = 0;
    }

    public ReferentialPointerCounter(Guid id,
        string pointerType, long counter)
    {
        Id = id;
        Counter = counter;
        PointerType = pointerType;
    }

    public Guid Id { get; set; }
    public string PointerType { get; }
    public long Counter { get; }

    public ReferentialPointerCounter Increase()
    {
        return new ReferentialPointerCounter(Id,
            PointerType, Counter + 1);
    }

    public ReferentialPointerCounter Decrease()
    {
        return new ReferentialPointerCounter(Id,
            PointerType, Counter - 1);
    }
}