using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UlubioneMiejsca.DataModels
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Phone { get; set; }
        public List<Location>? Locations { get; set; }
    }
}
