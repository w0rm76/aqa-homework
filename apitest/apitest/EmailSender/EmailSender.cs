using apitest.Interfaces;

namespace apitest.EmailSender;

public class EmailSender : IEmailSender
{
    public void Send(string to, string text)
    {
        Console.WriteLine($"Sending mail to {to}: {text}");
    }
}
