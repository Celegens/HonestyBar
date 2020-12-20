using System;

namespace HonestyBar.Domain
{
    public class Employee
    {
        public Employee(string firstName, string lastName, string email)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public Guid Id { get; }
        
        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }
    }
}
