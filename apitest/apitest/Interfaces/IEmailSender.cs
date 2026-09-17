namespace apitest.Interfaces;

public interface IEmailSender
{
    void Send(string to, string text);
}