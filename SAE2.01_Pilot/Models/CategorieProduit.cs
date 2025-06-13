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
    public class CategorieProduit
    {
        public int Id { get; set; }
        public string Libelle { get; set; }

        public CategorieProduit(int id, string libelle) : this(id)
        {
            Libelle = libelle;
        }

        public CategorieProduit(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Libelle;
        }

        public static List<CategorieProduit> GetAll()
        {
            List<CategorieProduit> categories = new List<CategorieProduit>();

            string sql = "SELECT NumCategorie, LibelleCategorie FROM Categorie";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

            DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
            foreach (DataRow row in dt.Rows)
            {
                int id = (int)row["NumCategorie"];
                string libelle = row["LibelleCategorie"].ToString();

                categories.Add(new CategorieProduit(id, libelle));
            }

            return categories;
        }
    }
}
