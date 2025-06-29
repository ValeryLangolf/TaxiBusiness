using System;

public interface IDeactivatable<T>
{
    public event Action<T> Deactivated;

    void ReturnInPool();
}