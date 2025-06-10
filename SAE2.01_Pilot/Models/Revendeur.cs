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
    public class Revendeur
    {
        private string raisonSociale;

        public int Id { get; set; }
        public Adresse Adresse { get; set; }
        public string RaisonSociale { 
            get => raisonSociale;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("La raison sociale ne peut pas être vide.");
                }

                raisonSociale = value;
            }
        }

        public Revendeur(int id, string raisonSociale, Adresse adresse)
        {
            Id = id;
            RaisonSociale = raisonSociale;
            Adresse = adresse;
        }

        public Revendeur(int id, string raisonSociale)
        {
            Id = id;
            RaisonSociale = raisonSociale;
        }

        public Revendeur()
        {
            Adresse = new Adresse();
        }

        public static ObservableCollection<Revendeur> GetAll()
        {
            ObservableCollection<Revendeur> revendeurs = new ObservableCollection<Revendeur>();
            string sql = "SELECT * FROM Revendeur";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dataRow in dt.Rows)
                {
                    int id = (int)dataRow["NumRevendeur"];
                    string raisonSociale = dataRow["RaisonSociale"].ToString();

                    Adresse adresse = new Adresse(
                        dataRow["AdresseRue"].ToString(),
                        dataRow["AdresseCP"].ToString(),
                        dataRow["AdresseVille"].ToString()
                    );

                    revendeurs.Add(new Revendeur(id, raisonSociale, adresse));
                }

                return revendeurs;
            }
        }

        public void Create()
        {
            string sql = "INSERT INTO Revendeur (RaisonSociale, AdresseRue, AdresseCP, AdresseVille) " +
                         "VALUES (@RaisonSociale, @AdresseRue, @AdresseCP, @AdresseVille) RETURNING NumRevendeur";

            using (NpgsqlCommand cmdInsert = new NpgsqlCommand(sql))
            {
                cmdInsert.Parameters.AddWithValue("@RaisonSociale", RaisonSociale);
                cmdInsert.Parameters.AddWithValue("@AdresseRue", Adresse.Rue);
                cmdInsert.Parameters.AddWithValue("@AdresseCP", Adresse.CodePostal);
                cmdInsert.Parameters.AddWithValue("@AdresseVille", Adresse.Ville);

                Id = DataAccess.Instance.ExecuteInsert(cmdInsert);

                Console.WriteLine("Revendeur créé avec succès. ID: " + Id);
            }
        }

        public void Update()
        {
            string sql = "UPDATE Revendeur SET RaisonSociale = @raisonSociale, " +
                         "AdresseRue = @AdresseRue, AdresseCP = @AdresseCP, AdresseVille = @AdresseVille " +
                         "WHERE NumRevendeur = @Id";

            using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(sql))
            {
                cmdUpdate.Parameters.AddWithValue("@Id", Id);
                cmdUpdate.Parameters.AddWithValue("@RaisonSociale", RaisonSociale);
                cmdUpdate.Parameters.AddWithValue("@AdresseRue", Adresse.Rue);
                cmdUpdate.Parameters.AddWithValue("@AdresseCP", Adresse.CodePostal);
                cmdUpdate.Parameters.AddWithValue("@AdresseVille", Adresse.Ville);
                
                DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
