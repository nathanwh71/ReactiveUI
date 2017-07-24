using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReativeUI_GridView.Auxiliary;
using ReativeUI_GridView.Service;

namespace ReativeUI_GridView.Model
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IService<Person> _peopleService;
        private IObservable<PersonViewModel> _toDeletedPeople;

        public MainViewModel()
        {
        }

        public MainViewModel(IService<Person> peopleService)
        {
            _peopleService = peopleService;
            
           var disposable = People.ItemsRemoved.SkipWhile(x => !(_peopleService.Get()).Contains(x.Model)).Subscribe( x => Console.WriteLine(x.FullName));
        }

        private ReactiveList<PersonViewModel> _people;
        public ReactiveList<PersonViewModel> People
        {
            get
            {
                if (_people != null) return _people;

                var persons = (_peopleService.Get()).Select(x => new PersonViewModel(x));
                _people = new ReactiveList<PersonViewModel>(persons);
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