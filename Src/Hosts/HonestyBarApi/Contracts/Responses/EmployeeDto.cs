using System;
using System.Collections.Generic;

namespace HonestyBar.Contracts.Responses
{
    public class EmployeeDto
    {
        public EmployeeDto(Guid id, string firstName, string lastName, string email,double saldo = 0d, List<ProductDto> consumptions = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Consumptions = consumptions;
            Saldo = saldo;
        }

        public Guid Id { get; }

        public string FirstName { get; }
        public double Saldo{ get; }

        public string LastName { get; }

        public string Email { get; }
        public List<ProductDto> Consumptions { get; }
    }
}
