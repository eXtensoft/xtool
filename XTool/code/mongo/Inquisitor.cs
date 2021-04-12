// <copyright file="Inquisitor.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool.Mongo
{
    using MongoDB.Bson;
    using MongoDB.Bson.IO;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web.Script.Serialization;



    public static class Inquisitor
    {
        public static string Execute(string connectionString, string databaseName, string collectionName, string command, string jsonQuery)
        {
            //string[] t = command.Split(new char[] { '.' });
            string json = String.Empty;
            var client = new MongoClient(connectionString);
            //var server = client.();
            //var db = server.GetDatabase(databaseName);
            //var collection = db.GetCollection(collectionName);

            if (!String.IsNullOrWhiteSpace(jsonQuery))
            {
                try
                {
                    var bson = BsonSerializer.Deserialize<BsonDocument>(jsonQuery);
                    //var query = new QueryDocument(bson);
                    JsonWriterSettings settings = new JsonWriterSettings();
                    settings.Indent = true;
                    settings.OutputMode = JsonOutputMode.Strict;
                    //json = collection.Find(query).ToJson(settings);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            return json;
        }

        //public static dynamic Execute(string connectionString, string databaseName, string collectionName, string command, string jsonQuery)
        //{
        //    //string[] t = command.Split(new char[] { '.' });
        //    string json = String.Empty;
        //    var client = new MongoClient(connectionString);
        //    var server = client.GetServer();
        //    var db = server.GetDatabase(databaseName);
        //    var collection = db.GetCollection(collectionName);

        //    if (!String.IsNullOrWhiteSpace(jsonQuery))
        //    {
        //        try
        //        {
        //            var bson = BsonSerializer.Deserialize<BsonDocument>(jsonQuery);
        //            var query = new QueryDocument(bson);
        //            JsonWriterSettings settings = new JsonWriterSettings();
        //            settings.Indent = true;
        //            settings.OutputMode = JsonOutputMode.Strict;
        //            json = collection.Find(query).ToJson(settings);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }
        //    }
        //    //using System.Web.Script.Serialization;
        //    JavaScriptSerializer jss = new JavaScriptSerializer();
        //    var d=jss.Deserialize<dynamic>(json);
        //    List<dynamic> list = new List<dynamic>();
        //    for (int i = 0; i < d.Length; i++)
        //    {
        //        list.Add(d[i]);
        //    }
        //    //System.Data.DataTable dt = list.ToDataTable();
        //    return d;
        //}

    }



    public enum CommandType
    {
        find,
        count,
        aggregate,
        group,
    }
}
