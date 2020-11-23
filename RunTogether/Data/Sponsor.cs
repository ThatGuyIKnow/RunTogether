using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RunTogether.Data
{
    public class Sponsor
    {
        public int SponsorId { get; set; }

        public string Name { get; set; }

        public string? Image { get; set; }

        public List<Stage> Stages { get; set; } = new List<Stage>();
    }
}
