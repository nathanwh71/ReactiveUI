using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Reactive.Concurrency;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace ReativeUI_GridView.Utilities
{
    public class TrackableReactiveList<T> : ReactiveList<T>, IReactiveObject where T : IStatus
    {
        private HashSet<T> _toInsert = new HashSet<T>();
        private HashSet<T> _toUpdate = new HashSet<T>();
        private HashSet<T> _toDelete = new HashSet<T>();

        public TrackableReactiveList() : base()
        {
            Initialize();
        }

        public TrackableReactiveList(IEnumerable<T> initialContents) : base(initialContents)
        {
            Initialize();
        }

        public TrackableReactiveList(IEnumerable<T> initialContents = null, double resetChangeThreshold = 0.3, IScheduler scheduler = null) : base(initialContents, resetChangeThreshold, scheduler)
        {
            Initialize();
        }

        private void Initialize()
        {
            ItemsAdded.Subscribe(x =>
            {
                _toInsert.Add(x);
                //Dump();
            });
            ItemsRemoved.Subscribe(x =>
            {
                if (x.IsNew)
                {
                    _toInsert.Remove(x);
                    //Dump();
                }
                else
                {
                    _toUpdate.Remove(x);
                    _toDelete.Add(x);
                   // Dump();
                }
            });
            ItemChanged.Subscribe(x =>
            {
                if (!x.Sender.IsNew)
                {
                    _toUpdate.Add(x.Sender);
                   // Dump();
                }
            });
        }

        public List<T> ToInsert => _toInsert.ToList();
        public List<T> ToUpdate => _toUpdate.ToList();
        public List<T> ToDelete => _toDelete.ToList();

   
    }
}