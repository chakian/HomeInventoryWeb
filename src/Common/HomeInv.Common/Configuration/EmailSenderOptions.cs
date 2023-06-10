namespace HomeInv.Common.Configuration;

public class EmailSenderOptions
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; } = 587;
    public string SenderAddress { get; set; }
    public string SenderName { get; set; }
    public string SenderPassword { get; set; }
}
