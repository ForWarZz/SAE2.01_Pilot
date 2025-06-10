using Npgsql;
using SAE2._01_Pilot.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public TypePointe TypePointe { get; set; }
        public TypeProduit Type { get; set; }
        public string Code
        {
            get => code;
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

                code = value;
            }
        }

        public string Nom
        {
            get => nom;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom du produit ne peut pas être vide.");
                }

                nom = value;
            }
        }

        public decimal PrixVente
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

        public List<CouleurProduit> Couleurs
        {
            get => couleurs;
            set => couleurs = value;
        }

        public bool Disponible { get; set; }

        private string code;
        private string nom;
        private decimal prixVente;
        private int quantiteStock;

        private List<CouleurProduit> couleurs;

        public Produit(TypePointe typePointe, TypeProduit typeProduit, string codeProduit, string nomProduit, decimal prixVente, int quantiteStock, List<CouleurProduit> couleurs, bool disponible)
        {
            TypePointe = typePointe;
            Type = typeProduit;
            Code = codeProduit;
            Nom = nomProduit;
            PrixVente = prixVente;
            QuantiteStock = quantiteStock;
            Couleurs = couleurs;
            Disponible = disponible;
        }

        public Produit(int id, TypePointe typePointe, TypeProduit typeProduit, string codeProduit, string nomProduit, decimal prixVente, int quantiteStock, List<CouleurProduit> couleurs, bool disponible) 
            : this(typePointe, typeProduit, codeProduit, nomProduit, prixVente, quantiteStock, couleurs, disponible)
        {
            Id = id;
        }

        public string CouleursString => string.Join(", ", Couleurs.Select(c => c.Libelle));

        public static ObservableCollection<Produit> GetAll(List<TypePointe> typePointes, List<TypeProduit> typeProduits, List<CouleurProduit> couleurs)
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
                    tp.NumTypePointe, 
                    t.NumType,
                    c.NumCouleur
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
                        TypePointe? typePointe = typePointes.FirstOrDefault(tp => tp.Id == (int)row["NumTypePointe"]);
                        TypeProduit? typeProduit = typeProduits.FirstOrDefault(tp => tp.Id == (int)row["NumType"]);

                        Produit produit = new Produit(
                            id: numProduit,
                            typePointe: typePointe,
                            typeProduit: typeProduit,
                            codeProduit: row["CodeProduit"].ToString(),
                            nomProduit: row["NomProduit"].ToString(),
                            prixVente: (decimal)row["PrixVente"],
                            quantiteStock: (int)row["QuantiteStock"],
                            couleurs: new List<CouleurProduit>(),
                            disponible: (bool)row["Disponible"]
                        );

                        produitsParId[numProduit] = produit;
                    }

                    CouleurProduit? couleur = couleurs.FirstOrDefault(c => c.Id == (int)row["NumCouleur"]);
                    produitsParId[numProduit].Couleurs.Add(couleur);
                }
            }

            return new ObservableCollection<Produit>(produitsParId.Values.ToList());
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
