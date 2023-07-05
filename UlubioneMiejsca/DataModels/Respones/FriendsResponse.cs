using System.ComponentModel.DataAnnotations.Schema;

namespace UlubioneMiejsca.DataModels.Respones
{
    public class FriendsResponse
    {
        public string Name { get; set; }
        public string Surname { get; set;}
        public string Phone { get; set; }
        public List<LocationResponse> Locations { get; set; }
    }
}
