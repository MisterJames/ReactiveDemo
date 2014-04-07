using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices;
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
using Microsoft.WindowsAzure.Storage.Table;
using ReactiveApp.Common;

namespace ReactiveApp.Web.Controllers
{
    public class CartController : ApiController
    {

        public List<AddItemModel> Get()
        {
            // return all items in cart
            if (MvcApplication.DbServiceState == DbServiceState.RealTime)
                return GetCartFromRelational();
            else
                return GetCartFromTable();

        }

        private List<AddItemModel> GetCartFromTable()
        {
            var cartId = Guid.Parse((string)HttpContext.Current.Session[MvcApplication.SessionCartIdKey]);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Config.StorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("cartitems");
            var query = new TableQuery<CartItemTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, cartId.ToString()));

            return table.ExecuteQuery(query).Select(x => new AddItemModel { ItemId = x.ItemId, Name = x.Name, Quantity = x.Quanity }).ToList();
            
        }

        private List<AddItemModel> GetCartFromRelational()
        {
            var result = new List<AddItemModel>();
            var cartId = (string)HttpContext.Current.Session[MvcApplication.SessionCartIdKey]; 
            
            using (var connection = new SqlConnection(Config.DefaultConnection))
            {
                connection.Open();

                var cmd = new SqlCommand("select itemId, [name], quantity from cartitems where cartid = @cartId ", connection);
                cmd.Parameters.AddWithValue("cartId", cartId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new AddItemModel
                        {
                            ItemId = reader.GetGuid(0),
                            Name = reader.GetString(1),
                            Quantity = reader.GetInt32(2)
                        });
                    }
                }

                connection.Close();
            }

            return result;
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
