﻿using System.Collections.Generic;

namespace RunTogether
{
    public class RunRoute
    {
        public int RunRouteId { get; set; }
        public List<Stage> Stages { get; set; }
        
        public int RunId { get; set; }

        public Run Run { get; set; }

    }
}