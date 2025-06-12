using Npgsql;
using SAE2._01_Pilot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SAE2._01_Pilot.Models
{
    public class LigneCommande
    {
        private int quantite;
        private Produit produit;

        public Produit Produit {
            get => produit;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Le produit ne peut pas être nul.");
                }

                produit = value;
            }
        }

        public int Quantite
        {
            get => quantite;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("La quantité doit être supérieure à zéro.");
                }

                quantite = value;
            }
        }

        public LigneCommande(Produit produit, int quantite)
        {
            Produit = produit;
            Quantite = quantite;
        }

        public LigneCommande()
        {
            
        }
    }
}
