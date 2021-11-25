using MySql.Data.MySqlClient;
using ProjetWorkshop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkshopI2.SDK;

namespace WorkshopI2.Queries
{
    class ChiffrementDAL
    {
        private static BDD_connexion bddConnection = new BDD_connexion();
        private static MySqlConnection connection = bddConnection.Connection;

        public static string[] SelectAll()
        {
            string[] chiffrements = new string[3];
            string requete = "SELECT methode as 'Methode' FROM chiffrement";

            try
            {
                DataTable table = HelperDb.GetDatatable(connection, requete);
                if(table != null && table.Rows.Count > 0)
                {
                    for(int i = 0; i< table.Rows.Count; i++)
                    {
                        chiffrements[i] = table.Rows[i]["Methode"].ToString();
                    }
                }
                return chiffrements;
            }
            catch(Exception exc)
            {
                throw exc;
            }

        }

        public static bool DeleteAll()
        {
            connection.Open();
            string requete = "TRUNCATE chiffrement";
            MySqlCommand cmd = new MySqlCommand(requete, connection);
            int result = cmd.ExecuteNonQuery() ;
            connection.Close();
            return result > 0;
        }

        public static bool InsertChiffrement()
        {
            try
            {
                foreach (string methode in Globals.chiffrements)
                {
                    connection.Open();
                    string requete = $"INSERT INTO `chiffrement` (`id`, `methode`, `date`) VALUES (NULL, '{methode}', CURRENT_TIMESTAMP);";
                    MySqlCommand cmd = new MySqlCommand(requete, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                return true;
            }
            catch(Exception exc)
            {
                return false;
            }
            
        }
    }
}
