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
    public class ModeTransport : ICrud<ModeTransport>
    {
        public int Id { get; set; }
        public string Libelle { get; set; }

        public ModeTransport(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }

        public ModeTransport(int id)
        {
            Id = id;
        }

        public static List<ModeTransport> GetAll()
        {
            List<ModeTransport> modesTransport = new List<ModeTransport>();

            string sql = "SELECT * FROM ModeTransport";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(sql))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dataRow in dt.Rows)
                {
                    int id = (int)dataRow["NumTransport"];
                    string libelle = (string)dataRow["LibelleTransport"];

                    modesTransport.Add(new ModeTransport(id, libelle));
                }
            }

            return modesTransport;
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
