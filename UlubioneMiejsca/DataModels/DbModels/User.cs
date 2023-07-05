using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UlubioneMiejsca.DataModels.Respones;

namespace UlubioneMiejsca.DataModels.DbModels
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? Token { get; set; }
        public DateTime? TokenTime { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Phone { get; set; }
        public List<Location>? Locations { get; set; }
        public UserProfile? userProfile { get; set; }
        public List<User>? Friends { get; set; }
    }
}
