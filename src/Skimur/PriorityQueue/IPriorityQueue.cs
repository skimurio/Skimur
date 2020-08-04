using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skimur.PriorityQueue
{
    /// <summary>
    /// The IPriorityQueue interface. This is mainly here for purists.
    /// For speed purposes, it is recommended that you don't access the
    /// priority queue through this interfase, since JTT can
    /// (theoretically?) optimize calls from concrete-type slightly better.
    /// </summary>
    public interface IPriorityQueue<T> : IEnumerable<T> where T : PriorityQueueNode
    {
        void Remove(T node);

        void UpdatePriority(T node, double priority);

        void Enqueue(T node, double priority);

        T Dequeue();

        T First { get; }

        int Count { get; }

        int MaxSize { get; }

        void Clear();

        bool Contains(T node);

        void Resize(int maxNodes);
    }
}
