using Proekt.DAL.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proekt.DAL.Classes
{
    [Serializable]
    public class Worker : IWorker
    {
        public Worker()
        {
            //Random rand = new Random();
            //ServicePosition sp = new ServicePosition();

            ////fullName = "XJ" + rand.Next(1, 10000);
            ////ID = rand.Next(1, 10000);
            //position = sp.ReturnRandomPosition();
        }
        public int ID { get; set; }
        public string fullName { get; set; }
        public string position { get; set; }
        
    }
}
