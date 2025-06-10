using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Libelle { get; set; }

        public Role(int id, string libelle)
        {
            Id = id;
            Libelle = libelle;
        }
    }
}
