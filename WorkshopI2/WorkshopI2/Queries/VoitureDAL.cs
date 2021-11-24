using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using WorkshopI2.Models;

namespace ProjetWorkshop.Queries
{
    public class VoitureDAL
    {
        private static BDD_connexion bddConnection = new BDD_connexion();
        private static MySqlConnection connection = bddConnection.Connection;

        //Requête SQL pour sélectionner toutes les voitures dans la base de données.
        public static void SelectAllVoiture(ObservableCollection<VoitureModel> ObsColVoiture)
        {
            connection.Close();
            connection.Open();
            string query = "SELECT voiture.id,voiture.marque,voiture.modele,voiture.couleur FROM voiture Group BY voiture.id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                VoitureModel voiture = new VoitureModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                ObsColVoiture.Add(voiture);
            }
            reader.Close();
            connection.Close();
        }

        //Requête SQL pour mettre à jour une voiture dans la base de données.
        public static void UpdateVoiture(VoitureModel voiture)
        {
            connection.Open();
            string query = "UPDATE voiture SET marque=\"" + voiture.Marque + "\",modele=\"" + voiture.Marque + "\",couleur=\"" + voiture.Couleur + "\" WHERE id=" + voiture.Id + "";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            //MySqlDataAdapter sqlDataAdap = new MySqlDataAdapter(cmd);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Requête SQL pour insérer une nouvelle voiture dans la base de données.
        public static void InsertVoiture(string nom, string prenom, DateTime dateNaissance)
        {
            connection.Open();
            string query = "INSERT INTO voiture(marque, modele, couleur) VALUES(@marque,@modele,@couleur)";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@marque", nom);
            cmd.Parameters.AddWithValue("@modele", prenom);
            cmd.Parameters.AddWithValue("@couleur", dateNaissance);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //Requête SQL pour supprimer une voiture de la base de données.
        public static void DeleteVoiture(int id)
        {
            connection.Open();
            string query = "DELETE FROM voiture WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        //Requête SQL pour récupérer l'id de la dernière voiture dans la base de données.
        public static int GetLastId()
        {
            int lastId = new int();

            connection.Open();
            string query = "SELECT id FROM voiture ORDER BY id DESC limit 1";
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
