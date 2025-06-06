using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class RoleHelper
    {
        public static LibelleRole LibelleRoleParNom(string nom)
        {
            switch (nom)
            {
                case "Commercial":
                    return LibelleRole.Commercial;
                case "Responsable de production":
                    return LibelleRole.ResponsableProduction;
                default:
                    throw new ArgumentException("Nom de rôle inconnu : " + nom);
            }
        }
    }

    public enum LibelleRole
    {
        Commercial,
        ResponsableProduction
    }
}
