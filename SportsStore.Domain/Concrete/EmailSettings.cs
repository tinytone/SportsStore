namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "tony.harding@bgoworld.com";

        public string MailFromAddress = "smtp.relay@bgoworld.com";

        public bool UseSsl = false;

        public string Username = string.Empty;

        public string Password = string.Empty;

        public string ServerName = "smtp.silo18.local";

        public int ServerPort = 25;

        public bool WriteAsFile = false;

        public string FileLocation = @"c:\sports_Store_emails";
    }
}
