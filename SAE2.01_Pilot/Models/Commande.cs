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
        private DateTime dateCreation;
        private DateTime dateLivraison;

        public int Id { get; set; }
        public ModeTransport ModeTransport { get; set; }
        public Revendeur Revendeur { get; set; }
        public int EmployeId { get; set; }
        public ObservableCollection<LigneCommande> LigneCommandes { get => ligneCommandes; set => ligneCommandes = value; }
        public DateTime DateCreation { get => dateCreation; set => dateCreation = value; }
        public DateTime DateLivraison { get => dateLivraison; set => dateLivraison = value; }

        public Commande(int id, ModeTransport modeTransport, Revendeur revendeur, int employeId, DateTime dateCommande, DateTime dateLivraison)
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
        }

        public decimal PrixTotal
        {
            get
            {
                return LigneCommandes.Sum(ligne => ligne.Produit.PrixVente * ligne.Quantite);
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
                    c.DateLivraison,
                    pc.NumProduit,
                    pc.QuantiteCommande
                FROM Commande c
                JOIN ProduitCommande pc ON pc.NumCommande = c.NumCommande
                WHERE c.NumEmploye = @NumEmploye
            ";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@NumEmploye", employeConnecte.Id);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);

                foreach (DataRow row in dt.Rows)
                {
                    int commandeId = (int)row["NumCommande"];

                    Console.WriteLine("Commande id " + commandeId);

                    if (!commandesParId.ContainsKey(commandeId))
                    {
                        ModeTransport? modeTransport = modeTransports.FirstOrDefault(m => m.Id == (int)row["NumTransport"]);

                        Console.WriteLine("Mode de transdport " + modeTransport.Libelle);
                        Console.WriteLine("Revendeur id : " + (int)row["NumRevendeur"]);

                        Revendeur? revendeur = revendeurs.FirstOrDefault(r => r.Id == (int)row["NumRevendeur"]);

                        Console.WriteLine("Revendeur : " + revendeur.Adresse.Rue);
                        
                        int employeId = (int)row["NumEmploye"];

                        Console.WriteLine("Employe " + employeId);

                        DateTime dateCommande = (DateTime)row["DateCommande"];
                        DateTime dateLivraison = (DateTime)row["DateLivraison"];

                        Console.WriteLine("date commande " + dateCommande);
                        Console.WriteLine("date livraison " + dateLivraison);

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

                    Console.WriteLine("Quantite : " + quantite);

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
                cmdInsert.Parameters.AddWithValue("@DateCommande", DateCreation);
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
