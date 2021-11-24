using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows;

namespace ProjetWorkshop
{
    public class BDD_connexion
    {
        private MySqlConnection _connection;
        public MySqlConnection Connection
        {
            get { return _connection; }
        }


        public BDD_connexion()
        {
            this.InitConnexion();
        }

        private void InitConnexion()
        {
            //Création de la chaine de connexion.
            try
            {
                this._connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["localhost"].ConnectionString);
            }
            catch (InvalidCastException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
