using System.Collections.Generic;

public class ModifiableValue<T>
{
    private T original;
    public T Value { get; set; }

    public bool Modified()
    {
        return !EqualityComparer<T>.Default.Equals(original, Value);
    }

    public void Consume()
    {
        original = Value;
    }
}
