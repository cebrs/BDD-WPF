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

namespace BDDVersionWPF
{
    /// <summary>
    /// Logique d'interaction pour GestionBoutiques.xaml
    /// </summary>
    public partial class GestionBoutiques : Window
    {
        List<BoutiqueSpecialisee> listeBS;

        //Constructuer
        public GestionBoutiques()
        {
            listeBS = BoutiqueSpecialisee.AffichageBS();
            this.DataContext = this;

            InitializeComponent();
            listBS.ItemsSource = listeBS; 
        }

        //Méthodes 

        /// <summary>
        /// Permet d'afficher les données de toutes les listes de la fenêtre XAML
        /// </summary>
        private void AffichageBoutique()
        {
            listeBS = BoutiqueSpecialisee.AffichageBS();
            this.DataContext = this;
            listBS.ItemsSource = listeBS;
        }

        /// <summary>
        /// Bouton supprimer : supprime la boutique sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuppresionBS(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            string supp = " DELETE FROM Boutique_spécialisé WHERE No_compagnie = @no";

            command.Parameters.Add("@no", MySqlDbType.Int16).Value = Convert.ToInt16(noBoutique.Text);

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
            AffichageBoutique();
        }

        /// <summary>
        /// Bouton ajouter : ajoute la boutique d'après les Textblocs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutBS(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string insertTable = " INSERT INTO Boutique_spécialisé(nom_compagnie, adresse_compagnie, telephone_compagnie, courriel_compagnie, remise)"
                                + "Values (@nom_compagnie, @adresse_compagnie, @telephone_compagnie, @courriel_compagnie, @remise)";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@nom_compagnie", MySqlDbType.VarChar).Value = nomBoutique.Text;
            command.Parameters.Add("@adresse_compagnie", MySqlDbType.VarChar).Value = adresseBoutique.Text;
            command.Parameters.Add("@telephone_compagnie", MySqlDbType.VarChar).Value = telephoneBoutique.Text;
            command.Parameters.Add("@courriel_compagnie", MySqlDbType.VarChar).Value = courrielBoutique.Text;
            command.Parameters.Add("@remise", MySqlDbType.Int16).Value = Convert.ToInt16(remiseBoutique.Text);

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
            AffichageBoutique();
        }

        /// <summary>
        /// Bouton sélection : récupère les données de la boutique sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionBS(object sender, RoutedEventArgs e)
        {
            foreach (BoutiqueSpecialisee bs in listBS.SelectedItems)
            {
                noBoutique.Text = Convert.ToString(bs.No_compagnie);
                nomBoutique.Text = bs.Nom_compagnie;
                adresseBoutique.Text = bs.Adresse_compagnie;
                telephoneBoutique.Text = bs.Telephone_compagnie;
                courrielBoutique.Text = bs.Courriel_compagnie;
                remiseBoutique.Text = Convert.ToString(bs.Remise);
            }
        }

        /// <summary>
        /// Bouton mettre à jour : met à jour la boutique avec les nouveaux paramètres rentrés
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiseAJourBS(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string maj = "UPDATE Boutique_spécialisé set nom_compagnie = @nom_co,adresse_compagnie = @adresse_co,"
                + "telephone_compagnie = @tel_co, courriel_compagnie = @courriel_co, remise = @remise_co"
                + " where no_compagnie = @no_compagnie;";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@no_compagnie", MySqlDbType.Int16).Value = Convert.ToInt16(noBoutique.Text);
            command.Parameters.Add("@nom_co", MySqlDbType.VarChar).Value = nomBoutique.Text;
            command.Parameters.Add("@adresse_co", MySqlDbType.VarChar).Value = adresseBoutique.Text;
            command.Parameters.Add("@tel_co", MySqlDbType.Int32).Value = Convert.ToInt32(telephoneBoutique.Text);
            command.Parameters.Add("@courriel_co", MySqlDbType.VarChar).Value = courrielBoutique.Text;
            command.Parameters.Add("@remise_co", MySqlDbType.Int16).Value = Convert.ToInt16(remiseBoutique.Text);

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
            AffichageBoutique();
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
