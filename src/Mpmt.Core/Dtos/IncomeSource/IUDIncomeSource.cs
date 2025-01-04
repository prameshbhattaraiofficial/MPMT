namespace Mpmt.Core.Dtos.IncomeSource;

public class IUDIncomeSource
{
    public char Event { get; set; }
    public int Id { get; set; }
    public string SourceName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public string LoggedInUser { get; set; }
    public string UserType { get; set; }
}
