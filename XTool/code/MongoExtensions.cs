// <copyright file="MongoExtensions.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

//namespace XTool.code
//{
//    using MongoDB.Bson;
//    using MongoDB.Bson.Serialization;
//    using MongoDB.Driver;
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Runtime.Serialization;



//    public sealed class MongoExtensions
//    {
//        public static List<T> GetItems<T>(this MongoCollection collection,
//                       string queryString, string orderString) where T : class
//        {
//            var queryDoc = BsonSerializer.Deserialize<BsonDocument>(queryString);
//            var orderDoc = BsonSerializer.Deserialize<BsonDocument>(orderString);

//            //as of version 1.8 you should use MongoDB.Driver.QueryDocument instead (thanks to @Erik Hunter)
//            var query = new QueryComplete(queryDoc);
//            var order = new SortByWrapper(orderDoc);

//            var cursor = collection.FindAs<T>(query);
//            cursor.SetSortOrder(order);

//            return cursor.ToList();
//        }
//    }
//}

//BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>("{ name : value }");
//        QueryDocument queryDoc = new QueryDocument(document);
//        MongoCursor toReturn = collection.Find(queryDoc);