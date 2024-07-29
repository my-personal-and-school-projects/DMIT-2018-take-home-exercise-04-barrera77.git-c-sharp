using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeHomeExercise4System.ViewModels
{
    public class CarListView
    {
        public int CarID { get; set; }
        public int TempID { get; set; }
        public string SerialNumber { get; set; }
        public string Ownership { get; set; }
        public string Class { get; set; }
        public string State { get; set; }
        public string Description { get; set; }
        public int MemberID { get; set; }
        public bool RemoveFromViewFlag { get; set; }
    }
}
