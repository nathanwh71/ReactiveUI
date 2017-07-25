using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReativeUI_GridView.Model;
using ReativeUI_GridView.Service;

namespace ReativeUI_GridView.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IService<Person> _peopleService;
        private List<PersonViewModel> _toAdded = new List<PersonViewModel>();
        private List<PersonViewModel> _toUpdate = new List<PersonViewModel>();
        private List<PersonViewModel> _toDeleted = new List<PersonViewModel>();
     
        public MainViewModel() {}

        public MainViewModel(IService<Person> peopleService)
        {
            _peopleService = peopleService;

            var addedStream = People.ItemsAdded.Subscribe(x =>
            {
                _toAdded.Add(x);
                Console.WriteLine($@"Added {x.FullName} {x.Uuid}");
            });
               
            var deletedStream = People.ItemsRemoved.Subscribe(x => Console.WriteLine($@"Deleted {x.FullName}"));
            var modifiedStream = People.ItemChanged.Subscribe(x =>
            {
                var modifiedPerson = x.Sender;
                if (!modifiedPerson.IsNew)
                {
                    _toUpdate.FindIndex()
                }


            });

           


        }

        private ReactiveList<PersonViewModel> _people;
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
        }
    }
}