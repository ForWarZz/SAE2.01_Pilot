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
    public class TypePointe
    {
        public int Id { get; set; }
        public string Libelle { get; set; }

        public TypePointe(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }

        public TypePointe(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Libelle;
        }

        public static List<TypePointe> GetAll()
        {
            List<TypePointe> typePointes = new List<TypePointe>();

            string sql = "SELECT * FROM TypePointe";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

            DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
            foreach (DataRow row in dt.Rows)
            {
                typePointes.Add(new TypePointe((int)row["NumTypePointe"], row["LibelleTypePointe"].ToString()));
            }

            return typePointes;
        }
    }
}
