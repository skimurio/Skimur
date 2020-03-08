using System.Collections.Generic;

namespace Skimur.Common.Utils
{
    /// <summary>
    /// A seeked list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class SeekedList<T> : List<T>
    {
        public int Skipped { get; private set; }

        public int? Taken { get; private set; }

        public long TotalCount { get; private set; }

        public bool HasMore { get { return TotalCount > (Skipped + Taken);  } }

        public bool HasPrevious { get { return Skipped > 0; } }

        public SeekedList(IEnumerable<T> source, int skipped, int? taken, long totalCount)
        {
            Skipped = skipped;
            Taken = taken;
            TotalCount = totalCount;
            AddRange(source);
        }

        public SeekedList() { }

    }
}
