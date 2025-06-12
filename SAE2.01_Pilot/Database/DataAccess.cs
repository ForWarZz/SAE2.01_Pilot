using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;
using System.IO;

namespace TD3_BindingBDPension.Model
{
    public class DataAccess
    {
        private static readonly DataAccess instance = new DataAccess();
        private string connectionString;

        public static readonly string PROD_SCHEMA = "prod";
        private static readonly string TEST_SCHEMA = "test";

        private static readonly string TEST_SQL_PATH = "SQL/sql.sql";

        public static readonly string TEST_LOGIN = "battigm";
        private static readonly string TEST_PASSWORD = "123";

        public static DataAccess Instance
        {
            get
            {
                return instance;
            }
        }

        public void SetCredentials(string username, string password, string schema)
        {
            connectionString = $"Host=localhost;Port=5432;Username={username};Password={password};Database=sae201;Options='-c search_path={schema}'";
        }

        // Pour récupérer une NOUVELLE connexion ouverte
        public NpgsqlConnection GetOpenedConnection()
        {
            NpgsqlConnection conn = null;
            try
            {
                conn = new NpgsqlConnection(connectionString);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb de connexion GetOpenedConnection \n" + connectionString);
                conn?.Close();
                throw;
            }
        }

        // Pour requêtes SELECT et retourne un DataTable (table de données en mémoire)
        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL dans ExecuteSelect pour la requête: " + cmd.CommandText);
                throw;
            }
            return dataTable;
        }

        // Pour requêtes INSERT et renvoie l'ID généré
        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                nb = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requête insert: " + cmd.CommandText);
                throw;
            }
            return nb;
        }

        public int ExecuteInsertSansRetour(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                nb = cmd.ExecuteNonQuery();
                if (nb == 0)
                {
                    throw new InvalidOperationException("Aucune ligne insérée.");
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requête insert: " + cmd.CommandText);
                throw;
            }
            return nb;
        }

        // Pour requêtes UPDATE, DELETE
        public int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requête set: " + cmd.CommandText);
                throw;
            }
            return nb;
        }

        // Pour requêtes avec une seule valeur retour (ex : COUNT, SUM)
        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                res = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requête select unique valeur: " + cmd.CommandText);
                throw;
            }
            return res;
        }

        public bool TesterConnexion()
        {
            try
            {
                using NpgsqlConnection conn = GetOpenedConnection();
                return true;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Échec de la connexion dans TesterConnexion");
                return false;
            }
        }

        public void SetupTestBDD()
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TEST_SQL_PATH);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(fullPath);
            }

            string sqlScript = File.ReadAllText(fullPath);
            SetCredentials(TEST_LOGIN, TEST_PASSWORD, TEST_SCHEMA);

            using NpgsqlConnection conn = DataAccess.Instance.GetOpenedConnection();
            using NpgsqlCommand cmdSetup = new NpgsqlCommand(sqlScript, conn);

            cmdSetup.ExecuteNonQuery();
        }
    }
}