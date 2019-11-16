using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPI.Models
{
    public class Transaction : Base
    {
        [JsonProperty(PropertyName = "time")]
        public DateTime Time { get; set; }

        [JsonProperty(PropertyName = "reservableObject")]
        public ReservableObject ReservableObject { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "total")]
        public float Total { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
