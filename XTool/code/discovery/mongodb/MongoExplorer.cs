// <copyright file="MongoExplorer.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;


    [Serializable]
    [DataContract(Namespace = "http://eXtensibleSolutions/schemas/2016/04")]
    public sealed class MongoExplorer
    {
        public List<Database> Databases { get; set; }

        public string ConnectionString { get; set; }


        public MongoExplorer(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Discover()
        {
            Databases = new List<Database>();

            var client = new MongoClient(ConnectionString);
            

            foreach (var dbName in client.ListDatabaseNames().ToList())
            {
                //MongoDatabase mdb = server.GetDatabase(dbName);
                var mdb = client.GetDatabase(dbName);
 
                Database db = new Database()
                {
                    Name = dbName,
                    Collections = new List<Collection>()
                };

                JsonWriterSettings settings = new JsonWriterSettings();
                settings.Indent = true;
                if (true)
                {
                    foreach (var collectionName in mdb.ListCollectionNames().ToList())
                    {

                        var mc = mdb.GetCollection<BsonDocument>(collectionName);
                        Collection c = new Collection();
                        //var filter = new Builder<BsonDocument>.Filter
                        //var found = mc.Find(null).Limit(1);
                        //var ss = found.ToList();
                        //if (ss.Count >= 1)
                        //{
                        //    c.Json = ss[0].ToJson(settings);
                        //}
                    
                        c.Name = collectionName;
                        //c.FullName = mc.FullName;
                        //var stats = mc.GetStats();
                        
                        //c.AvgDocumentSize = stats.AverageObjectSize;
                        //c.DataSize = stats.DataSize;
                        //c.IndexCount = stats.IndexCount;
                        //c.DocumentCount = stats.ObjectCount;
                        c.Indexes = new List<Index>();
                        //foreach (var idx in mc.Indexes())
                        //{
                        //    c.Indexes.Add(new Index() 
                        //    { 
                        //        Key = idx.Key.ToString(), 
                        //        Name = idx.Name 
                        //    });
                        //}
                        db.Collections.Add(c);

                    }
                }


                Databases.Add(db);

            }

        }





    }
}
