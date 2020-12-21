using System;
using System.Collections.Generic;
using System.Text;

namespace HonestyBar.Domain
{
 public   class Consumption
    {
        public Consumption()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.Now; 
        }
        public Consumption(Product  p)
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.Now;
            Product = p;
        }
        public virtual Product Product { get; set; }
     
        public  Guid Id { get; }
        public  DateTime DateTime { get; }
    }
}
