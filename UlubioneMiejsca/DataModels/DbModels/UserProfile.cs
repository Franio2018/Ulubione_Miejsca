using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UlubioneMiejsca.DataModels.DbModels
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public byte[] UserProfileData { get; set; }
        public string UserNickName { get; set; }
    }
}
