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
    public class TypeProduit : ICrud<TypeProduit>
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

        public TypeProduit(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Libelle;
        }

        public static List<TypeProduit> GetAll()
        {
            List<TypeProduit> typeProduits = new List<TypeProduit>();

            string sql = @"
                    SELECT * FROM TypeProduit tp
                    JOIN Type t
                    ON tp.NumType = t.NumType
                ";

            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmd);
                foreach (DataRow dr in dt.Rows)
                {
                    CategorieProduit categorie = new CategorieProduit((int)dr["NumCategorie"]);
                    typeProduits.Add(new TypeProduit((int)dr["NumType"], dr["LibelleType"].ToString(), categorie));
                }

                return typeProduits;
            }
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
