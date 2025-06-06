using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class TypePointeHelper
    {
        public static LibelleTypePointe LibellePointeParNom(string nom)
        {
            switch (nom)
            {
                case "Pointe fine":
                    return LibelleTypePointe.Fine;
                case "Pointe moyenne":
                    return LibelleTypePointe.Moyenne;
                case "Pointe épaisse":
                    return LibelleTypePointe.Epaisse;
                default:
                    throw new Exception("Type de pointe inconnu : " + nom);
            }
        }

        public static string NomParLibelle(LibelleTypePointe libelle)
        {
            switch (libelle)
            {
                case LibelleTypePointe.Fine:
                    return "Pointe fine";
                case LibelleTypePointe.Moyenne:
                    return "Pointe moyenne";
                case LibelleTypePointe.Epaisse:
                    return "Pointe épaisse";
                default:
                    throw new Exception("Libellé de type de pointe inconnu : " + libelle);
            }
        }
    }

    public enum LibelleTypePointe
    {
        Fine,
        Moyenne,
        Epaisse
    }
}
