using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReactiveApp.Web.Models
{
    public class AddItemModel
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

    }
}