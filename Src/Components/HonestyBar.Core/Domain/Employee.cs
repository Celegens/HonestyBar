using System;
using System.Collections;
using System.Collections.Generic;

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
            Consumptions =  new List<Consumption>(); 
            Active = true;
            Saldo = 0.0D;
        }

        public Guid Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }
        public bool Active { get; set; }
        public double Saldo { get; set; }

        public virtual ICollection<Consumption> Consumptions { get; set; }
    }
}
