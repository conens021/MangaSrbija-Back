
using MailKit.Net.Smtp;
using MimeKit;

namespace MangaSrbija.BLL.helpers
{
    public class EmailClient
    {
        public static async Task Send(string recipientName, string recipientAddress,string messageText)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin",
            "n.rackovic021@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress(recipientName, recipientAddress);
            message.To.Add(to);

            message.Subject = "Kod za resetovanje lozinke";

            BodyBuilder bodyBuilder = new BodyBuilder();
            string body = $"<h2>Vaš šestocifreni kod za resetovanje lozinke:</h2><br/><h2>{messageText}</h2>";
            bodyBuilder.HtmlBody = body;


            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate("n.rackovic021@gmail.com", "klisa021ns");


            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            client.Dispose();

        }
    }
}
