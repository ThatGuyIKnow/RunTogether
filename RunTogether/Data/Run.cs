using Microsoft.AspNetCore.Routing;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RunTogether.Areas.Identity;

namespace RunTogether
{

    public class Run
    {
        public int ID { get; set; }

        public string Name { get; set; } = "";

        public string QRString { get; set; } = "";

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public RunRoute? Route { get; set; }
        public List<ApplicationUser> Runners { get; set; } = new List<ApplicationUser>();
        private int NextRunnerId { get; set; } = 1;

        public int GetNextRunnerId() { return NextRunnerId; }
        public void IncrementRunnerId() { NextRunnerId++; }
    }
}
