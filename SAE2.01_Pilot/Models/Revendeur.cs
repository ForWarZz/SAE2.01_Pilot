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
    public class Revendeur : ICrud<Revendeur>
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

        public Revendeur(string raisonSociale, Adresse adresse)
        {
            RaisonSociale = raisonSociale;
            Adresse = adresse;
        }

        public Revendeur()
        {
            Adresse = new Adresse();
        }

        public static List<Revendeur> GetAll()
        {
            List<Revendeur> revendeurs = new List<Revendeur>();
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
                         "VALUES (@raisonSociale, @adresseRue, @adresseCP, @adresseVille) RETURNING NumRevendeur";

            using (NpgsqlCommand cmdInsert = new NpgsqlCommand(sql))
            {
                cmdInsert.Parameters.AddWithValue("@raisonSociale", RaisonSociale);
                cmdInsert.Parameters.AddWithValue("@adresseRue", Adresse.Rue);
                cmdInsert.Parameters.AddWithValue("@adresseCP", Adresse.CodePostal);
                cmdInsert.Parameters.AddWithValue("@adresseVille", Adresse.Ville);

                Id = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
        }

        public void Update()
        {
            string sql = "UPDATE Revendeur SET RaisonSociale = @raisonSociale, " +
                         "AdresseRue = @adresseRue, AdresseCP = @adresseCP, AdresseVille = @adresseVille " +
                         "WHERE NumRevendeur = @id";

            using (NpgsqlCommand cmdUpdate = new NpgsqlCommand(sql))
            {
                cmdUpdate.Parameters.AddWithValue("@id", Id);
                cmdUpdate.Parameters.AddWithValue("@raisonSociale", RaisonSociale);
                cmdUpdate.Parameters.AddWithValue("@adresseRue", Adresse.Rue);
                cmdUpdate.Parameters.AddWithValue("@adresseCP", Adresse.CodePostal);
                cmdUpdate.Parameters.AddWithValue("@adresseVille", Adresse.Ville);
                
                DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
