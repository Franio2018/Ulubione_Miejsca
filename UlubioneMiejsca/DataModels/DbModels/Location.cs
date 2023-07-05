using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UlubioneMiejsca.DataModels.Respones;

namespace UlubioneMiejsca.DataModels.DbModels
{
    public class Location
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [Required]
        public string Address { get; set; }
        public int? Rating { get; set; }
        public string? Opinion { get; set; }
        public string? TypeOfFood { get; set; }
    }
}

