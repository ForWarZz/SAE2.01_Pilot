using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD3_BindingBDPension.Model;

namespace SAE2._01_Pilot.Models
{
    public class Employe
    {
        public int Id { get; set; }
        public string Nom {  get; set; }
        public string Prenom { get; set; }
        public LibelleRole Role { get; set; }

        public Employe(int id, string nom, string prenom, LibelleRole role)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Role = role;
        }

        public Employe(int id, string nom, string prenom, string role)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Role = RoleHelper.LibelleRoleParNom(role);
        }

        public bool EstCommercial => Role == LibelleRole.Commercial;
        public bool EstResponsableProduction => Role == LibelleRole.ResponsableProduction;

        public static Employe? FindByCredentials(string login, string password)
        {
            string sql = "SELECT * FROM Employe e " +
                        "JOIN Role r ON r.NumRole = e.NumRole " +
                        "WHERE e.login = @login AND e.password = @password;";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                cmdSelect.Parameters.AddWithValue("login", login);
                cmdSelect.Parameters.AddWithValue("password", password);

                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count <= 0)
                    return null;

                DataRow dataRow = dt.Rows[0];

                return new Employe(
                    (int)dataRow["NumEmploye"], 
                    dataRow["Prenom"].ToString(), 
                    dataRow["Nom"].ToString(), 
                    dataRow["LibelleRole"].ToString());
            }
        }
    }
}
