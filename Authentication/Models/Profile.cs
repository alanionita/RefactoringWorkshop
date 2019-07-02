namespace Authentication.Models
{
    public class Profile : SkinnyProfile
    {
        public string CRMTitle { get; set; }
        public string CommerceTitle { get; set; }
        public string CRMFirstName { get; set; }
        public string CommerceFirstName { get; set; }
        public string CRMLastName { get; set; }
        public string CommerceLastName { get; set; }
        public string Email { get; set; }
        public string NonRegisteredEmail { get; set; }
        public bool IsRegistered { get; set; }
    }
}
