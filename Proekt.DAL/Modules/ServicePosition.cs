using Proekt.DAL.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Proekt.DAL.Modules
{

    public class ServicePosition
    {
        string[] Positions = { "Boss", "Clerk", "Manager" };
        private Dictionary<int, string> serializableDictionary = new SerializableDictionary<int, string>();
        private Random rand = new Random();
        private string path { get; set; }
        public ServicePosition(string path)
        {
            if (string.IsNullOrEmpty(path))
                this.path = Path.Combine(@"C:\Users\1\source\repos\Proekt\Proekt\bin\Debug\DictionaryOfPositions.xml");
            else
                this.path = path;
        }

        //public string ReturnRandomPosition()
        //{
        //    return positions[rand.Next(0, positions.Count)].ToString();
        //}

        public void ShowStatistics()
        {
            if (XmlDeserialize().Count == 0)
            {
                Console.WriteLine("Список должностей пуст"); return;
            }
            foreach (KeyValuePair<int, string> item in XmlDeserialize())
            {
                Console.WriteLine(item.Value + " -> " + XmlDeserialize().Where(o => o.Value == item.Value).Count());
            }
        }


        public void ShowPositions()
        {
            if (XmlDeserialize().Count == 0)
            {
                Console.WriteLine("Список должностей пуст"); return;
            }
            Console.WriteLine("Количество должностей: " + XmlDeserialize().Where(o => o.Value != "DELETED").Count() + "\n");
            //  Console.WriteLine("Средняя количество должностей: " + XmlDeserialize().Where(o => o.fullName != "DELETED").Average(o => o.ID) + "\n");
            foreach (KeyValuePair<int, string> item in XmlDeserialize())
            {
                if (item.Key != null && item.Value != "DELETED")
                {
                    Console.WriteLine("ID: {0} | Position: {1}", item.Key, item.Value);
                    Console.WriteLine("------------------------------------------");
                }
            }
        }

        public bool ContaintsPosition(string pos)
        {
            foreach (KeyValuePair<int, string> item in XmlDeserialize())
            {
                if (item.Value == pos)
                    return true;
            }
            return false;
        }
        public bool EditPosition()
        {
            ShowPositions();
            Console.Write("Введите ID должности ->");
            if (SearchPositionByIDForEdit(Int32.Parse(Console.ReadLine())))
                return true;
            else
                return false;
        }

        public bool SearchPositionByIDForEdit(int ID)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            if (xd == null)
                Console.WriteLine("Такого файла нет");
            XmlElement root = xd.DocumentElement;
            Console.WriteLine("Введите новую должность: ");
            string s = Console.ReadLine();
            foreach (XmlElement item in root)
            {
                foreach (XmlElement i in item.ChildNodes)
                {
                    if (ID.ToString().Contains(i.InnerText))
                    {
                        //item.ChildNodes[1].InnerText = s;
                        serializableDictionary[ID] = s;
                        XmlSerialize();
                        return true;
                    }
                }
            }
            return false;
        }

        public void SearchPoisitionByName()
        {
            Console.Write("Введите название должности: ");
            string name = Console.ReadLine();
            Console.WriteLine();
            XmlDocument xd = new XmlDocument();
            xd.Load(path);
            if (xd == null)
                Console.WriteLine("Такого файла нет");
            XmlElement root = xd.DocumentElement;
            if (root == null)
            {
                Console.WriteLine("Сия должность не найдена"); return;
            }
            foreach (XmlElement item in root)
            {
                foreach (XmlElement i in item.ChildNodes)
                {
                    if (name.Contains(i.InnerText))
                    {
                        Console.Write("Сия должность найдена: ");
                        Console.WriteLine(item.InnerText);
                        return;
                    }
                }
            }
            Console.WriteLine("Сия должность не найдена");
        }

        //public void SearchPositionByIDForRemove(int ID)
        //{
        //    ShowPositions();
        //    XmlDocument xd = new XmlDocument();
        //    xd.Load(path);
        //    if (xd == null)
        //        Console.WriteLine("Такого файла нет");
        //    XmlElement root = xd.DocumentElement;
        //    foreach (XmlElement item in root)
        //    {
        //        foreach (XmlElement i in item.ChildNodes)
        //        {
        //            if (ID.ToString().Contains(i.InnerText))
        //            {
        //                //item.ChildNodes[1].InnerText = s;
        //                serializableDictionary.Remove(ID);
        //                Dictionary<int, string> serializableDictionary1 = new SerializableDictionary<int, string>();
        //                //serializableDictionary1 = (SerializableDictionary<int, string>)serializableDictionary.Where(o => o.Value != null);

        //                foreach (KeyValuePair<int, string> keyValue in serializableDictionary)
        //                {
        //                    if(keyValue.Value != null)
        //                    {
        //                        serializableDictionary1.Add(keyValue.Key, keyValue.Value);
        //                    }

        //                }
        //                serializableDictionary = serializableDictionary1;
        //                XmlSerialize();
        //                break;
        //            }
        //        }
        //    }
        //}



        public bool AddPosition()
        {
            int a = rand.Next(1, 10000);
            Console.WriteLine("Введите название должности: ");
            string s = Console.ReadLine();

            if (IsExistPosition(a))
            {
                serializableDictionary = XmlDeserialize();
                serializableDictionary.Add(a, s);
                XmlSerialize();
                return true;
            }
            return false;
        }

        private bool IsExistPosition(int ID)
        {
            if (serializableDictionary.Where(o => o.Key == ID).Count() > 0)
            {
                Console.WriteLine("Сия должность уже есть");
                return false;
            }
            return true;
        }

        private void XmlSerialize()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(SerializableDictionary<int, string>));
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, serializableDictionary);
            }
        }

        private Dictionary<int, string> XmlDeserialize()
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(SerializableDictionary<int, string>));

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    serializableDictionary = (SerializableDictionary<int, string>)formatter.Deserialize(fs);
                }

            }
            catch (Exception ex) { Console.WriteLine(); }
            return serializableDictionary;
        }
    }
}