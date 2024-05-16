using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wakeDomain.Domain.Models.Global;

namespace wakeDomain.Domain.Models
{
    public class Product : BaseModel
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Inventory { get; set; }

    }
}
