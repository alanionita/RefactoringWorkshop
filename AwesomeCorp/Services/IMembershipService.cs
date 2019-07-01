namespace BigMess.Services
{
    public interface IMembershipService
    {
        bool AuthenticateUser(string emailAddress, string password);
    }
}