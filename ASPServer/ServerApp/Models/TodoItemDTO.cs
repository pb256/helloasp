namespace ServerApp.Models;

// DTO(데이터 전송 개체)
// 그 용도는 무엇인가
// - 과도한 게시 방지 (필드를 줄일수 있어서?)
// - 일부 속성 숨기기 (응답으로 보낼때 첨부시킬수있음)
// - 페이로드 크기를 줄이기 위해 일부 속성을 생략
// - 중첩된 개체를 포함하는 개체 그래프를 평면화

public class TodoItemDTO
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}