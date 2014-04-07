using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Common
{
    public class CartItemTableEntity:TableEntity
    {
        public CartItemTableEntity() { }

        public CartItemTableEntity(Guid messageId, Guid cartId, Guid itemId, string name, int quantity)
        {
            this.PartitionKey = cartId.ToString();
            this.RowKey = messageId.ToString();
            Name = name;
            Quanity = quantity;
            ItemId = itemId;
        }

        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public int Quanity { get; set; }
    }
}
