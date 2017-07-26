using ReactiveUI;
using ReativeUI_GridView.Model;
using ReativeUI_GridView.Utilities;

namespace ReativeUI_GridView.ViewModel
{
    public class PersonViewModel : ReactiveObject, IStatus
    {
        private Person _person;

        public PersonViewModel():this(new Person()) {}

        public PersonViewModel(Person person)
        {
            _person = person;
        }

        public Person Model { get { return _person;  } }

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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() != this.GetType();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


    }
}