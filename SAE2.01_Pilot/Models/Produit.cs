using Npgsql;
using SAE2._01_Pilot.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypePointe)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
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

                if (value.Length != 5)
                {
                    throw new ArgumentOutOfRangeException("Le code produit doit comporter au moins 5 caractères.");
                }

                code = value.ToUpper();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Code)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Nom)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrixVente)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantiteStock)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Couleurs)));
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

        public Produit(int id, TypePointe typePointe, TypeProduit type, string code, string nom, decimal prixVente, int quantiteStock, ObservableCollection<CouleurProduit> couleurs, bool disponible)
        {
            Id = id;
            TypePointe = typePointe;
            Type = type;
            Code = code;
            Nom = nom;
            PrixVente = prixVente;
            QuantiteStock = quantiteStock;
            Couleurs = couleurs;
            Disponible = disponible;
        }

        public Produit(TypePointe typePointe, TypeProduit type, string code, string nom, decimal prixVente, int quantiteStock, ObservableCollection<CouleurProduit> couleurs, bool disponible)
        {
            TypePointe = typePointe;
            Type = type;
            Code = code;
            Nom = nom;
            PrixVente = prixVente;
            QuantiteStock = quantiteStock;
            this.couleurs = couleurs;
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
            string sqlCheckExists = "SELECT COUNT(*) FROM Produit p WHERE p.CodeProduit = @CodeProduit";

            using (NpgsqlCommand cmdCheck = new NpgsqlCommand(sqlCheckExists))
            {
                cmdCheck.Parameters.AddWithValue("CodeProduit", Code);
                int count = (int)(Int64)DataAccess.Instance.ExecuteSelectUneValeur(cmdCheck);

                if (count > 0)
                {
                    throw new InvalidOperationException("Un produit avec ce code existe déjà.");
                }
            }

            Console.WriteLine("Aucun produit avec ce code n'existe, insertion en base de données...");

            string sqlInsertProduit = @"
                INSERT INTO Produit (CodeProduit, NomProduit, PrixVente, QuantiteStock, Disponible, NumTypePointe, NumType)
                VALUES (@CodeProduit, @NomProduit, @PrixVente, @QuantiteStock, @Disponible, @NumTypePointe, @NumType)
                RETURNING NumProduit;";

            using (NpgsqlCommand cmdInsertProduit = new NpgsqlCommand(sqlInsertProduit))
            {
                cmdInsertProduit.Parameters.AddWithValue("CodeProduit", Code);
                cmdInsertProduit.Parameters.AddWithValue("NomProduit", Nom);
                cmdInsertProduit.Parameters.AddWithValue("PrixVente", PrixVente);
                cmdInsertProduit.Parameters.AddWithValue("QuantiteStock", QuantiteStock);
                cmdInsertProduit.Parameters.AddWithValue("Disponible", Disponible);
                cmdInsertProduit.Parameters.AddWithValue("NumTypePointe", TypePointe.Id);
                cmdInsertProduit.Parameters.AddWithValue("NumType", Type.Id);

                Id = DataAccess.Instance.ExecuteInsert(cmdInsertProduit);
            }

            List<int> couleurIds = Couleurs.Select(c => c.Id).ToList();

            string sqlInsertCouleurs = @"
                    INSERT INTO CouleurProduit (NumProduit, NumCouleur)
                    SELECT @NumProduit, unnest(@NumCouleurIds::int[]);
                ";

            using (NpgsqlCommand cmdInsertCouleurs = new NpgsqlCommand(sqlInsertCouleurs))
            {
                cmdInsertCouleurs.Parameters.AddWithValue("NumProduit", Id);
                cmdInsertCouleurs.Parameters.AddWithValue("NumCouleurIds", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, couleurIds);

                cmdInsertCouleurs.ExecuteNonQuery();
            }
        }


        public void Update()
        {
            /*NpgsqlConnection conn = DataAccess.Instance.GetConnection();
            using (NpgsqlTransaction transaction = conn.BeginTransaction())
            {
                try
                {
                    string sqlCheckCodeExists = "SELECT COUNT(*) FROM Produit WHERE CodeProduit = @CodeProduit AND NumProduit != @Id";
                    using (NpgsqlCommand cmdCheckCode = new NpgsqlCommand(sqlCheckCodeExists, conn, transaction))
                    {
                        cmdCheckCode.Parameters.AddWithValue("CodeProduit", Code);
                        cmdCheckCode.Parameters.AddWithValue("Id", Id);
                        int count = (int)(long)DataAccess.Instance.ExecuteSelectUneValeur(cmdCheckCode);
                        if (count > 0)
                        {
                            throw new InvalidOperationException("Un autre produit utilise déjà ce code. Veuillez choisir un code unique.");
                        }
                    }

                    UpdateBase();

                    string sqlDeleteCouleurs = @"
                        DELETE FROM CouleurProduit
                        WHERE NumProduit = @NumProduit;";

                    using (NpgsqlCommand cmdDeleteCouleurs = new NpgsqlCommand(sqlDeleteCouleurs, conn, transaction))
                    {
                        cmdDeleteCouleurs.Parameters.AddWithValue("NumProduit", Id);
                        cmdDeleteCouleurs.ExecuteNonQuery();
                    }

                    List<int> couleurIds = Couleurs.Select(c => c.Id).ToList();

                    string sqlInsertCouleurs = @"
                            INSERT INTO CouleurProduit (NumProduit, NumCouleur)
                            SELECT @NumProduit, unnest(@NumCouleurIds::int[]);
                        ";

                    using (NpgsqlCommand cmdInsertCouleurs = new NpgsqlCommand(sqlInsertCouleurs, conn, transaction))
                    {
                        cmdInsertCouleurs.Parameters.AddWithValue("NumProduit", Id);
                        cmdInsertCouleurs.Parameters.AddWithValue("NumCouleurIds", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, couleurIds);

                        cmdInsertCouleurs.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }*/
        }

        public void UpdateBase()
        {
            string sqlUpdateProduit = @"
                        UPDATE Produit
                        SET
                            CodeProduit = @CodeProduit,
                            NomProduit = @NomProduit,
                            PrixVente = @PrixVente,
                            QuantiteStock = @QuantiteStock,
                            Disponible = @Disponible,
                            NumTypePointe = @NumTypePointe,
                            NumType = @NumType
                        WHERE NumProduit = @NumProduit";

            using (NpgsqlCommand cmdUpdateProduit = new NpgsqlCommand(sqlUpdateProduit))
            {
                cmdUpdateProduit.Parameters.AddWithValue("CodeProduit", Code);
                cmdUpdateProduit.Parameters.AddWithValue("NomProduit", Nom);
                cmdUpdateProduit.Parameters.AddWithValue("PrixVente", PrixVente);
                cmdUpdateProduit.Parameters.AddWithValue("QuantiteStock", QuantiteStock);
                cmdUpdateProduit.Parameters.AddWithValue("Disponible", Disponible);
                cmdUpdateProduit.Parameters.AddWithValue("NumTypePointe", TypePointe.Id);
                cmdUpdateProduit.Parameters.AddWithValue("NumType", Type.Id);
                cmdUpdateProduit.Parameters.AddWithValue("NumProduit", Id);

                DataAccess.Instance.ExecuteSet(cmdUpdateProduit);
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
