using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeHomeExercise4System.ViewModels
{
    public class CarClassListView
    {
        public int CarClassID {  get; set; }
        public string CarClassName { get; set; }
        public decimal MaxEngineSize { get; set; }
        public string CertificationLevel { get; set; }
        public double RaceRentalFee { get; set; }
        public string Description { get; set; }
        public bool RemoveFromViewFlag { get; set; }

    }
}
