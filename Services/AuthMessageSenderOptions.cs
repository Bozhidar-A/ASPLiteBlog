namespace ASPLiteBlog.Services
{
    public class AuthMessageSenderOptions
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPSenderName { get; set; }
        public string SMTPSenderMail { get; set; }
        public string SMTPSenderPassword { get; set; }
        public string? SendGridKey { get; set; }
    }
}
