using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkshopI2.Models
{
    public class VoitureModel
    {
        public int Id { get; set; }

        public string Marque { get; set; }

        public string Modele { get; set; }

        public string Couleur { get; set; }

        public VoitureModel(int id, string marque, string modele, string couleur)
        {
            this.Id = id;
            this.Marque= marque;
            this.Modele = modele;
            this.Couleur = couleur;
        }
    }
}
