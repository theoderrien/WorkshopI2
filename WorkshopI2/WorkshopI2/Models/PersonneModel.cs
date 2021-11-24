using System;

namespace WorkshopI2.Models
{
    public class PersonneModel
    {
        public int Id { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public DateTime DateNaissance { get; set; }

        public PersonneModel(int id, string nom, string prenom, DateTime dateNaissance)
        {
            this.Id = id;
            this.Nom = nom;
            this.Prenom = prenom;
            this.DateNaissance = dateNaissance;
        }
    }
}
