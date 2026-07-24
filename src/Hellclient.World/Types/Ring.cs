namespace Hellclient.World.Types;

public class Ring<T> where T : class
{
    public Ring(int capacity)
    {
        Capacity = capacity;
        _items = new List<T?>(capacity);
    }
    private List<T?> _items { get; init; } = new List<T?>();
    private int _index { get; set; } = 0;
    private int _count { get; set; } = 0;
    public int Capacity { get; set; } = 0;
    public void Add(T item)
    {
        if (_items.Count < Capacity)
        {
            _items.Add(item);
            _count++;
        }
        else
        {
            _items[_index] = item;
            _index = (_index + 1) % Capacity;
        }
    }
    public T? GetItemByIndex(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            return null;
        }
        return _items[index];
    }
    public List<T> GetAllItems()
    {
        return GetRecentItems(_count);
    }
    public int Count()
    {
        return _count;
    }
    public void Flush()
    {
        _items.Clear();
        _index = 0;
        _count = 0;
    }
    public List<T> GetRecentItems(int count)
    {
        var result = new List<T>();
        if (count == 0 || _count == 0 || _items.Count == 0)
        {
            return result;
        }
        bool direction = count > 0;
        count = count > 0 ? count : -count;
        if (count > _items.Count)
        {
            count = _items.Count;
        }
        var remain = count;
        var current = _index;
        while (remain > 0)
        {
            current = direction ? (current - 1 + _items.Count) % _items.Count : (current + 1) % _items.Count;
            if (_items[current] != null)
            {
                result.Add(_items[current]!);
                remain--;
            }
            if (current == _index)
            {
                break;
            }
        }
        return result;
    }
    public T? GetCurrentItem()
    {
        if (_items.Count == 0)
        {
            return null;
        }
        var currentIndex = (_index - 1 + _items.Count) % _items.Count;
        return _items[currentIndex];
    }
    public int DeleteItems(int count)
    {
        if (count <= 0 || _count == 0)
        {
            return 0;
        }
        if (count > _count)
        {
            count = _count;
        }
        var deletedCount = 0;
        var currentIndex = (_index + _items.Count) % _items.Count;
        while (deletedCount < count)
        {
            currentIndex = (currentIndex - 1 + _items.Count) % _items.Count;
            if (_items[currentIndex] != null)
            {
                _items[currentIndex] = null;
                deletedCount++;
                _count--;
            }
            if (currentIndex == _index)
            {
                currentIndex = 0;
                break;
            }
        }
        _index = currentIndex;
        return deletedCount;
    }
}