using System;
using System.Collections.ObjectModel;
using System.Data;
using MySql.Data.MySqlClient;
using WorkshopI2.Models;
using WorkshopI2.SDK;

namespace ProjetWorkshop.Queries
{
    public class PersonneDAL
    {
        private static BDD_connexion bddConnection = new BDD_connexion();
        private static MySqlConnection connection = bddConnection.Connection;

        //Requête SQL pour sélectionner tous les aéroports dans la base de données.
        public static void SelectAllPersonne(ObservableCollection<PersonneModel> ObsColPersonne)
        {
            connection.Close();
            connection.Open();
            string query = "SELECT personne.id,personne.nom,personne.prenom,personne.date_naissance FROM personne Group BY personne.id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                PersonneModel personne = new PersonneModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3));
                ObsColPersonne.Add(personne);
            }
            reader.Close();
            connection.Close();
        }

        public static DataTable SelectAllPersonne()
        {
            string requete = "SELECT personne.id , personne.nom FROM personne Group BY personne.id;";

            return HelperDb.GetDatatable(connection, requete);
        }

        //Requête SQL pour mettre à jour une personne dans la base de données.
        public static void UpdatePersonne(PersonneModel personne)
        {
            connection.Open();
            string query = "UPDATE personne SET nom=\"" + personne.Nom + "\",prenom=\"" + personne.Prenom + "\",date_naissance=\"" + personne.DateNaissance + "\" WHERE id=" + personne.Id + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //MySqlDataAdapter sqlDataAdap = new MySqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Requête SQL pour insérer une nouvelle personne dans la base de données.
        public static void InsertPersonne(string nom, string prenom, DateTime dateNaissance )
        {
            connection.Open();
            string query = "INSERT INTO personne(nom, prenom, date_naissance) VALUES(@nom,@prenom,@date_naissance)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@prenom", prenom);
            cmd.Parameters.AddWithValue("@date_naissance", dateNaissance);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Requête SQL pour supprimer une personne de la base de données.
        public static void DeletePersonne(int id)
        {
            connection.Open();
            string query = "DELETE FROM personne WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        //Requête SQL pour récupérer l'id de la dernière personne dans la base de données.
        public static int GetLastId()
        {
            int lastId = new int();

            connection.Open();
            string query = "SELECT id FROM personne ORDER BY id DESC limit 1";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lastId = Convert.ToInt32(reader["id"]);
            }
            reader.Close();
            connection.Close();

            return lastId;
        }
    }
}
