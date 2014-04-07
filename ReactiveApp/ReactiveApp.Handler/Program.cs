using Microsoft.WindowsAzure.Jobs;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using ReactiveApp.Common;

namespace ReactiveApp.Handler
{
    class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();
            host.RunAndBlock();
        }

        public static void ProcessQueueMessage([QueueInput("additemtocartqueue")] string inputText)
        {
            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<Reactive.Messages.AddItemToCart>(inputText);
            if (message != null)
            {
                InsertIntoRelational(message);
                InsertIntoTable(message);
            }
        }
        private static void InsertIntoRelational(Reactive.Messages.AddItemToCart message)
        {
            using (var connection = new SqlConnection(Config.DefaultConnection))
            {
                connection.Open();
                connection.Execute("insert into cartitems(cartid, itemid, quantity, name) values (@cartId, @itemId, @quantity, @name)", message);
            }
        }

        private static void InsertIntoTable(Reactive.Messages.AddItemToCart message)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Config.StorageConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("cartitems");
            table.CreateIfNotExists();

            var insertOperation = TableOperation.Insert(new CartItemTableEntity(message.MessageId, message.CartId, message.ItemId, message.ItemName, message.Quantity));

            table.Execute(insertOperation);
        }
    }


}
