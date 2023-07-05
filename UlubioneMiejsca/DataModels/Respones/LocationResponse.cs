using System.ComponentModel.DataAnnotations.Schema;

namespace UlubioneMiejsca.DataModels.Respones
{
    public class LocationResponse
    {
        public string Address { get; set; }
        public int? Rating { get; set; }
        public string? Opinion { get; set; }
        public string? TypeOfFood { get; set; }
    }
}
