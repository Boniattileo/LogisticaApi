using Google.Cloud.Firestore;
using LogisticaApi.Models;

namespace LogisticaApi.Services
{
    public class ProductService
    {
        private readonly FirestoreDb _firestoreDb;

        public ProductService(IConfiguration configuration)
        {
            string jsonCredentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configuration["Firebase:JsonCredentialsPath"]);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", jsonCredentialsPath);
            _firestoreDb = FirestoreDb.Create(configuration["Firebase:ProjectId"]);
        }

        public async Task<string> AddProductAsync(Product product)
        {
            try
            {
                CollectionReference collection = _firestoreDb.Collection("products");

                Dictionary<string, object> productData = new Dictionary<string, object>
                {
                    { "Id", product.Id },
                    { "Width", product.Width },
                    { "Height", product.Height },
                    { "Length", product.Length },
                    { "Weight", product.Weight }
                };

                DocumentReference docRef = await collection.AddAsync(productData);
                return docRef.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar produto: {ex.Message}");
            }
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            try
            {
                DocumentReference docRef = _firestoreDb.Collection("products").Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (!snapshot.Exists)
                    return null;

                return snapshot.ConvertTo<Product>();
            }
            catch (Exception ex) 
            {
                throw new Exception($"Erro ao obter produto: {ex.Message}");
            }
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            Query query = _firestoreDb.Collection("products");
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            List<Product> products = new List<Product>();

            foreach (DocumentSnapshot doc in snapshot.Documents)
            {
                products.Add(doc.ConvertTo<Product>());
            }

            return products;
        }
    }
}

