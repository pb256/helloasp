using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Models.Model;

[Table("user_score")]
public class UserScore
{
    [Key] [MaxLength(64)]
    public string uid { get; set; }
    
    public int score { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime created { get; set; }
}