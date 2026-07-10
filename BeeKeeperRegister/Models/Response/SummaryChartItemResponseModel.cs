using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Models.Response
{
    public class SummaryChartItemResponseModel
    {
        public string Label { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
