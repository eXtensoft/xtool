//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Serialization;

//namespace JunkHarness
//{
//    public static class GenericSerializer
//    {
//        public static void WriteGenericList<T>(List<T> list, string fileName)
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
//            XmlWriter writer;
//            InitializeXmlWriter(fileName, out writer);
//            serializer.Serialize(writer, list);
//            CleanupXmlWriter(ref writer);
//        }

//        public static void WriteGenericItem<T>(T t, string fileName)
//        {
//            XmlSerializer serializer = new XmlSerializer(typeof(T));
//            XmlWriter writer;
//            InitializeXmlWriter(fileName, out writer);
//            serializer.Serialize(writer, t);
//            CleanupXmlWriter(ref writer);
//        }

//        public static void WriteGenericItemBinary<T>(T t, string filePath)
//        {
//            using (FileStream stream = new FileStream(filePath, FileMode.Create))
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                formatter.Serialize(stream, t);
//            }
//        }

//        public static T ReadGenericItemBinary<T>(string filePath)
//        {
//            T t = default(T);
//            using (FileStream stream = new FileStream(filePath, FileMode.Open))
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                t = (T)formatter.Deserialize(stream);
//            }
//            return t;
//        }

//        public static List<T> ReadGenericListBinary<T>(string filePath)
//        {
//            List<T> list = new List<T>();
//            using (FileStream stream = new FileStream(filePath, FileMode.Open))
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                list = (List<T>)formatter.Deserialize(stream);
//            }
//            return list;
//        }

//        public static void WriteGenericListBinary<T>(List<T> list, string filePath)
//        {
//            using (FileStream stream = new FileStream(filePath, FileMode.Create))
//            {
//                BinaryFormatter formatter = new BinaryFormatter();
//                formatter.Serialize(stream, list);
//            }
//        }

//        public static T ReadGenericItem<T>(string fileName)
//        {
//            T t = default(T);
//            if (File.Exists(fileName))
//            {
//                XmlSerializer serializer = new XmlSerializer(typeof(T));
//                TextReader reader = new StreamReader(fileName);
//                t = (T)serializer.Deserialize(reader);
//                reader.Close();
//            }
//            return t;
//        }

//        public static List<T> ReadGenericList<T>(string fileName)
//        {
//            if (File.Exists(fileName))
//            {
//                List<T> l = new List<T>();
//                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
//                TextReader reader = new StreamReader(fileName);
//                l = (List<T>)serializer.Deserialize(reader);
//                reader.Close();
//                return l;
//            }
//            else
//            {
//                return null;
//            }

//        }

//        private static void InitializeXmlWriter(string filename, out XmlWriter writer)
//        {
//            XmlWriterSettings settings;
//            CreateSettings(out settings);
//            writer = XmlWriter.Create(filename, settings);
//        }
//        private static void CleanupXmlWriter(ref XmlWriter writer)
//        {
//            writer.Flush();
//            writer.Close();
//        }
//        private static void CreateSettings(out XmlWriterSettings settings)
//        {

//            settings = new XmlWriterSettings();
//            settings.ConformanceLevel = ConformanceLevel.Document;
//            settings.Indent = true;
//            settings.IndentChars = (" ");
//            settings.CheckCharacters = false;
//            //settings.Encoding = Encoding.BigEndianUnicode;
//        }
//    }
//}
