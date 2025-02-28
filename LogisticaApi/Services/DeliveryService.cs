using Google.Cloud.Firestore;
using LogisticaApi.Models;

namespace LogisticaApi.Services
{
    public class DeliveryService
    {
        private readonly FirestoreDb _firestoreDb;

        public DeliveryService(IConfiguration configuration)
        {
            string jsonCredentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuration["Firebase:JsonCredentialsPath"]);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonCredentialsPath);
            _firestoreDb = FirestoreDb.Create(configuration["Firebase:ProjectId"]);
        }


        public async Task<string> AddDeliveryAsync(Delivery delivery)
        {
            try
            {
                CollectionReference collection = _firestoreDb.Collection("deliveries");
                var deliveryId = GetDeliveryByIdAsync(delivery.ProductId);

                Dictionary<string, object> deliveryData = new Dictionary<string, object>
            {
                { "Id", delivery.Id },
                { "TrackingId", delivery.TrackingId },
                { "PostalCodeOrigin", delivery.PostalCodeOrigin },
                { "PostalCodeReceiver", delivery.PostalCodeReceiver },
                { "AdressNumber", delivery.AdressNumber },
                { "ReceiverCpf", delivery.ReceiverCpf },
                { "ReceiverName", delivery.ReceiverName },
                { "StreetName", delivery.StreetName },
                { "Neighborhood", delivery.Neighborhood },
                { "Delivered", delivery.Delivered },
                { "CollectedDate", delivery.CollectedDate },
                { "StatusDelivery", delivery.StatusDelivery.ToString() },
                { "ProductId", delivery.ProductId }
            };

                DocumentReference docRef = await collection.AddAsync(deliveryData);
                return docRef.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar entrega: {ex.Message}");
            }
        }

        public async Task<Delivery> GetDeliveryByIdAsync(string id)
        {
            DocumentReference docRef = _firestoreDb.Collection("deliveries").Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
                return null;

            return snapshot.ConvertTo<Delivery>();
        }

        public async Task<List<Delivery>> GetAllDeliveriesAsync()
        {
            Query query = _firestoreDb.Collection("deliveries");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            List<Delivery> deliveries = new List<Delivery>();

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                deliveries.Add(doc.ConvertTo<Delivery>());
            }

            return deliveries;
        }
    }
}
