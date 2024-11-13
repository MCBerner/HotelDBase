using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HotelDBase
{
    public class Facility
    {
        public int Facility_ID { get; set; }

        public string FacilityName { get; set; }

        public override string ToString()
        {
            return $"ID: {Facility_ID}, Name: {FacilityName}";
        }

    }
   
}
