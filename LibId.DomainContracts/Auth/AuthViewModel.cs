namespace LibIdentity.DomainContracts.Auth
{
    public class AuthViewModel
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public List<int> Permissions { get; set; }

        public AuthViewModel() { }

        public AuthViewModel(long id, long roleId, string role, string fullName, string userName, string mobile, List<int> permissions)
        {
            Id = id;
            RoleId = roleId;
            Role = role;
            FullName = fullName;
            UserName = userName;
            Mobile = mobile;
            Permissions = permissions;
        }
    }
}