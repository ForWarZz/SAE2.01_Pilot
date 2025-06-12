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
    public class Commande : ICrud<Commande>, INotifyPropertyChanged
    {
        public int Id { get; set; }

        private ObservableCollection<LigneCommande> ligneCommandes;
        private DateTime dateCreation;
        private DateTime? dateLivraison;

        private ModeTransport modeTransport;
        private Revendeur revendeur;

        private int employeId;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Commande(int id, ModeTransport modeTransport, Revendeur revendeur, int employeId, DateTime dateCommande, DateTime? dateLivraison)
        {
            Id = id;
            ModeTransport = modeTransport;
            Revendeur = revendeur;
            EmployeId = employeId;
            LigneCommandes = new ObservableCollection<LigneCommande>();
            DateCreation = dateCommande;
            DateLivraison = dateLivraison;
        }

        public Commande()
        {
            DateCreation = DateTime.Now;
            LigneCommandes = new ObservableCollection<LigneCommande>();
        }

        public decimal PrixTotal
        {
            get
            {
                return LigneCommandes.Sum(ligne => ligne.Produit.PrixVente * ligne.Quantite);
            }
        }

        public ModeTransport ModeTransport
        {
            get => modeTransport;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Le mode de transport ne peut pas être nul.");
                }

                modeTransport = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModeTransport)));
            }
        }

        public Revendeur Revendeur
        {
            get => revendeur;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Le revendeur ne peut pas être nul.");
                }

                revendeur = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Revendeur)));
            }
        }

        public int EmployeId { get => employeId; set => employeId = value; }

        public ObservableCollection<LigneCommande> LigneCommandes { get => ligneCommandes; set => ligneCommandes = value; }

        public DateTime DateCreation { 
            get => dateCreation;
            set 
            {
                dateCreation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateCreation)));
            }
        }

        public DateTime? DateLivraison { 
            get => dateLivraison;
            set 
            {
                if (value < DateCreation)
                {
                    throw new ArgumentOutOfRangeException("La date de livraison ne peut pas être antérieure à la date de commande.");
                }

                dateLivraison = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateLivraison)));
            } 
        }

        public static ObservableCollection<Commande> GetFromEmploye(List<ModeTransport> modeTransports, ObservableCollection<Revendeur> revendeurs, ObservableCollection<Produit> produits, Employe employeConnecte)
        {
            Dictionary<int, Commande> commandesParId = new Dictionary<int, Commande>();

            string sql = @"
                SELECT
                    c.NumCommande,
                    c.NumTransport,
                    c.NumRevendeur,
                    c.NumEmploye,
                    c.DateCommande,
                    CASE
                        WHEN c.DateLivraison = '-infinity' THEN NULL
                        ELSE c.DateLivraison
                    END AS DateLivraisonValide,
                    pc.NumProduit,
                    pc.QuantiteCommande
                FROM Commande c
                JOIN ProduitCommande pc ON pc.NumCommande = c.NumCommande
                WHERE c.NumEmploye = @NumEmploye
            ";


            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@NumEmploye", employeConnecte.Id);

            DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);

            foreach (DataRow row in dt.Rows)
            {
                int commandeId = (int)row["NumCommande"];

                if (!commandesParId.ContainsKey(commandeId))
                {
                    ModeTransport? modeTransport = modeTransports.FirstOrDefault(m => m.Id == (int)row["NumTransport"]);
                    Revendeur? revendeur = revendeurs.FirstOrDefault(r => r.Id == (int)row["NumRevendeur"]);

                    int employeId = (int)row["NumEmploye"];

                    DateTime dateCommande = (DateTime)row["DateCommande"];
                    DateTime? dateLivraison = row["DateLivraisonValide"] == DBNull.Value
                                                          ? null
                                                          : (DateTime)row["DateLivraisonValide"];

                    commandesParId[commandeId] = new Commande(
                        id: commandeId,
                        modeTransport: modeTransport,
                        revendeur: revendeur,
                        employeId: employeId,
                        dateCommande: dateCommande,
                        dateLivraison: dateLivraison
                    );
                }

                int quantite = (int)row["QuantiteCommande"];

                Produit? produit = produits.FirstOrDefault(p => p.Id == (int)row["NumProduit"]);
                LigneCommande ligne = new LigneCommande(produit, quantite);

                commandesParId[commandeId].LigneCommandes.Add(ligne);
            }

            return new ObservableCollection<Commande>(commandesParId.Values);
        }

        public void Create()
        {
            string sqlInsertCmd = @"
                INSERT INTO Commande (NumTransport, NumRevendeur, NumEmploye, DateCommande, PrixTotal)
                VALUES (@NumTransport, @NumRevendeur, @NumEmploye, @DateCommande, @PrixTotal) RETURNING NumCommande";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlTransaction transaction = conn.BeginTransaction();
            using NpgsqlCommand cmdInsert = new NpgsqlCommand(sqlInsertCmd, conn, transaction);

            try
            {
                cmdInsert.Parameters.AddWithValue("@NumTransport", ModeTransport.Id);
                cmdInsert.Parameters.AddWithValue("@NumRevendeur", Revendeur.Id);
                cmdInsert.Parameters.AddWithValue("@NumEmploye", EmployeId);
                cmdInsert.Parameters.AddWithValue("@DateCommande", DateCreation);
               /* cmdInsert.Parameters.AddWithValue("@DateLivraison", DateLivraison);*/
                cmdInsert.Parameters.AddWithValue("@PrixTotal", PrixTotal);

                Id = DataAccess.Instance.ExecuteInsert(cmdInsert);

                foreach (LigneCommande ligne in LigneCommandes)
                {
                    ligne.IdCommande = Id;
                    ligne.Create(conn, transaction);
                }

                transaction.Commit();
            } catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public void Update()
        {
            string sqlUpdateCmd = @"
                UPDATE Commande
                SET DateLivraison = @DateLivraison
                WHERE NumCommande = @NumCommande";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmdUpdate = new NpgsqlCommand(sqlUpdateCmd, conn);

            cmdUpdate.Parameters.AddWithValue("@DateLivraison", DateLivraison);
            cmdUpdate.Parameters.AddWithValue("@NumCommande", Id);

            DataAccess.Instance.ExecuteSet(cmdUpdate);
        }

        public void Delete()
        {
            string sqlDeleteCmd = @"
                DELETE FROM Commande
                WHERE NumCommande = @NumCommande";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlTransaction transaction = conn.BeginTransaction();
            using NpgsqlCommand cmdDelete = new NpgsqlCommand(sqlDeleteCmd, conn, transaction);

            try
            {
                cmdDelete.Parameters.AddWithValue("@NumCommande", Id);
                DataAccess.Instance.ExecuteSet(cmdDelete);

                foreach (LigneCommande ligne in LigneCommandes)
                {
                    ligne.Delete(conn, transaction);
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
