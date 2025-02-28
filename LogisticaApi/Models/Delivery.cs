using Google.Cloud.Firestore;

namespace LogisticaApi.Models
{
    [FirestoreData]
    public class Delivery
    {
        [FirestoreProperty]
        public int Id { get; set; }

        [FirestoreProperty]
        public string TrackingId { get; set; }

        [FirestoreProperty]
        public int PostalCodeOrigin { get; set; }

        [FirestoreProperty]
        public int PostalCodeReceiver { get; set; }

        [FirestoreProperty]
        public int AdressNumber { get; set; }

        [FirestoreProperty]
        public string ReceiverCpf { get; set; }

        [FirestoreProperty]
        public string ReceiverName { get; set; }

        [FirestoreProperty]
        public string StreetName { get; set; }

        [FirestoreProperty]
        public string Neighborhood { get; set; }

        [FirestoreProperty]
        public DateTime? Delivered { get; set; }

        [FirestoreProperty]
        public DateTime? CollectedDate { get; set; }

        [FirestoreProperty("status", ConverterType = typeof(FirestoreEnumNameConverter<DeliveryStatus>))]
        public DeliveryStatus StatusDelivery { get; set; }

        [FirestoreProperty]
        public string ProductId { get; set; }
    }
}
