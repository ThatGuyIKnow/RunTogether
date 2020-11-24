using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RunTogether.Data
{
    public class Sponsor
    {
        public int SponsorId { get; set; }

        public string Name { get; set; }

        public string? Image { get; set; }

        [JsonIgnore]
        public List<Stage> Stages { get; set; } = new List<Stage>();


        public Dictionary<string, object> ToJsonSerializableViewer()
        {
            return new Dictionary<string, object>(){{"Name", Name}, {"PictureURL", Image}};
        }
    }
}
