namespace PFE_PROJECT.Helpers
{
    public static class RoleHelper
    {
        public const string AdminIT = "Admin IT";
        public const string AdministrateurMetier = "Administrateur Métier";
        public const string ResponsableUnite = "Responsable Unité";

        public static bool IsValidRole(string roleName)
        {
            return roleName == AdminIT || 
                   roleName == AdministrateurMetier || 
                   roleName == ResponsableUnite;
        }
    }
}

//pour verifier que les roles doit étre l'un des 3 citéé en dessus