namespace Commerce.Models
{
    public class UserProfile
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string NonRegisteredEmailAddress { get; set; }
        public UserTypes? UserType { get; set; }
        public RegistrationStatus MatchStatus { get; set; }
    }
}
