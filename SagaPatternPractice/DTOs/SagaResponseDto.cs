namespace SagaPatternPractice.DTOs
{
    public class SagaResponseDto
    {
        public bool Success { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Steps { get; set; } = new List<string>();
    }
}
