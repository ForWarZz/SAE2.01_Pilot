
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class CouleurProduitHelper
    {
        public static LibelleCouleurProduit LibelleCouleurParNom(string nom)
        {
            switch (nom)
            {
                case "Bleu":
                    return LibelleCouleurProduit.Bleu;
                case "Noir":
                    return LibelleCouleurProduit.Noir;
                case "Vert":
                    return LibelleCouleurProduit.Vert;
                case "Rouge":
                    return LibelleCouleurProduit.Rouge;
                default:
                    throw new Exception("Couleur de produit inconnue : " + nom);
            }
        }

        public static string NomParLibelle(LibelleCouleurProduit libelle)
        {
            switch (libelle)
            {
                case LibelleCouleurProduit.Bleu:
                    return "Bleu";
                case LibelleCouleurProduit.Noir:
                    return "Noir";
                case LibelleCouleurProduit.Vert:
                    return "Vert";
                case LibelleCouleurProduit.Rouge:
                    return "Rouge";
                default:
                    throw new Exception("Libellé de couleur inconnu : " + libelle);
            }
        }
    }

    public enum LibelleCouleurProduit
    {
        Bleu,
        Noir,
        Vert,
        Rouge
    }
}
