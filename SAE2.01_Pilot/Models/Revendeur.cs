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
    public class Revendeur : ICrud<Revendeur>, INotifyPropertyChanged
    {
        private string raisonSociale;
        private Adresse adresse;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id { get; set; }
        public Adresse Adresse
        {
            get => adresse;
            set
            {
                if (value == null)
                    throw new ArgumentNullException("L'adresse ne peut pas être vide.");

                adresse = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Adresse)));
            }
        }
        public string RaisonSociale { 
            get => raisonSociale;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("La raison sociale ne peut pas être vide.");
                }

                raisonSociale = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RaisonSociale)));
            }
        }

        public Revendeur(string raisonSociale, Adresse adresse)
        {
            RaisonSociale = raisonSociale;
            Adresse = adresse;
        }

        public Revendeur(int id, string raisonSociale, Adresse adresse) :
            this(raisonSociale, adresse)
        {
            Id = id;
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

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmdSelect = new NpgsqlCommand(sql, conn);

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

        public void Read()
        {
            string sql = "SELECT * FROM Revendeur WHERE NumRevendeur = @NumRevendeur";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmdSelect = new NpgsqlCommand(sql, conn);

            cmdSelect.Parameters.AddWithValue("@NumRevendeur", Id);

            DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
            if (dt.Rows.Count == 0)
                throw new InvalidOperationException("Revendeur non trouvé.");

            DataRow dataRow = dt.Rows[0];
            RaisonSociale = dataRow["RaisonSociale"].ToString();
            Adresse = new Adresse(
                dataRow["AdresseRue"].ToString(),
                dataRow["AdresseCP"].ToString(),
                dataRow["AdresseVille"].ToString()
            );

            Id = (int)dataRow["NumRevendeur"];
        }

        public void Create()
        {
            string sqlCheckExists = @"SELECT COUNT(*) FROM Revendeur WHERE RaisonSociale = @RaisonSociale AND
                                    AdresseRue = @AdresseRue AND AdresseCP = @AdresseCP AND AdresseVille = @AdresseVille";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmdCheckExists = new NpgsqlCommand(sqlCheckExists, conn);

            cmdCheckExists.Parameters.AddWithValue("@RaisonSociale", RaisonSociale);
            cmdCheckExists.Parameters.AddWithValue("@AdresseRue", Adresse.Rue);
            cmdCheckExists.Parameters.AddWithValue("@AdresseCP", Adresse.CodePostal);
            cmdCheckExists.Parameters.AddWithValue("@AdresseVille", Adresse.Ville);

            int count = (int)(Int64)DataAccess.Instance.ExecuteSelectUneValeur(cmdCheckExists);

            if (count > 0)
                throw new InvalidOperationException("Un revendeur avec les mêmes informations existe déjà.");

            string sql = "INSERT INTO Revendeur (RaisonSociale, AdresseRue, AdresseCP, AdresseVille) " +
                         "VALUES (@RaisonSociale, @AdresseRue, @AdresseCP, @AdresseVille) RETURNING NumRevendeur";

            using NpgsqlCommand cmdInsert = new NpgsqlCommand(sql, conn);

            cmdInsert.Parameters.AddWithValue("@RaisonSociale", RaisonSociale);
            cmdInsert.Parameters.AddWithValue("@AdresseRue", Adresse.Rue);
            cmdInsert.Parameters.AddWithValue("@AdresseCP", Adresse.CodePostal);
            cmdInsert.Parameters.AddWithValue("@AdresseVille", Adresse.Ville);

            Id = DataAccess.Instance.ExecuteInsert(cmdInsert);
        }

        public void Update()
        {
            string sqlCheckExists = @"SELECT COUNT(*) FROM Revendeur WHERE RaisonSociale = @RaisonSociale AND
                                    AdresseRue = @AdresseRue AND AdresseCP = @AdresseCP AND AdresseVille = @AdresseVille AND
                                    NumRevendeur != @NumRevendeur";


            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmdCheckExists = new NpgsqlCommand(sqlCheckExists, conn);

            cmdCheckExists.Parameters.AddWithValue("@RaisonSociale", RaisonSociale);
            cmdCheckExists.Parameters.AddWithValue("@AdresseRue", Adresse.Rue);
            cmdCheckExists.Parameters.AddWithValue("@AdresseCP", Adresse.CodePostal);
            cmdCheckExists.Parameters.AddWithValue("@AdresseVille", Adresse.Ville);
            cmdCheckExists.Parameters.AddWithValue("@NumRevendeur", Id);

            int count = (int)(Int64)DataAccess.Instance.ExecuteSelectUneValeur(cmdCheckExists);

            if (count > 0)
                throw new InvalidOperationException("Un revendeur avec les mêmes informations existe déjà.");
            

            string sql = "UPDATE Revendeur SET RaisonSociale = @raisonSociale, " +
                         "AdresseRue = @AdresseRue, AdresseCP = @AdresseCP, AdresseVille = @AdresseVille " +
                         "WHERE NumRevendeur = @Id";

            using NpgsqlCommand cmdUpdate = new NpgsqlCommand(sql, conn);

            cmdUpdate.Parameters.AddWithValue("@Id", Id);
            cmdUpdate.Parameters.AddWithValue("@RaisonSociale", RaisonSociale);
            cmdUpdate.Parameters.AddWithValue("@AdresseRue", Adresse.Rue);
            cmdUpdate.Parameters.AddWithValue("@AdresseCP", Adresse.CodePostal);
            cmdUpdate.Parameters.AddWithValue("@AdresseVille", Adresse.Ville);
                
            DataAccess.Instance.ExecuteSet(cmdUpdate);
            
        }

        public void Delete()
        {
            throw new NotImplementedException("Method not implemented");
        }
    }
}
