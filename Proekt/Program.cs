using Proekt.DAL.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proekt
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServiceWorker serviceWorker = new ServiceWorker(null);

            ////serviceWorker.AddWorker();
            ////serviceWorker.ShowWorkers();
            ////serviceWorker.EditWorker();
            ////serviceWorker.ShowWorkers();
            ////serviceWorker.XmlSerialize();
            ////serviceWorker.SearchWorkerByName("");
            ////serviceWorker.SearchWorkerByIDForRemove(6682);
            ////Console.WriteLine();
            ////serviceWorker.ShowWorkers();

            ////---------------------------------------------------

            //ServicePosition servicePostion = new ServicePosition(null);
            ////servicePostion.ShowPositions();
            ////servicePostion.AddPosition();
            ////servicePostion.AddPosition();
            ////servicePostion.ShowPositions();
            ////servicePostion.EditPosition();
            ////servicePostion.ShowPositions();
            ////servicePostion.SearchPoisitionByName("wetyr"); 
            //servicePostion.ShowPositions();

            Dictionary<string, string> LoginParol = new Dictionary<string, string>();
            LoginParol.Add("Oruell", "1984");
            LoginParol.Add("Hitler", "1488");
            LoginParol.Add("AK", "47");

            while (Autorization(LoginParol)) { }
            menu();
        }


        static bool Autorization(Dictionary<string, string> LoginParol)
        {
            Console.Clear();
            Console.Write("Введите логин ->"); string login = Console.ReadLine();
            Console.Write("Введите пароль ->"); string parol = Console.ReadLine();
            if (LoginParol.Where(o => o.Key == login && o.Value == parol).Count() > 0)
                return false;
            return true;
        }

        static void menu()
        {
            ServiceWorker serviceWorker = new ServiceWorker(null);
            ServicePosition servicePostion = new ServicePosition(null);
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("С каким справочником будем работать?");
                    Console.WriteLine("1 - Сотрудники");
                    Console.WriteLine("2 - Должности");
                    Console.WriteLine("0 - Выход");
                    switch (Int32.Parse(Console.ReadLine()))
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("-----------Меню сотрудников------------");
                            Console.WriteLine("1 - Отображение списка всех сотрудников.");
                            Console.WriteLine("2 - Прием на работу нового сотрудника.");
                            Console.WriteLine("3 - Редактирование карточки сотрудника");
                            Console.WriteLine("4 - Увольнение сотрудника.");
                            Console.WriteLine("5 - Поиск сотрудника по имени.");
                            Console.WriteLine("6 - Отображение статистики.");
                            Console.WriteLine("0 - Назад.");
                            switch (Int32.Parse(Console.ReadLine()))
                            {
                                case 1: serviceWorker.ShowWorkers(); Console.ReadKey(); break;
                                case 2:
                                    if (serviceWorker.AddWorker())
                                    {
                                        Console.WriteLine("Сотрудник успешно добавлен"); Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Сотрудник не добавлен"); Console.ReadKey();
                                    }
                                    break;
                                case 3:
                                    if (serviceWorker.EditWorker())
                                    {
                                        Console.WriteLine("Сотрудник успешно редактирован"); Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Сотрудник не редактирован"); Console.ReadKey();
                                    }
                                    break;
                                case 4:
                                    if (serviceWorker.RemoveWorker())
                                    {
                                        Console.WriteLine("Сотрудник успешно уволен"); Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Сотрудник не уволен"); Console.ReadKey();
                                    }
                                    break;
                                case 5: serviceWorker.SearchWorkerByName(); Console.ReadKey(); break;
                                case 6: serviceWorker.ShowStatistics(); Console.ReadKey(); break;
                                case 0: break;
                                default: break;
                            }
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("-----------Меню должностей------------");
                            Console.WriteLine("1 - Отображение списка всех должностей.");
                            Console.WriteLine("2 - Добавление новой должности.");
                            Console.WriteLine("3 - Редактирование должности");
                            Console.WriteLine("4 - Поиск должности по имени.");
                            Console.WriteLine("5 - Отображение статистики.");
                            Console.WriteLine("0 - Назад.");
                            switch (Int32.Parse(Console.ReadLine()))
                            {
                                case 1: servicePostion.ShowPositions(); Console.ReadKey(); break;
                                case 2:
                                    if (servicePostion.AddPosition())
                                    {
                                        Console.WriteLine("Должность успешно добавлена"); Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Должность не добавлена"); Console.ReadKey();
                                    }
                                    break;
                                case 3:
                                    if (servicePostion.EditPosition())
                                    {
                                        Console.WriteLine("Должность успешно редактирована"); Console.ReadKey();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Должность не редактирована"); Console.ReadKey();
                                    }
                                    break;
                                case 4: servicePostion.SearchPoisitionByName(); Console.ReadKey(); break;
                                case 5: servicePostion.ShowStatistics(); Console.ReadKey(); break;
                                case 0: break;
                                default: break;
                            }
                            break;
                        case 0: return;
                        default: break;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }
    }
}
