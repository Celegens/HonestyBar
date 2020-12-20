using System;
using System.Collections.Generic;

namespace HonestyBar.Contracts.Responses
{
    public class EmployeeDto
    {
        public EmployeeDto(Guid id, string firstName, string lastName, string email , List<ProductDto> consumptions = null)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Consumptions = consumptions;
        }

        public Guid Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }
        public List<ProductDto> Consumptions { get; }
    }
}
