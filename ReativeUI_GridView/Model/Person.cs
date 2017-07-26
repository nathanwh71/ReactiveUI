namespace ReativeUI_GridView.Model
{
    public class Person
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Person() : this(string.Empty, string.Empty, 0) {}
    
        public Person(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
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