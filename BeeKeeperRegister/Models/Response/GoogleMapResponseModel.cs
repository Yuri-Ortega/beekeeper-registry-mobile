using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class GoogleMapResponseModel
    {
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
    }
}
