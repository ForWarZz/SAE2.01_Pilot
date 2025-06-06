using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class TypeProduitHelper
    {
        public static LibelleTypeProduit LibelleTypeProduitParNom(string nom)
        {
            switch (nom)
            {
                case "Roller gel":
                    return LibelleTypeProduit.RollerGel;
                case "Billes":
                    return LibelleTypeProduit.Bille;
                case "Roller liquide":
                    return LibelleTypeProduit.RollerLiquide;
                case "Plume feutre":
                    return LibelleTypeProduit.PlumeFeutre;
                case "Couleur fun":
                    return LibelleTypeProduit.CouleurFun;
                default:
                    throw new Exception("Type de produit inconnu : " + nom);
            }
        }

        public static string NomParLibelle(LibelleTypeProduit libelle)
        {
            switch (libelle)
            {
                case LibelleTypeProduit.RollerGel:
                    return "Roller gel";
                case LibelleTypeProduit.Bille:
                    return "Billes";
                case LibelleTypeProduit.RollerLiquide:
                    return "Roller liquide";
                case LibelleTypeProduit.PlumeFeutre:
                    return "Plume feutre";
                case LibelleTypeProduit.CouleurFun:
                    return "Couleur fun";
                default:
                    throw new Exception("Libellé de type de produit inconnu : " + libelle);
            }
        }
    }


    public enum LibelleTypeProduit
    {
        RollerGel,
        Bille,
        RollerLiquide,
        PlumeFeutre,
        CouleurFun
    }
}
