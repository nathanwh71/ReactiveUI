using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using ReativeUI_GridView.Model;
using ReativeUI_GridView.Service;
using ReativeUI_GridView.Utilities;

namespace ReativeUI_GridView.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IService<Person> _peopleService;
     /*   private HashSet<Person> _toAdded = new HashSet<Person>();
        private HashSet<Person> _toUpdate = new HashSet<Person>();
        private HashSet<Person> _toDeleted = new HashSet<Person>();
     */
        public MainViewModel() {}

        public MainViewModel(IService<Person> peopleService)
        {
            _peopleService = peopleService;
            People.ItemsAdded.Subscribe(x => Dump());
            People.ItemChanged.Subscribe(x => Dump());
            People.ItemsRemoved.Subscribe(x => Dump());
            /* var addedStream = People.ItemsAdded.Subscribe(x =>
             {
                 _toAdded.Add(x.Model);
                 Dump();
             });
 
             var deletedStream = People.ItemsRemoved.Subscribe(x =>
             {
                 if (x.IsNew) _toAdded.Remove(x.Model);
                 else
                 {
                     _toUpdate.Remove(x.Model);
                     _toDeleted.Add(x.Model);
                 }
                 Dump();
             });
 
             var modifiedStream = People.ItemChanged.Subscribe(x =>
             {
                if (!x.Sender.IsNew) _toUpdate.Add(x.Sender.Model);
                 Dump();
             });*/
        }

       /* private ReactiveList<PersonViewModel> _people;
        public ReactiveList<PersonViewModel> People
        {
            get
            {
                if (_people != null) return _people;

                var persons = (_peopleService.Get()).Select(x => new PersonViewModel(x) { IsNew = false });
                _people = new ReactiveList<PersonViewModel>(persons) { ChangeTrackingEnabled = true};
                return _people;
            }
            set
            {
                _people = value;
                this.RaiseAndSetIfChanged(ref _people, value);
            }
        }*/

        private TrackableReactiveList<PersonViewModel> _people;

        public TrackableReactiveList<PersonViewModel> People
        {
            get
            {
                if (_people != null) return _people;

                var persons = (_peopleService.Get()).Select(x => new PersonViewModel(x) { IsNew = false });
                _people = new TrackableReactiveList<PersonViewModel>(persons) { ChangeTrackingEnabled = true };
                return _people;
            }
            set
            {
                _people = value;
                this.RaiseAndSetIfChanged(ref _people, value);
            }

        }


        private void Dump()
        {
            var toInsert = JValue.Parse(JsonConvert.SerializeObject(People.ToInsert.Select(x => x.Model))).ToString(Formatting.Indented);
            var toUpdate = JValue.Parse(JsonConvert.SerializeObject(People.ToUpdate.Select(x => x.Model))).ToString(Formatting.Indented);
            var toDelete = JValue.Parse(JsonConvert.SerializeObject(People.ToDelete.Select(x => x.Model))).ToString(Formatting.Indented);

            Console.WriteLine(@" ");
            Console.WriteLine(@"-----------------------------------");
            Console.WriteLine(@"Insert: ");
            Console.WriteLine(toInsert);
            Console.WriteLine(@"Update: ");
            Console.WriteLine(toUpdate);
            Console.WriteLine(@"Delete: ");
            Console.WriteLine(toDelete);
            Console.WriteLine(@"-----------------------------------");
            Console.WriteLine(@" ");

        }
    }
}