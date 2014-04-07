using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactive.Messages
{
    public class AddItemToCart
    {
        public Guid MessageId { get; set; }
        public Guid ItemId { get; set; }
        public Guid CartId { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
    }
}
