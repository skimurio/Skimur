using System;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public class SubWrapped
    {
        private int? _fuzzedSubscribers;

        public SubWrapped(Sub sub)
        {
            Sub = sub;
        }

        public Sub Sub { get; private set; }

        public bool IsSubscribed { get; set; }

        public int Subscribers
        {
            get
            {
                if (_fuzzedSubscribers.HasValue)
                    return _fuzzedSubscribers.Value;
                return Sub.Subscribers;
            }
        }

        public void FuzzSubscribers(int fuzzed)
        {
            if (_fuzzedSubscribers.HasValue)
                return;

            _fuzzedSubscribers = fuzzed;
        }

        public bool IsSubscribersFuzzed
        {
            get { return _fuzzedSubscribers.HasValue; }
        }
    }
}
