using ProjetWorkshop;
using ProjetWorkshop.Queries;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using WorkshopI2.Command;
using WorkshopI2.Queries;
using WorkshopI2.SDK;

namespace WorkshopI2
{
    public class ChiffrementViewModel
    {
        //Commande pour insérer dans la base de données.
        #region SubmitCommand
        private ICommand _SubmitCommand;

        public ICommand SubmitCommand
        {
            get
            {
                if (_SubmitCommand == null)
                {
                    _SubmitCommand = new RelayCommand(SubmitExecute, CanSubmitExecute, false);
                }
                return _SubmitCommand;
            }
        }

        //Ce qui est exécuté selon si la condition est vraie ou fausse.
        private void SubmitExecute(object parameter)
        {
            //HelperHash.AleatoireChiffrement();
            //VolDAL.InsertVol(NewAeroportDepart, NewAeroportArrivee, NewAvion, NewDepartTheorique, NewDepartReel, NewArriveeTheorique, NewArriveeReelle, NewPrixEco, NewPrixBusiness, NewPrixPremium);
            string[] chiffrements = ChiffrementDAL.SelectAll();

            DataTable resultat = PersonneDAL.SelectAllPersonne();
            string donnee;

            foreach (DataRow row in resultat.Rows)
            {
                if (chiffrements.Length > 0 && chiffrements[0] != null)
                {
                    donnee = HelperHash.DecryptData(row["nom"].ToString(), chiffrements);
                    donnee = HelperHash.EncryptData(donnee.ToString(), Globals.chiffrements);

                    
                }
                else
                {
                    donnee = HelperHash.EncryptData(row["nom"].ToString(), Globals.chiffrements);
                }

                try
                {
                    HelperConfig helper = new HelperConfig();
                    string test = string.Format(helper.RequeteAutoGenere("personne"), donnee, row["id"].ToString());
                    PersonneDAL.UpdatePersonne(test);
                }
                catch (Exception exc) { throw exc; }
            }


            ChiffrementDAL.DeleteAll();
            ChiffrementDAL.InsertChiffrement();

            MessageBox.Show("*chiffrement*");
        }

        //La condition pour exécuter.
        private bool CanSubmitExecute(object parameter)
        {
            if (1 != 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
