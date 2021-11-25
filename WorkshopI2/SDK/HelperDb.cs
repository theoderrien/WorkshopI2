using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopI2.SDK
{
    class HelperDb
    {
        public static DataTable GetDatatable(MySqlConnection dsn, string requete)
        {
            DataTable table = new DataTable();

            dsn.Open();
            MySqlCommand commande = new MySqlCommand
            {
                Connection = dsn,
                CommandText = requete
            };
            commande.ExecuteNonQuery();

            MySqlDataAdapter adapter = new MySqlDataAdapter(commande);
            adapter.Fill(table);

            return table;
        }
    }
}
