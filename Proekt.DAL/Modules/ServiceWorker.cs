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
    [Serializable]
    public class ServiceWorker
    {
        Logs logs = new Logs();
        private Random rand = new Random();

        private List<Worker> workers = new List<Worker>();

        private ServicePosition servicePosition = new ServicePosition(null);

        private string path { get; set; }

        public ServiceWorker(string path)
        {
            {
                if (string.IsNullOrEmpty(path))
                    this.path = Path.Combine(@"C:\Users\1\source\repos\Proekt\Proekt\bin\Debug\Workers");
                else
                    this.path = path;
            }
        }

        public void ShowStatistics()
        {
            Console.WriteLine("Количество сотрудников: " + XmlDeserialize().Where(o => o.fullName != "DELETED").Count() + "\n");
            Console.WriteLine("Средняя зарплата сотрудников: " + XmlDeserialize().Where(o => o.fullName != "DELETED").Average(o => o.ID) + "\n");
        }

        public void ShowWorkers()
        {

            if (XmlDeserialize().Count == 0)
            {
                Console.WriteLine("Список сотрудников пуст"); return;
            }
            foreach (Worker item in workers)
            {
                if (item.fullName != null && item.position != null)
                {
                    Console.WriteLine("ID: {0} | Full name: {1} | Position: {2}", item.ID, item.fullName, item.position);
                    Console.WriteLine("------------------------------------------");
                }
            }
        }

        public bool AddWorker()
        {
            Worker w = new Worker();

            w.ID = rand.Next(1, 10000);

            Console.Write("Введите имя нового сотрудника: ");
            string name = Console.ReadLine();
            Console.WriteLine();

            servicePosition.ShowPositions();

            Console.WriteLine();
            Console.Write("Введите должность нового сотрудника: ");
            string pos = Console.ReadLine();
            Console.WriteLine();

            if (servicePosition.ContaintsPosition(pos))
            {
                w.fullName = name;
                w.position = pos;
            }
            else
            {
                Console.WriteLine("Такой должности нет в списке."); return false;
            }
            if (IsExistWorker(w))
            {
                workers.Add(w);
                XmlSerialize(w);
                XmlSerialize();
                return true;
            }
            return false;
        }

        public bool EditWorker()
        {
            ShowWorkers(); Console.ReadKey();
            Console.Write("Введите ID сотрудника ->");
            if (SearchWorkerByIDForEdit(Int32.Parse(Console.ReadLine())))
                return true;
            else
                return false;
        }

        private XmlElement Edit(XmlElement w)
        {
            int q = 0;
            foreach (XmlElement item in w.ChildNodes)
            {
                if (q > 0)
                {
                    Console.Write(item.Name + ": (" + item.InnerText + ") - ");
                    string s = Console.ReadLine();
                    if (item.Name == "position")
                    {
                        if (!servicePosition.ContaintsPosition(s))
                        {
                            return null;
                        }
                    }
                    Console.WriteLine();
                    if (!string.IsNullOrEmpty(s))
                    {
                        item.InnerText = s;
                    }
                    else return null;
                }
                ++q;
            }
            return w;
        }

        public void SearchWorkerByName()
        {
            Console.Write("Введите имя сотрудника:");
            string name = Console.ReadLine();
            Console.WriteLine();
            XmlDocument xd = GetDocument(name);

            if (xd == null)
                Console.WriteLine("Такого файла нет");

            XmlElement root = xd.DocumentElement;

            if (root == null)
            {
                Console.WriteLine("Сей сотрудник не найден"); return;
            }
            foreach (XmlElement item in root)
            {
                if (item.Name == "fullName" && item.InnerText == name)
                {
                    Console.Write("Сей сотрудник найден:");
                    foreach (XmlElement i in root)
                    {
                        Console.Write(" | " + i.InnerText + " | ");
                    }
                    Console.WriteLine();
                    return;
                }
            }
        }

        private bool SearchWorkerByIDForEdit(int ID)
        {
            XmlDocument xd = GetDocument(ID);
            XmlDocument xdW = new XmlDocument();
            xdW.Load("Workers.xml");
            if (xd == null || xdW == null)
            {
                Console.WriteLine("Такого файла нет");
                return false;
            }
            XmlElement xeW = null;
            XmlElement root = xd.DocumentElement;
            XmlElement rootW = xdW.DocumentElement;
            string s = "";

            foreach (XmlElement item in rootW)
            {
                foreach (XmlElement i in item)
                {
                    if (i.Name == "ID" && i.InnerText == ID.ToString())
                    {
                        xeW = Edit(item);
                        if (xeW != null)
                        {
                            xdW.Save("Workers.xml");
                            return true;
                        }
                        else return false;
                    }
                }
            }
            foreach (XmlElement item in root)
            {
                if (item.Name == "ID" && item.InnerText == ID.ToString())
                {
                    s = item.InnerText;
                    XmlElement xe = xeW;
                    xd.Save(path + @"\" + s + ".xml");
                }
            }
            return false;
        }

        public bool RemoveWorker()
        {
            ShowWorkers(); Console.ReadKey();
            Console.Write("Введите ID работника ->");
            if (SearchWorkerByIDForRemove(Int32.Parse(Console.ReadLine())))
                return true;
            return false;
        }

        private bool SearchWorkerByIDForRemove(int ID)
        {
            XmlDocument xd = GetDocument(ID);
            if (xd == null)
            {
                Console.WriteLine("Такого файла нет");
                return false;
            }
            string s = "";

            XmlElement root = xd.DocumentElement;

            foreach (XmlElement item in root)
            {
                if (item.Name == "ID" && item.InnerText == ID.ToString())
                {
                    s = item.InnerText;
                    foreach (XmlElement i in root)
                    {
                        i.InnerText = null;
                    }
                    xd.Save(path + @"\" + s + ".xml");

                    XmlDeserialize();
                    //workers.Remove((Worker)workers.Where(o => o.ID == ID));
                    List<Worker> workers1 = new List<Worker>();
                    foreach (Worker w in workers)
                    {
                        if (w.ID != ID)
                            workers1.Add(w);
                    }
                    workers = workers1;
                    XmlSerialize();
                    return true;
                }
            }
            return false;
        }

        private bool IsExistWorker(Worker w)
        {
            if (workers.Where(o => o.ID == w.ID).Count() > 0)
            {
                Console.WriteLine("Такой работник уже есть");
                return false;
            }
            return true;
        }


        private void XmlSerialize(Worker w)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Worker));

            using (FileStream fs = new FileStream(path + @"\" + w.ID + ".xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, w);
            }
        }

        private void XmlSerialize()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Worker>));

            using (FileStream fs = new FileStream("Workers.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, workers);
            }
        }

        private void XmlSerialize(List<Worker> workers)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Worker>));

            using (FileStream fs = new FileStream("Workers.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, workers);
            }
        }


        public List<Worker> XmlDeserialize()
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Worker>));
                using (FileStream fs = new FileStream("Workers.xml", FileMode.OpenOrCreate))
                {
                    workers = ((List<Worker>)formatter.Deserialize(fs));
                }
            }
            catch (Exception ex) { Console.WriteLine(); logs.WriteLogs(ex); }
            return workers;

            //List<Worker> w = new List<Worker>();

            //XmlSerializer formatter = new XmlSerializer(typeof(List<Worker>));

            //DirectoryInfo directory = new DirectoryInfo(path);

            //FileInfo[] fi = directory.GetFiles();

            //foreach (FileInfo item in fi)
            //{
            //    using (FileStream fs = new FileStream(item.FullName, FileMode.OpenOrCreate))
            //    {
            //        w.Add((Worker)formatter.Deserialize(fs));
            //    }
            //}
            //return w;
        }

        private XmlDocument GetDocument(int ID)
        {
            XmlDocument xd = new XmlDocument();
            foreach (Worker item in XmlDeserialize())
            {
                if (item.ID == ID)
                {
                    FileInfo fi = new FileInfo(path + @"\" + item.ID + ".xml");
                    if (fi.Exists)
                    {
                        xd.Load(fi.FullName); break;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return xd;
        }

        private XmlDocument GetDocument(string name)
        {
            XmlDocument xd = new XmlDocument();
            foreach (Worker item in XmlDeserialize())
            {
                if (item.fullName == name)
                {
                    FileInfo fi = new FileInfo(path + @"\" + item.ID + ".xml");
                    if (fi.Exists)
                    {
                        xd.Load(fi.FullName); break;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return xd;
        }
    }
}
