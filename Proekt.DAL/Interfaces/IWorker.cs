using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proekt.DAL.Classes
{
    public interface IWorker
    {
        int ID { get; set; }
        string fullName { get; set; }
       // Position position { get; set; }
    }
}
