namespace Mpmt.Core.Dtos.Sms;

public class SociairErrorResponse
{
    public string Message { get; set; }
    public int Ntc { get; set; }
    public int Ncell { get; set; }
    public int Smartcell { get; set; }
    public int Other { get; set; }
    public IEnumerable<string> InvalidNumber { get; set; }  
    public string Errors { get; set; }
}