namespace AwesomeCorp.Services
{
    public interface IMembershipService
    {
        bool AuthenticateUser(string emailAddress, string password);
    }
}
