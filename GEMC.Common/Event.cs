using System.Collections.Generic;

namespace GEMC.Common
{
    public class Event
    {
        public string Version { get; set; }

        public string Key { get; set; }

        public long Timestamp { get; set; }

        public Config Config { get; set; }

        public Metadata Metadata { get; set; }

        public List<Data> Data { get; set; }
    }
}