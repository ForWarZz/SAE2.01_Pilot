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
    public class Commande : ICrud<Commande>
    {
        private ObservableCollection<LigneCommande> ligneCommandes;
        private DateTime dateCommande;
        private DateTime dateLivraison;

        public int Id { get; set; }
        public ModeTransport ModeTransport { get; set; }
        public Revendeur Revendeur { get; set; }
        public int EmployeId { get; set; }
        public ObservableCollection<LigneCommande> LigneCommandes { get => ligneCommandes; set => ligneCommandes = value; }
        public DateTime DateCommande { get => dateCommande; set => dateCommande = value; }
        public DateTime DateLivraison { get => dateLivraison; set => dateLivraison = value; }

        public Commande(int id, ModeTransport modeTransport, Revendeur revendeur, int employeId, DateTime dateCommande, DateTime dateLivraison)
        {
            Id = id;
            ModeTransport = modeTransport;
            Revendeur = revendeur;
            EmployeId = employeId;
            LigneCommandes = new ObservableCollection<LigneCommande>();
            DateCommande = dateCommande;
            DateLivraison = dateLivraison;
        }

        public Commande()
        {
            DateCommande = DateTime.Now;
        }

        public decimal PrixTotal
        {
            get
            {
                return (decimal)LigneCommandes.Sum(ligne => ligne.Produit.PrixVente * ligne.Quantite);
            }
        }

        public static ObservableCollection<Commande> GetAll(List<ModeTransport> modeTransports, ObservableCollection<Revendeur> revendeurs, ObservableCollection<Produit> produits)
        {
            Dictionary<int, Commande> commandesParId = new Dictionary<int, Commande>();

            string sql = @"
                SELECT 
                    c.Id AS CommandeId,
                    c.NumTransport,
                    c.NumRevendeur,
                    c.NumEmploye,
                    c.DateCommande,
                    c.DateLivraison,
                    lc.NumProduit,
                    lc.QuantiteCommande,
                FROM Commande c
                JOIN LigneCommande lc ON lc.IdCommande = c.Id
                ORDER BY c.Id
            ";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
            {
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
                        DateTime dateLivraison = (DateTime)row["DateLivraison"];

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
            }

            return new ObservableCollection<Commande>(commandesParId.Values.ToList());
        }

        public void Create()
        {
            string sqlInsertCmd = @"
                INSERT INTO Commande (NumTransport, NumRevendeur, EmployeId, DateCommande, DateLivraison, PrixTotal)
                VALUES (@NumTransport, @NumRevendeur, @EmployeId, @DateCommande, @DateLivraison, @PrixTotal) RETURNING NumCommande";

            using (NpgsqlCommand cmdInsert = new NpgsqlCommand(sqlInsertCmd))
            {
                cmdInsert.Parameters.AddWithValue("@NumTransport", ModeTransport.Id);
                cmdInsert.Parameters.AddWithValue("@NumRevendeur", Revendeur.Id);
                cmdInsert.Parameters.AddWithValue("@EmployeId", EmployeId);
                cmdInsert.Parameters.AddWithValue("@DateCommande", DateCommande);
                cmdInsert.Parameters.AddWithValue("@DateLivraison", DateLivraison);
                cmdInsert.Parameters.AddWithValue("@PrixTotal", PrixTotal);

                Id = DataAccess.Instance.ExecuteInsert(cmdInsert);

                foreach (LigneCommande ligne in LigneCommandes)
                {
                    ligne.IdCommande = Id;
                    ligne.Create();
                }
            }
        }

        public void Update()
        {
            string sqlUpdateCmd = @"
                UPDATE Commande
                SET DateLivraison = @DateLivraison
                WHERE NumCommande = @NumCommande";

            using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(sqlUpdateCmd))
            {
                cmdUpdate.Parameters.AddWithValue("@DateLivraison", DateLivraison);
                cmdUpdate.Parameters.AddWithValue("@NumCommande", Id);

                DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
