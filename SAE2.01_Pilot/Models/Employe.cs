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
        public Role Role { get; set; }

        public Employe()
        {
            
        }

        public Employe(int id, string nom, string prenom, Role role)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Role = role;
        }

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
                Role role = new Role((int)dataRow["NumRole"], Enum.Parse<LibelleRole>(dataRow["LibelleRole"].ToString()));

                return new Employe
                {
                    Id = (int)dataRow["NumEmploye"],
                    Prenom = dataRow["Prenom"].ToString(),
                    Nom = dataRow["Nom"].ToString(),
                    Role = role
                };
            }
        }
    }
}
