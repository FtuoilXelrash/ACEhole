using System.Collections.Generic;

namespace ACEhole
{
    /// <summary>
    /// Fixed-capacity FIFO buffer. Oldest entries are dropped when capacity is exceeded.
    /// Used to cap the console RichTextBox line count and prevent memory growth.
    /// </summary>
    public class CircularBuffer<T>
    {
        private readonly Queue<T> _queue;
        private readonly int _capacity;

        public CircularBuffer(int capacity)
        {
            _capacity = capacity;
            _queue = new Queue<T>(capacity);
        }

        public int Count => _queue.Count;

        public void Add(T item)
        {
            if (_queue.Count >= _capacity)
                _queue.Dequeue();
            _queue.Enqueue(item);
        }

        public IEnumerable<T> Items => _queue;

        public void Clear() => _queue.Clear();
    }
}
