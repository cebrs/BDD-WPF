using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace BDDVersionWPF
{
    /// <summary>
    /// Logique d'interaction pour GestionFournisseurs.xaml
    /// </summary>
    public partial class GestionFournisseurs : Window
    {
        List<Fournisseur> listeFournisseurs;

        //Constructeur
        public GestionFournisseurs()
        {
            listeFournisseurs = Fournisseur.AffichageFournisseur();
            this.DataContext = this;

            InitializeComponent();
            listF.ItemsSource = listeFournisseurs;
        }

        //Méthodes
        /// <summary>
        /// Affiche les fournisseurs dans les listes XAML
        /// </summary>
        private void AffichageFournisseur()
        {
            listeFournisseurs = Fournisseur.AffichageFournisseur();
            this.DataContext = this;
            listF.ItemsSource = listeFournisseurs;
        }

        /// <summary>
        /// Bouton supprimer : supprime un fournisseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuppresionFournisseur(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            string supp = " DELETE FROM Fournisseur WHERE siret = @siret";

            command.Parameters.Add("@siret", MySqlDbType.VarChar).Value = siret1.Text;

            command.CommandText = supp;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException er)
            {
                MessageBox.Show(" ErreurConnexion : " + er.ToString());
                MainWindow.connexion.Close();
                return;
            }
            MainWindow.connexion.Close();
            AffichageFournisseur();
        }

        /// <summary>
        /// Sélectionne le fournisseur séletionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selection(object sender, RoutedEventArgs e)
        {
            foreach (Fournisseur f in listF.SelectedItems)
            {
                siret1.Text = f.Siret;
                nom1.Text = f.Nom_entreprise;
                contact1.Text = f.Contact_entreprise;
                adresse1.Text = f.Adresse_entreprise;
                libelle1.Text = Convert.ToString(f.Libelle);
            }
        }

        /// <summary>
        /// Ajoute un fournisseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutFournisseur(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string insertTable = " INSERT INTO Fournisseur(siret, nom_entreprise, contact_entreprise, adresse_entreprise, libellé)"
                                + "Values (@siret, @nomEntreprise, @contactEntreprise,@adresseEntreprise, @libelle)";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@siret", MySqlDbType.VarChar).Value = siret1.Text;
            command.Parameters.Add("@nomEntreprise", MySqlDbType.VarChar).Value = nom1.Text;
            command.Parameters.Add("@contactEntreprise", MySqlDbType.VarChar).Value = contact1.Text;
            command.Parameters.Add("@adresseEntreprise", MySqlDbType.VarChar).Value = adresse1.Text;
            command.Parameters.Add("@libelle", MySqlDbType.Int16).Value = Convert.ToInt16(libelle1.Text);

            command.CommandText = insertTable;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException er)
            {
                MessageBox.Show(" ErreurConnexion : " + er.ToString());
                MainWindow.connexion.Close();
                return;
            }
            MainWindow.connexion.Close();
            AffichageFournisseur();
        }

        /// <summary>
        /// Mets à jour le fournisseur sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiseAJourFournisseur(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string maj = "UPDATE Fournisseur set nom_entreprise =@nom, contact_entreprise =@contact, adresse_entreprise =@adresse, libellé =@libelle where siret = @siret";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@siret", MySqlDbType.VarChar).Value = siret1.Text;
            command.Parameters.Add("@nom", MySqlDbType.VarChar).Value = nom1.Text;
            command.Parameters.Add("@contact", MySqlDbType.VarChar).Value = contact1.Text;
            command.Parameters.Add("@adresse", MySqlDbType.VarChar).Value = adresse1.Text;
            command.Parameters.Add("@libelle", MySqlDbType.Int16).Value = libelle1.Text;

            command.CommandText = maj;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException er)
            {
                MessageBox.Show(" ErreurConnexion : " + er.ToString());
                MainWindow.connexion.Close();
                return;
            }
            MainWindow.connexion.Close();
            AffichageFournisseur();
        }

        /// <summary>
        /// Ferme la fenêtre XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
