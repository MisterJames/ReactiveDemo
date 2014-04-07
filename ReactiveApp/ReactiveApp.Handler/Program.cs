using System;
using Dapper;
using System.Linq;
using Newtonsoft.Json;
using Reactive.Messages;
using ReactiveApp.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Jobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;

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
            var message = JsonConvert.DeserializeObject<Reactive.Messages.AddItemToCart>(inputText);
            if (message != null)
            {
                PublishEvents(message);

            }
        }
        private static void PublishEvents(AddItemToCart message)
        {
            var itemAddedToCartEvent = new ItemAddedToCart { CartId = message.CartId, ItemId = message.ItemId, ItemName = message.ItemName, MessageId = message.MessageId, Quantity = message.Quantity };

            var serializedItem = JsonConvert.SerializeObject(itemAddedToCartEvent);

            var storageAccount = CloudStorageAccount.Parse(Config.StorageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queueMessage = new CloudQueueMessage(serializedItem);

            var queue = queueClient.GetQueueReference("itemaddedtocartrelational");
            queue.CreateIfNotExists();
            queue.AddMessage(queueMessage);

            queue = queueClient.GetQueueReference("itemaddedtocarttable");
            queue.CreateIfNotExists();
            queue.AddMessage(queueMessage);
        }
        public static void InsertIntoRelational([QueueInput("itemaddedtocartrelational")]string inputText)
        {
                var message = JsonConvert.DeserializeObject<Reactive.Messages.ItemAddedToCart>(inputText);
                if (message != null)
                {
                    using (var connection = new SqlConnection(Config.DefaultConnection))
                    {
                        connection.Open();
                        connection.Execute("insert into cartitems(cartid, itemid, quantity, name) values (@cartId, @itemId, @quantity, @name)", message);
                    }
                }
        }

        public static void InsertIntoTable([QueueInput("itemaddedtocarttable")]string inputText)
        {
            var message = JsonConvert.DeserializeObject<Reactive.Messages.ItemAddedToCart>(inputText);
            if (message != null)
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


}
