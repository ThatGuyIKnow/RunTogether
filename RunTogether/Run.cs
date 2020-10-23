using Microsoft.AspNetCore.Routing;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether
{

    public class Run
    {
        public int ID { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public RunRoute Route { get; set; }
    }
}
