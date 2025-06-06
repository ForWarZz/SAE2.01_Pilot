using Npgsql;
using SAE2._01_Pilot.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SAE2._01_Pilot.Models
{
    public class Produit : ICrud<Produit>
    {
        public int Id { get; set; }
        public LibelleTypePointe TypePointe { get; set; }
        public LibelleTypeProduit TypeProduit { get; set; }
        public string CodeProduit
        {
            get => codeProduit;
            set
            { 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le code produit ne peut pas être vide.");
                }

                if (value.Length < 5)
                {
                    throw new ArgumentOutOfRangeException("Le code produit doit comporter au moins 5 caractères.");
                }

                codeProduit = value;
            }
        }

        public string NomProduit
        {
            get => nomProduit;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom du produit ne peut pas être vide.");
                }

                nomProduit = value;
            }
        }

        public double PrixVente
        {
            get => prixVente;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Le prix de vente ne peut pas être négatif.");
                }

                prixVente = value;
            }
        }

        public int QuantiteStock
        {
            get => quantiteStock;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("La quantité en stock ne peut pas être négative.");
                }
                quantiteStock = value;
            }
        }

        public List<LibelleCouleurProduit> Couleurs
        {
            get => couleurs;
            set => couleurs = value;
        }


        public bool Disponible { get; set; }

        private string codeProduit;
        private string nomProduit;
        private double prixVente;
        private int quantiteStock;

        private List<LibelleCouleurProduit> couleurs;

        public Produit(int id, string typePointe, string typeProduit, string codeProduit, string nomProduit, double prixVente, int quantiteStock, bool disponible)
        {
            Id = id;
            TypePointe = TypePointeHelper.LibellePointeParNom(typePointe);
            TypeProduit = TypeProduitHelper.LibelleTypeProduitParNom(typeProduit);
            CodeProduit = codeProduit;
            NomProduit = nomProduit;
            PrixVente = prixVente;
            QuantiteStock = quantiteStock;
            Couleurs = new List<LibelleCouleurProduit>();
            Disponible = disponible;
        }

        public static List<Produit> GetAll()
        {
            Dictionary<int, Produit> produitsParId = new Dictionary<int, Produit>();

            string sql = @"
                SELECT 
                    p.NumProduit, 
                    p.CodeProduit, 
                    p.NomProduit, 
                    p.PrixVente, 
                    p.QuantiteStock, 
                    p.Disponible, 
                    tp.LibelleTypePointe, 
                    t.LibelleType,
                    c.LibelleCouleur
                FROM Produit p
                JOIN TypePointe tp ON tp.NumTypePointe = p.NumTypePointe
                JOIN Type t ON t.NumType = p.NumType
                JOIN CouleurProduit cp ON cp.NumProduit = p.NumProduit
                JOIN Couleur c ON c.NumCouleur = cp.NumCouleur
            ";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);

                foreach (DataRow row in dt.Rows)
                {
                    int numProduit = (int)row["NumProduit"];

                    if (!produitsParId.ContainsKey(numProduit))
                    {
                        Produit produit = new Produit(
                            numProduit,
                            row["LibelleTypePointe"].ToString(),
                            row["LibelleType"].ToString(),
                            row["CodeProduit"].ToString(),
                            row["NomProduit"].ToString(),
                            (double)row["PrixVente"],
                            (int)row["QuantiteStock"],
                            (bool)row["Disponible"]
                        );

                        produitsParId[numProduit] = produit;
                    }

                    produitsParId[numProduit].Couleurs.Add(CouleurProduitHelper.LibelleCouleurParNom(row["LibelleCouleur"].ToString()));
                }
            }

            return produitsParId.Values.ToList();
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
