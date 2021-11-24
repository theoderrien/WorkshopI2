using System.Windows;
using System.Windows.Input;
using WorkshopI2.Command;

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
