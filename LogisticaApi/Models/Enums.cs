using System.Text.Json.Serialization;

namespace LogisticaApi.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeliveryStatus
    {
        Created,
        Collected,
        Shipping,
        Received
    }
}
