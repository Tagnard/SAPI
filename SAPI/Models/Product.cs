using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPI.Models
{
    public class Product : Base
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ID { get; set; }

        [JsonProperty(PropertyName = "productNo")]
        public int ProductNumber { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "price")]
        public float Price { get; set; }

        [JsonProperty(PropertyName = "discount")]
        public string Discount { get; set; }

        [JsonProperty(PropertyName = "extras")]
        public bool Extras { get; set; }

        // Unused because I don't have an example how it is used.
        //[JsonProperty(PropertyName = "reservation")]
        //public string Reservation { get; set; }

        [JsonProperty(PropertyName = "activeReservationId")]
        public Guid ActiveReservationID { get; set; }

        [JsonProperty(PropertyName = "activeReservableObjectName")]
        public string ActiveReservableObjectName { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }

        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }

        [JsonProperty(PropertyName = "labelPrinted")]
        public bool LabelPrinted { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "readOnly")]
        public bool ReadOnly { get; set; }

        [JsonProperty(PropertyName = "sales")]
        public List<Sale> Sales { get; set; }
    }

    public class Sale
    {
        [JsonProperty(PropertyName = "soldAt")]
        public DateTime SoldAt { get; set; }

        [JsonProperty(PropertyName = "transactionId")]
        public Guid TransactionID { get; set; }

        [JsonProperty(PropertyName = "reservationId")]
        public Guid ReservationID { get; set; }

        [JsonProperty(PropertyName = "amout")]
        public float Amount { get; set; }
    }
}
