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
    public class TypeProduit
    {
        public int Id { get; set; }
        public string Libelle { get; set; }
        public CategorieProduit Categorie { get; set; }

        public TypeProduit(int id, string libelle, CategorieProduit categorie)
        {
            Id = id;
            Libelle = libelle;
            Categorie = categorie;
        }

        public TypeProduit(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }

        public override string ToString()
        {
            return Libelle;
        }

        public static List<TypeProduit> GetAll(List<CategorieProduit> categorieProduits)
        {
            List<TypeProduit> typeProduits = new List<TypeProduit>();

            string sql = "SELECT * FROM type";

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);

            DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
            foreach (DataRow dr in dt.Rows)
            {
                CategorieProduit? categorie = categorieProduits.FirstOrDefault(c => c.Id == (int)dr["NumCategorie"]);
                typeProduits.Add(new TypeProduit((int)dr["NumType"], dr["LibelleType"].ToString(), categorie));
            }

            return typeProduits;
        }
    }
}
