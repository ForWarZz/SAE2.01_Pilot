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

        public Employe(int id, string nom, string prenom, Role role)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Role = role;
        }

        public bool EstCommercial => Role.Libelle == "Commercial";
        public bool EstResponsableProduction => Role.Libelle == "Responsable de production";

/*        public bool EstCommercial => true;
        public bool EstResponsableProduction => true;*/

        public static Employe? FindByIdentifiant(string login)
        {
            string sql = "SELECT * FROM Employe e " +
                        "JOIN Role r ON r.NumRole = e.NumRole " +
                        "WHERE e.login = @login;";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmdSelect = new NpgsqlCommand(sql, conn);
            
            cmdSelect.Parameters.AddWithValue("login", login);

            DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
            if (dt.Rows.Count <= 0)
                return null;

            DataRow dataRow = dt.Rows[0];

            Role role = new Role((int)dataRow["NumRole"], dataRow["LibelleRole"].ToString());

            return new Employe(
                (int)dataRow["NumEmploye"], 
                dataRow["Prenom"].ToString(), 
                dataRow["Nom"].ToString(), 
                role);
           
        }
    }
}
