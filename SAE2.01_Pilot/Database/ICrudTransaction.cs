using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Database
{
    public interface ICrudTransaction<T>
    {
        public void Create(NpgsqlConnection conn, NpgsqlTransaction transaction);
        public void Update(NpgsqlConnection conn, NpgsqlTransaction transaction);
        public void Delete(NpgsqlConnection conn, NpgsqlTransaction transaction);
    }
}
