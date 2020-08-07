using System;
using System.Collections;
using System.Collections.Generic;

namespace Skimur.PriorityQueue
{
    /// <summary>
    /// An implementation of a min-Priority Queue using a heap. Has O(1) .Contains()!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class HeapPriorityQueue<T> : IPriorityQueue<T> where T : PriorityQueueNode
    {
        private int _numNodes;
        private T[] _nodes;
        private long _numNodesEverEnqueued;

        /// <summary>
        /// Instantiate a new Priority Queue
        /// </summary>
        /// <param name="maxNodes">The max nodes ever allowed to be enqueued (going over this will cause
        /// undefined behavior)</param>
        public HeapPriorityQueue(int maxNodes)
        {
#if DEBUG
            if (maxNodes <= 0)
            {
                throw new InvalidOperationException("New queue size cannot be smaller than 1");
            }
#endif

            _numNodes = 0;
            _nodes = new T[maxNodes + 1];
            _numNodesEverEnqueued = 0;
        }

        /// <summary>
        /// Returns the number of nodes in the queue
        /// </summary>
        public int Count
        {
            get
            {
                return _numNodes;
            }
        }

        /// <summary>
        /// Returns the maximum number of items that can be enqueud at once in this queue.
        /// Once you his this number (ie. once Count == MaxSize), attempting to enqueue
        /// another item will cause undefined behavior. O(1)
        /// </summary>
        public int MaxSize
        {
            get
            {
                return _nodes.Length - 1;
            }
        }

        /// <summary>
        /// Removes every node from the queue O(n) (So, don't do this often!)
        /// </summary>
        public void Clear()
        {
            Array.Clear(_nodes, 1, _numNodes);
            _numNodes = 0;
        }

        /// <summary>
        /// Returns (in O(1)!) whether the given node is in the queue. O(1)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Contains(T node)
        {
#if DEBUG
            if (node.QueueIndex < 0 || node.QueueIndex >= _nodes.Length)
            {
                throw new InvalidOperationException("node.QueueIndex has been corrupted. " +
                    "Did you change it manually? Or ad this node to another queue?");
            }
#endif
            return (_nodes[node.QueueIndex] == node);
        }


        /// <summary>
        /// Enqueue a node
        /// If the queue is full, the result is undefined.
        /// if the node is already ennqueued, the result is undefined.
        /// O(log n)
        /// </summary>
        /// <param name="node">The node to enqueue</param>
        /// <param name="priority">The node priority</param>
        public void Enqueue(T node, double priority)
        {
#if DEBUG
            if (_numNodes >= _nodes.Length - 1)
            {
                throw new InvalidOperationException("Queue is full - node cannot be added: " + node);
            }

            if (Contains(node))
            {
                throw new InvalidOperationException("Node is already enqueued: " + node);
            }
#endif

            node.Priority = priority;
            _numNodes++;
            _nodes[_numNodes] = node;
            node.QueueIndex = _numNodes;
            node.InsertionIndex = _numNodesEverEnqueued++;
            CascadeUp(_nodes[_numNodes]);
        }

        /// <summary>
        /// Swaps the position of two nodes in the queue.
        /// </summary>
        /// <param name="node1">the first node to swap</param>
        /// <param name="node2">the second node to swap</param>
        private void Swap(T node1, T node2)
        {
            // swap the nodes
            _nodes[node1.QueueIndex] = node2;
            _nodes[node2.QueueIndex] = node1;

            // swap their indicies
            int temp = node1.QueueIndex;
            node1.QueueIndex = node2.QueueIndex;
            node2.QueueIndex = temp;
        }

        /// <summary>
        /// Moves a node up in the queue
        /// </summary>
        /// <param name="node">The node to move up</param>
        private void CascadeUp(T node)
        {
            // aka Heapify-up
            int parent = node.QueueIndex / 2;
            while (parent >= 1)
            {
                T parentNode = _nodes[parent];
                if (HasHigherPriority(parentNode, node))
                {
                    break;
                }

                // Node has lower priority value, so move it up the heap
                Swap(node, parentNode);

                parent = node.QueueIndex / 2;
            }
        }

        /// <summary>
        /// Moves a node down in the queue
        /// </summary>
        /// <param name="node">The node to move down</param>
        private void CascadeDown(T node)
        {
            // aka Heapify-down
            T newParent;
            int finalQueueIndex = node.QueueIndex;

            while (true)
            {
                newParent = node;
                int childLeftIndex = 2 * finalQueueIndex;

                // Check if the left-child is higher-priority than the current node
                if (childLeftIndex > _numNodes)
                {
                    // this could be placed outside the loop, but then we'd have to check newParent != node twice
                    node.QueueIndex = finalQueueIndex;
                    _nodes[finalQueueIndex] = node;
                    break;
                }

                T childLeft = _nodes[childLeftIndex];
                if (HasHigherPriority(childLeft, newParent))
                {
                    newParent = childLeft;
                }

                // check if the right-child is higher-priority than either
                // the current node or the left child
                int childRightIndex = childLeftIndex + 1;
                if (childRightIndex <= _numNodes)
                {
                    T childRight = _nodes[childRightIndex];
                    if (HasHigherPriority(childRight, newParent))
                    {
                        newParent = childRight;
                    }
                }

                // if either of the children has higher (smaller) priority
                // swap and continue cascading
                if (newParent != node)
                {
                    // Move new parent to its index. node will be moved once, at the end
                    // Doing it this way is one les assignment operation than calling Swap()
                    _nodes[finalQueueIndex] = newParent;

                    int temp = newParent.QueueIndex;
                    newParent.QueueIndex = finalQueueIndex;
                    finalQueueIndex = temp;
                }
                else
                {
                    // se notes above
                    node.QueueIndex = finalQueueIndex;
                    _nodes[finalQueueIndex] = node;
                }
            }
        }


        /// <summary>
        /// Returns true if 'higher' has higher priority than 'lower', false otherwise.
        /// Note that calling HasHigherPriority(node, node) (ie. both arguments the same node) will return false
        /// </summary>
        /// <param name="higher">the higher node to check</param>
        /// <param name="lower">the lower node to check</param>
        /// <returns></returns>
        private bool HasHigherPriority(T higher, T lower)
        {
            return (higher.Priority < lower.Priority ||
                (higher.Priority == lower.Priority && higher.InsertionIndex < lower.InsertionIndex));
        }

        /// <summary>
        /// Removes the head of the queue (node with highest priority; ties are broken by
        /// order of insertion), and returns it.
        /// If queue is empty, result is undefiend
        /// O(log n)
        /// </summary>
        public T Dequeue()
        {
#if DEBUG
            if (_numNodes <= 0)
            {
                throw new InvalidOperationException("Cannot call Dequeue() on an empty queue");
            }

            if (!IsValidQueue())
            {
                throw new InvalidOperationException("Queue has been corrupted (Did you update a node priority manually " +
                    "instead of calling UpdatePriority()? Or add the same node to two different queues?");
            }
#endif

            T returnMe = _nodes[1];
            Remove(returnMe);
            return returnMe;
        }

        /// <summary>
        /// Resize the queue so it can accept more nodes. All currently enqueued nodes remain in the queue.
        /// Attempting to decrease the queue size to a size too small to hold the existing ndoes results
        /// in undefined behavior
        /// O(n)
        /// </summary>
        /// <param name="maxNodes">the max number of nodes to resize the queue to</param>
        public void Resize(int maxNodes)
        {
#if DEBUG
            if (maxNodes <= 0)
            {
                throw new InvalidOperationException("Queue size cannot be smaller than 1");
            }

            if (maxNodes < _numNodes)
            {
                throw new InvalidOperationException("Called Resize(" + maxNodes + "), but current queue " +
                    "contains " + _numNodes + " nodes");
            }
#endif
            T[] newArray = new T[maxNodes + 1];
            int highestIndexToCopy = Math.Min(maxNodes, _numNodes);
            for (int i = 1; i <= highestIndexToCopy; i++)
            {
                newArray[i] = _nodes[i];
            }
            _nodes = newArray;
        }

        /// <summary>
        /// Returns the head of the queue, without removing it (use Dequeue() for that).
        /// Returns null if the queue is empty
        /// O(1)
        /// </summary>
        public T First
        {
            get
            {
                return _nodes[1];
            }
        }

        /// <summary>
        /// This method must be called on a node every time its priority changes while it is in the queue.
        /// Forgetting to call this method will result in a corrupted queue!
        /// Calling this method on a node not in the queue results in undefined behavior
        /// O(log n)
        /// </summary>
        /// <param name="node">The node to update</param>
        /// <param name="priority">The new priority of the node</param>
        public void UpdatePriority(T node, double priority)
        {
#if DEBUG
            if (!Contains(node))
            {
                throw new InvalidOperationException("Cannot call UpdatePriority() on a node which is not enqueeud: " + node);
            }
#endif

            node.Priority = priority;
            OnNodeUpdated(node);
        }

        /// <summary>
        /// This method is called when a node is updated
        /// </summary>
        /// <param name="node">the node that was updated</param>
        private void OnNodeUpdated(T node)
        {
            // Bubble the updated node up or down as appropiate
            int parentIndex = node.QueueIndex / 2;
            T parentNode = _nodes[parentIndex];

            if (parentIndex > 0 && HasHigherPriority(node, parentNode))
            {
                CascadeUp(node);
            }
            else
            {
                // note that CasecadeDown will be called if parentNode == node (that is, node is the root)
                CascadeDown(node);
            }
        }

        /// <summary>
        /// Removes a node from the queue. This node does not need to be the head of the queue.
        /// If nod is not in the queue, this result is undefined. If unsure, check Contains() first
        /// O(log n)
        /// </summary>
        /// <param name="node">The node to remove</param>
        public void Remove(T node)
        {
#if DEBUG
            if (!Contains(node))
            {
                throw new InvalidOperationException("Cannot call Remove() on a node which is not enqueued: " + node);
            }
#endif
            if (_numNodes <= 1)
            {
                _nodes[1] = null;
                _numNodes = 0;
                return;
            }

            // Make sure the node is the last node in the queue
            bool wasSwapped = false;
            T formerLastNode = _nodes[_numNodes];
            if (node.QueueIndex != _numNodes)
            {
                // swap the node with the last node
                Swap(node, formerLastNode);
                wasSwapped = true;
            }

            _numNodes--;
            _nodes[node.QueueIndex] = null;

            if (wasSwapped)
            {
                // Now bubble formerLastNode (which is no longer the last node) up or down as appropiate
                OnNodeUpdated(formerLastNode);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 1; i <= _numNodes; i++)
            {
                yield return _nodes[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Should not be called in production code.
        /// Checks to make sure the queue is still in a valid state.
        /// Used for testing/debugging the queue
        /// </summary>
        public bool IsValidQueue()
        {
            for (int i = 1; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    int childLeftIndex = 2 * i;
                    if (childLeftIndex < _nodes.Length && _nodes[childLeftIndex] != null
                        && HasHigherPriority(_nodes[childLeftIndex], _nodes[i]))
                    {
                        return false;
                    }

                    int childRightIndex = childLeftIndex + 1;
                    if (childRightIndex < _nodes.Length && _nodes[childRightIndex] != null
                        && HasHigherPriority(_nodes[childRightIndex], _nodes[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
