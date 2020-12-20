using System;

namespace HonestyBar.Domain
{
    public class Product
    {
        public Product(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }
        public double UnitPrice { get; } 
    }
}
