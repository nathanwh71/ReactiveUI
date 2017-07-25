using ReactiveUI;
using ReativeUI_GridView.Model;

namespace ReativeUI_GridView.ViewModel
{
    public class PersonViewModel : ReactiveObject
    {
        private Person _person;

        public PersonViewModel():this(new Person()) {}

        public PersonViewModel(Person person)
        {
            _person = person;
        }

        public Person Model { get { return _person;  } }


        public string Uuid { get; set; } = System.Guid.NewGuid().ToString();

        public string FullName { get { return $"{_person.FirstName} {_person.LastName}"; } }

        public string FirstName
        {
            get { return _person.FirstName; }
            set
            {
                _person.FirstName = value;
                this.RaisePropertyChanged();
            }
        }

        public string LastName
        {
            get { return _person.LastName; }
            set
            {
                _person.LastName = value;
                this.RaisePropertyChanged();
            }
        }

        public int Age
        {
            get { return _person.Age; }
            set
            {
                _person.Age = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsNew { get; set; } = true;


    }
}