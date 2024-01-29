namespace Identity.Services.Authorization.Const
{
    public static class AuthorizePermissionConsts
    {
        public static string Permission = "Permissions";
        public const string FullAccess = "FullAccess";
        public static class User
        {
            public const string UserAccess = "UserAccess";

            public const string GetAllUser = "Permission.User.GetAll";
            public const string GetUser = "Permission.User.GetUser";
        }

        public static class Role
        {
            public const int GetRole = 50;
            public const int GetAllRole = 51;
            public const int CreateRole = 52;
            public const int UpdateRole = 53;
            public const int DeleteRole = 54;
        }
    }
}
