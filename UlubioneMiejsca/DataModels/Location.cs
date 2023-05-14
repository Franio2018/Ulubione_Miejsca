using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UlubioneMiejsca.DataModels
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Address { get; set; }
        public int? Rating { get; set; }
        public string? Opinion { get; set; }
        public string? TypeOfFood { get; set; }
        public Guid UserId { get; set; }
    }
}

