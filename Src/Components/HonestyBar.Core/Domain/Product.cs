using Newtonsoft.Json;
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
        public Product(string name , double unitprice = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            UnitPrice = unitprice;
        }
        public Guid Id { get; }

        public string Name { get; }
        public double UnitPrice { get; } 
    }
}
