using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive.Messages
{
    public class AddItemToCart
    {
        public Guid ItemId { get; set; }
        public Guid CartId { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
