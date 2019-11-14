using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace SAPI.Models
{
    public class ReservationResponse
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }

        [BrowsableAttribute(false)]
        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }

        [JsonProperty(PropertyName = "discount")]
        public float Discount { get; set; }

        [JsonProperty(PropertyName = "prePaid")]
        public bool PrePaid { get; set; }

        [JsonProperty(PropertyName = "prePaidTo")]
        public DateTime PrePaidTo { get; set; }

        [JsonProperty(PropertyName = "reduction")]
        public float Reduction { get; set; }

        [JsonProperty(PropertyName = "reservableObject")]
        public ReservableObject reservableObject { get; set; }

        [JsonProperty(PropertyName = "reservationNo")]
        public int ReservationNo { get; set; }

        [JsonProperty(PropertyName = "reservationProductCode")]
        public string ReservationProductCode { get; set; }

        [JsonProperty(PropertyName = "reservedFrom")]
        public DateTime ReservedFrom { get; set; }

        [JsonProperty(PropertyName = "reservedTo")]
        public DateTime ReservedTo { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }

    public class ReservableObject
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }

    public class Reservation
    { 
        public int Nr { get; set; }
        public string Cubicle { get; set; }
        public string Customer { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
