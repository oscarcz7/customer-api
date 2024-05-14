using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace customer_api.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string Nombre { get; set; } = null!;
        [BsonElement("Lastname")]
        public string Apellido { get; set; } = null!;
        [BsonElement("AccountNumber")]
        public string NumCuenta { get; set; } = null!;
        [BsonElement("Balance")]
        public decimal Saldo { get; set; }
        [BsonElement("Birthdate")]
        public string FechaNacimiento { get; set; } = null!;
        [BsonElement("Address")]
        public string Direccion { get; set; } = null!;
        [BsonElement("Phone")]
        public string Telf { get; set; } = null!;
        [BsonElement("Mail")]
        public string Correo { get; set; } = null!;
        [BsonElement("ClientType")]
        public string TipoCliente { get; set; } = null!;
        [BsonElement("CivilStatus")]
        public string EstadoCivil { get; set; } = null!;
        [BsonElement("Identification")]
        public string NumIdentificacion { get; set; } = null!;
        [BsonElement("Ocupation")]
        public string Ocupacion { get; set; } = null!;
        [BsonElement("Gender")]
        public string Genero { get; set; } = null!;
        [BsonElement("Nacionality")]
        public string Nacionalidad { get; set; } = null!;

    }
}
