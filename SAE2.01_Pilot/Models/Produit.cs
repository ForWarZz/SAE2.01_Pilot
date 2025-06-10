using Npgsql;
using SAE2._01_Pilot.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SAE2._01_Pilot.Models
{
    public class Produit : ICrud<Produit>, INotifyPropertyChanged
    {
        public int Id { get; set; }

        public TypePointe TypePointe
        {
            get => typePointe;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Le type de pointe ne peut pas être nul.");
                }

                typePointe = value;
                OnPropertyChanged(nameof(TypePointe));
            }
        }

        public TypeProduit Type
        {
            get => type;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Le type de produit ne peut pas être nul.");
                }

                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

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
                OnPropertyChanged(nameof(Code));
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
                OnPropertyChanged(nameof(Nom));
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
                OnPropertyChanged(nameof(PrixVente));
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
                OnPropertyChanged(nameof(QuantiteStock));
            }
        }

        public ObservableCollection<CouleurProduit> Couleurs
        {
            get => couleurs;
            set
            {
                if (value == null || value.Count == 0)
                {
                    throw new ArgumentException("La liste des couleurs ne peut pas être vide.");
                }

                couleurs = value;
                OnPropertyChanged(nameof(Couleurs));
            }
        }

        public bool Disponible { get; set; }

        private string code;
        private string nom;
        private TypePointe typePointe;
        private TypeProduit type;
        private decimal prixVente;
        private int quantiteStock;

        private ObservableCollection<CouleurProduit> couleurs;
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Produit()
        {
            couleurs = new ObservableCollection<CouleurProduit>();
        }

        public Produit(int id, TypePointe typePointe, TypeProduit typeProduit, string codeProduit, string nomProduit, decimal prixVente, int quantiteStock, bool disponible) 
        {
            Id = id;

            TypePointe = typePointe;
            Type = typeProduit;
            Code = codeProduit;
            Nom = nomProduit;
            PrixVente = prixVente;
            QuantiteStock = quantiteStock;
            couleurs = new ObservableCollection<CouleurProduit>();
            Disponible = disponible;
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
