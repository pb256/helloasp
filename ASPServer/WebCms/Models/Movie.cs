using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCms.Models;

public class Movie
{
    // 기본키 대신 필요
    public int Id { get; set; }

    [Required]
    [StringLength(60, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    
    [Display(Name = "개봉일")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    
    [Required]
    [StringLength(30)]
    public string Genre { get; set; }
    
    // 값 형식(예: decimal, int, float, DateTime)은 기본적으로 필수이며, [Required] 특성이 필요하지 않습니다.
    [Column(TypeName = "decimal(18, 2)")]    
    public decimal Price { get; set; }

    // 정규식도 쓸수있다
    // [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    [Required]
    [Display(Name = "평점")]
    public string Rating { get; set; } = string.Empty;
}