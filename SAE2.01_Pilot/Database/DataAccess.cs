using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;

namespace TD3_BindingBDPension.Model
{
    public class DataAccess
    {
        private static readonly DataAccess instance = new DataAccess();
        private readonly string connectionString = "Host=srv-peda-new;Port=5433;Username=benardax;Password=E3ES16;Database=SAE201_BM_BA;Options='-c search_path=pilot'";

        public static DataAccess Instance
        {
            get
            {
                return instance;
            }
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
                // Assurez-vous de fermer la connexion en cas d'erreur lors de l'ouverture
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
    }
}