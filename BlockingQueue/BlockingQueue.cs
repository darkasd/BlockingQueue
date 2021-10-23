using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace BlockingQueue
{
    public class BlockingQueue<T>
    {
        private readonly AutoResetEvent _waiter = new AutoResetEvent(false);

        private readonly object _locker = new object();

        private readonly Queue<T> _internalQueue = new Queue<T>();

        public void Push(T item)
        {
            lock(_locker)
            {
                _internalQueue.Enqueue(item);
                _waiter.Set();
            }
        }

        public T Pull()
        {
            var isEmpty = true;

            lock (_locker)
            {
                isEmpty = _internalQueue.Count == 0;
            }

            if (isEmpty)
            {
                _waiter.WaitOne();
            }

            lock (_locker)
            {
                return _internalQueue.Dequeue();
            }
        }

        public int Count()
        {
            var count = 0;

            lock (_locker)
            {
                count = _internalQueue.Count;
            }

            return count;
        }
    }
}
