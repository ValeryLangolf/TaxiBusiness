using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    private readonly List<T> _elements = new();

    public int Count => _elements.Count;

    public void Enqueue(T item)
    {
        _elements.Add(item);
        int childIndex = _elements.Count - 1;

        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;

            if (_elements[childIndex].CompareTo(_elements[parentIndex]) >= 0)
                break;

            (_elements[childIndex], _elements[parentIndex]) =
                (_elements[parentIndex], _elements[childIndex]);

            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        T frontItem = _elements[0];
        int lastIndex = _elements.Count - 1;
        _elements[0] = _elements[lastIndex];
        _elements.RemoveAt(lastIndex);

        int parentIndex = 0;
        while (true)
        {
            int leftChildIndex = parentIndex * 2 + 1;

            if (leftChildIndex >= _elements.Count)
                break;

            int rightChildIndex = leftChildIndex + 1;
            int minIndex = leftChildIndex;

            if (rightChildIndex < _elements.Count && _elements[rightChildIndex].CompareTo(_elements[leftChildIndex]) < 0)
                minIndex = rightChildIndex;

            if (_elements[parentIndex].CompareTo(_elements[minIndex]) <= 0)
                break;

            (_elements[parentIndex], _elements[minIndex]) = (_elements[minIndex], _elements[parentIndex]);

            parentIndex = minIndex;
        }

        return frontItem;
    }

    public bool Contains(Func<T, bool> predicate)
    {
        foreach (T element in _elements)
        {
            if (predicate(element))
                return true;
        }

        return false;
    }
}