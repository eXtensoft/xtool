using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace XTool
{
    public sealed partial class GenericSerializer
    {
        public static T Deserialize<T>(byte[] byteArray)
        {
            T t = default(T);
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                t = (T)serializer.ReadObject(stream);
            }
            return t;
        }

        public static byte[] Serialize<T>(T t)
        {
            byte[] byteArray = null;
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, t);
                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        public static byte[] ReadFullStream(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static void WriteGenericList<T>(List<T> list, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            XmlWriter writer;
            InitializeXmlWriter(fileName, out writer);
            serializer.Serialize(writer, list);
            CleanupXmlWriter(ref writer);
        }

        public static void WriteGenericItem<T>(T t, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlWriter writer;
            InitializeXmlWriter(fileName, out writer);
            serializer.Serialize(writer, t);
            CleanupXmlWriter(ref writer);
        }

        public static T ReadGenericItem<T>(string fileName)
        {
            T t = default(T);
            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(fileName);
                t = (T)serializer.Deserialize(reader);
                reader.Close();
            }
            return t;
        }

        public static List<T> ReadGenericList<T>(string fileName)
        {
            if (File.Exists(fileName))
            {
                List<T> l = new List<T>();
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                TextReader reader = new StreamReader(fileName);
                l = (List<T>)serializer.Deserialize(reader);
                reader.Close();
                return l;
            }
            else
            {
                return null;
            }

        }

        public static List<T> CreateGenericListFromXmlDoc<T>(XmlDocument xml)
        {
            List<T> l = new List<T>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (MemoryStream stream = new MemoryStream())
            {
                xml.Save(stream);
                stream.Position = 0;
                l = (List<T>)serializer.Deserialize(stream);
            }
            return l;
        }

        public static T CreateGenericItemFromXmlDoc<T>(XmlDocument xml)
        {
            T t = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                xml.Save(stream);
                stream.Position = 0;
                t = (T)serializer.Deserialize(stream);
            }
            return t;
        }

        public static XmlDocument XmlDocFromItem(object o)
        {
            Type t = o.GetType();
            XmlDocument xml = new XmlDocument();


            xml.PreserveWhitespace = false;
            XmlSerializer serializer = new XmlSerializer(t);
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (StreamReader reader = new StreamReader(stream))
            {
                serializer.Serialize(writer, o);
                stream.Position = 0;
                xml.Load(stream);
            }
            return xml;
        }

        public static T CloneItem<T>(T item)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc = XmlDocFromGenericItem<T>(item);
            T t = CreateGenericItemFromXmlDoc<T>(xdoc);
            return t;
        }

        public static T Clone<T>(T item)
        {
            T t;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, item);
                stream.Position = 0;
                t = (T)formatter.Deserialize(stream);
            }
            return t;
        }

        public static XmlDocument XmlDocFromGenericItem<T>(T t, bool preserveWhitespace)
        {
            XmlDocument xml = new XmlDocument();
            xml.PreserveWhitespace = preserveWhitespace;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (StreamReader reader = new StreamReader(stream))
            {
                serializer.Serialize(writer, t);
                stream.Position = 0;
                xml.Load(stream);
            }
            return xml;
        }

        public static XmlDocument XmlDocFromGenericItem<T>(T t)
        {
            return XmlDocFromGenericItem<T>(t, false);
        }

        public static byte[] ItemToByteArray(object o)
        {
            byte[] array = null;
            if (o != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, o);
                    array = stream.ToArray();
                }
            }
            return array;
        }

        public static byte[] GenericItemToByteArray<T>(T t)
        {
            byte[] array = null;
            if (t != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, t);
                    array = stream.ToArray();
                }
            }
            return array;
        }

        public static object ByteArrayToItem(byte[] bytes)
        {
            object o = null;
            if (bytes != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    o = (object)formatter.Deserialize(stream);
                }
            }

            return o;
        }

        public static T ByteArrayToGenericItem<T>(byte[] bytes)
        {
            T t = default(T);
            if (bytes != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    object o = (object)formatter.Deserialize(stream);
                    if (o is T)
                    {
                        t = (T)o;
                    }
                }
            }

            return t;
        }

        public static XmlDocument XmlDocFromGenericList<T>(List<T> list)
        {
            XmlDocument xml = new XmlDocument();
            xml.PreserveWhitespace = false;
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            using (MemoryStream stream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(stream))
            using (StreamReader reader = new StreamReader(stream))
            {
                serializer.Serialize(writer, list);
                stream.Position = 0;
                xml.Load(stream);
            }
            return xml;
        }

        public static List<T> StringToGenericList<T>(string s)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(s);
            return CreateGenericListFromXmlDoc<T>(xdoc);
        }
        public static T StringToGenericItem<T>(string s)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(s);
            return CreateGenericItemFromXmlDoc<T>(xdoc);

        }


        #region helper methods

        private static void InitializeXmlWriter(string filename, out XmlWriter writer)
        {
            XmlWriterSettings settings;
            CreateSettings(out settings);
            writer = XmlWriter.Create(filename, settings);
        }
        private static void CleanupXmlWriter(ref XmlWriter writer)
        {
            writer.Flush();
            writer.Close();
        }
        private static void CreateSettings(out XmlWriterSettings settings)
        {

            settings = new XmlWriterSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.Indent = true;
            settings.IndentChars = (" ");
            settings.CheckCharacters = false;
            //settings.Encoding = Encoding.BigEndianUnicode;
        }

        #endregion


    }
}

