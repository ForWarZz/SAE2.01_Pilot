using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class CategorieProduitHelper
    {
        public static LibelleCategorieProduit LibelleCategorieParNom(string nom)
        {
            switch (nom)
            {
                case "Bureau":
                    return LibelleCategorieProduit.Bureau;
                case "École":
                    return LibelleCategorieProduit.Ecole;
                case "Loisir":
                    return LibelleCategorieProduit.Loisir;
                case "Haute écriture":
                    return LibelleCategorieProduit.HauteEcriture;
                default:
                    throw new Exception("Catégorie de produit inconnue : " + nom);
            }
        }

        public static string NomParLibelle(LibelleCategorieProduit libelle)
        {
            switch (libelle)
            {
                case LibelleCategorieProduit.Bureau:
                    return "Bureau";
                case LibelleCategorieProduit.Ecole:
                    return "École";
                case LibelleCategorieProduit.Loisir:
                    return "Loisir";
                case LibelleCategorieProduit.HauteEcriture:
                    return "Haute écriture";
                default:
                    throw new Exception("Libellé de catégorie inconnu : " + libelle);
            }
        }
    }

    public enum LibelleCategorieProduit
    {
        Bureau,
        Ecole,
        Loisir,
        HauteEcriture
    }
}
