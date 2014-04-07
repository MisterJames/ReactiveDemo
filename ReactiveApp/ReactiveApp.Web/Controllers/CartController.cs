using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.ModelBinding;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Reactive.Messages;
using ReactiveApp.Web.Models;

namespace ReactiveApp.Web.Controllers
{
    public class CartController : ApiController
    {

        public List<string> Get()
        {
            // return all items in cart
            throw  new NotImplementedException();
        }

        public HttpResponseMessage Post(AddItemModel item)
        {
            var cartId = Guid.Parse((string)HttpContext.Current.Session[MvcApplication.SessionCartIdKey]);

            // add item to cart
            var itemToAdd = new AddItemToCart
            {
                MessageId = Guid.NewGuid(),
                CartId = cartId,
                ItemId = item.ItemId,
                ItemName = item.Name,
                MessageId = Guid.NewGuid(),
                Quantity = item.Quantity
            };

            try
            {
                var serializedItem = JsonConvert.SerializeObject(itemToAdd);

                var storageAccount = CloudStorageAccount.Parse(Config.StorageConnectionString);
                var queueClient = storageAccount.CreateCloudQueueClient();
                var queue = queueClient.GetQueueReference("additemtocartqueue");
                queue.CreateIfNotExists();
                var message = new CloudQueueMessage(serializedItem);

                queue.AddMessage(message);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch 
            {
                // do something interesting here...
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

    }
}
