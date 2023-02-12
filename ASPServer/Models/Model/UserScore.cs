using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Model;

// 원래 키값 없이 스코어를 계속 누적하려고 만들었는데 데이터를 추가하는데 문제가 있어서 키값이 있는 형식으로 바꾸었다가
// 데이터베이스에 업데이트하니까 에러가 났음
[Table("user_score")]
public class UserScore
{
    [Key] [MaxLength(255)]
    public string uid { get; set; }
    public int score { get; set; }
    public DateTime time_stamp { get; set; }
}