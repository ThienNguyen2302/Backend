namespace TDTU.API.Models;

public class SuggestRequest
{
	public Guid? Id { get; set; }
	public string? TextSearch { get; set; }
	public int PageIndex { get; init; } = 1;
	public int PageSize { get; init; } = 10;
	public Guid? TermId { get; set; }
	public string? SkillIds { get; set; }
}
