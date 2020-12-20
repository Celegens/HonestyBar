using System;

namespace HonestyBar.Contracts.Responses
{
    public class ProductDto
    {
        public ProductDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }
    }
}
