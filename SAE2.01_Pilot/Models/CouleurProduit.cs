
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
    public class CouleurProduit
    {
        public int Id { get; set; }
        public string Libelle { get; set; }

        public CouleurProduit(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }

        public static List<CouleurProduit> GetAll()
        {
            List<CouleurProduit> couleurProduits = new List<CouleurProduit>();

            string sql = "SELECT * FROM Couleur";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

            DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
            foreach (DataRow dr in dt.Rows)
            {
                couleurProduits.Add(new CouleurProduit(
                    (int)dr["NumCouleur"],
                    dr["LibelleCouleur"].ToString()
                ));
            }

            return couleurProduits;

        }

        public override bool Equals(object? obj)
        {
            return obj is CouleurProduit produit &&
                   Id == produit.Id;
        }
    }
}
