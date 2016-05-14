using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //连接信息
            string conn = "mongodb://10.202.101.43:27017";
            var client = new MongoClient(conn);
            var database = client.GetDatabase("HelloMongo");
            var collection = database.GetCollection<BsonDocument>("Order");

            //1.Init query
            Console.WriteLine("---1.query from mongdb");
            OutputAll(collection);

            //2. insert one and query all
            Console.WriteLine("==press enter,insert one data  enter");
            Console.ReadLine();
            var document = new BsonDocument {
                { "name", "xiaomiao" },
                { "sex", "male" }
            };
            collection.InsertOne(document);
            Console.WriteLine("---2.query after insert");
            OutputAll(collection);

            //3. insert many and query all
            Console.WriteLine("==input count,like:2,will insert 2 row data");
            string insertmanys = Console.ReadLine();
            var documents = Enumerable.Range(0, 2).Select(i => new BsonDocument("counter", i));
            collection.InsertMany(documents);
            Console.WriteLine("---3.query after insert 2 row data");
            OutputAll(collection);

            //4. update and query all
            Console.WriteLine("==press enter,update xiaomiao's info");
            Console.ReadLine();
            var filterUpdate = Builders<BsonDocument>.Filter.Eq("name", "xiaomiao");
            var update = Builders<BsonDocument>.Update.Set("sex ", "x-male");
            collection.UpdateOne(filterUpdate, update);
            Console.WriteLine("---4.query after update data");
            OutputAll(collection);

            //5. delete one and query all
            Console.WriteLine("==input counter you want to delete,like 1");
            string deleteCounter = Console.ReadLine();
            var filter = Builders<BsonDocument>.Filter.Eq("counter", Convert.ToInt32(deleteCounter));
            collection.DeleteOne(filter);
            Console.WriteLine("---5.query after delete data");
            OutputAll(collection);



            Console.ReadLine();


        }

        private static void OutputAll(IMongoCollection<BsonDocument> collection)
        {
            var cursor = collection.Find(new BsonDocument()).ToCursor();
            foreach (var document in cursor.ToEnumerable())
            {
                Console.WriteLine(document);
            }
        }

    }
}
