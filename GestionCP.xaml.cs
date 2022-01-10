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
    /// Logique d'interaction pour GestionCP.xaml
    /// </summary>
    public partial class GestionCP : Window
    {
        List<ClientParticulier> listeClientsParticuliers;

        //Constructeur
        public GestionCP()
        {
            listeClientsParticuliers = ClientParticulier.AffichageParticulier();
            this.DataContext = this;
            InitializeComponent();
            listCP.ItemsSource = listeClientsParticuliers;
            listFidelio.ItemsSource = Fidelio.AffichageFidelio();
        }

        //Méthodes
        /// <summary>
        /// Permet d'afficher toutes les listes de la fenêtre XAML
        /// </summary>
        private void AffichageParticulier()
        {
            listeClientsParticuliers = ClientParticulier.AffichageParticulier();
            this.DataContext = this;
            listCP.ItemsSource = listeClientsParticuliers;
        }

        /// <summary>
        /// Bouton supprime : supprime un client paticulier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuppresionCP(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            string supp = " DELETE FROM Individu WHERE No_client = @no_client";

            command.Parameters.Add("@no_client", MySqlDbType.Int16).Value = Convert.ToInt16(noClient.Text);

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
            AffichageParticulier();
        }

        /// <summary>
        /// Bouton ajouter : ajoute un client 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutCP(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string insertTable = " INSERT INTO Individu(nom_client, prenom_client, adresse_client, telephone_client, courriel_client, no_fidélio)"
                + "Values (@nomclient, @prenomclient, @adresseclient, @telephoneclient, @courrielclient, @nofidelio);";
            string insertTable2 = " INSERT INTO Individu(nom_client, prenom_client, adresse_client, telephone_client, courriel_client)"
                + "Values (@nomclient, @prenomclient, @adresseclient, @telephoneclient, @courrielclient);";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@nomclient", MySqlDbType.VarChar).Value = nomClient.Text;
            command.Parameters.Add("@prenomclient", MySqlDbType.VarChar).Value = prenomClient.Text;
            command.Parameters.Add("@adresseclient", MySqlDbType.VarChar).Value = adresseClient.Text;
            command.Parameters.Add("@telephoneclient", MySqlDbType.VarChar).Value = telephoneClient.Text;
            command.Parameters.Add("@courrielclient", MySqlDbType.VarChar).Value = courrielClient.Text;
            command.Parameters.Add("@nofidelio", MySqlDbType.Int16).Value = Convert.ToInt16(fidelioClient.Text);

            if (Convert.ToInt16(fidelioClient.Text) == 0)
            {
                command.CommandText = insertTable2;
            }
            else
            {
                command.CommandText = insertTable;
            }
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
            AffichageParticulier();
        }

        /// <summary>
        /// Bouton sléection : sélectionne les attributs du client sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionCP(object sender, RoutedEventArgs e)
        {
            foreach (ClientParticulier cp in listCP.SelectedItems)
            {
                noClient.Text = Convert.ToString(cp.No_client);
                nomClient.Text = cp.Nom_client;
                prenomClient.Text = cp.Prenom_client;
                adresseClient.Text = cp.Adresse_client;
                telephoneClient.Text = cp.Telephone_client;
                courrielClient.Text = cp.Courriel_client;
                fidelioClient.Text = Convert.ToString(cp.No_fidelio);
            }
        }

        /// <summary>
        /// Bouton mAJ : met à jour le client sélectionné (HORS FIDELIO)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiseAJourCP(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string maj = "UPDATE Individu set nom_client = @nom, prenom_client =@prenom, adresse_client = @adresse,"
                + "telephone_client = @tel, courriel_client = @courriel where no_client = @noClient;";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@noClient", MySqlDbType.Int16).Value = Convert.ToInt16(noClient.Text);
            command.Parameters.Add("@nom", MySqlDbType.VarChar).Value = nomClient.Text;
            command.Parameters.Add("@prenom", MySqlDbType.VarChar).Value = prenomClient.Text;
            command.Parameters.Add("@adresse", MySqlDbType.VarChar).Value = adresseClient.Text;
            command.Parameters.Add("@tel", MySqlDbType.VarChar).Value = telephoneClient.Text;
            command.Parameters.Add("@courriel", MySqlDbType.VarChar).Value = courrielClient.Text;

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
            AffichageParticulier();
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

        /// <summary>
        /// Mets à jour le numéro fidélio et la date de fin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiseAJourFidelio(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string selec = "SELECT durée from fidélio where no_fidélio=@no;";
            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.Parameters.Add("@noClient", MySqlDbType.Int16).Value = Convert.ToInt16(noClient.Text);
            command.Parameters.Add("@no", MySqlDbType.Int16).Value = Convert.ToInt16(fidelioClient.Text);

            command.CommandText = selec;
            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string duree = "";

            while (reader.Read())
            {
                duree = reader.GetString(0);
            }

            command.Dispose();
            MainWindow.connexion.Close();

            MainWindow.connexion.Open();

            MySqlCommand command1 = MainWindow.connexion.CreateCommand();
            string maj = "UPDATE Individu set no_fidélio = @nofidelio, fin_fidélio=@fin_fidelio where no_client = @noClient;";
            command1.Parameters.Add("@noClient", MySqlDbType.Int16).Value = Convert.ToInt16(noClient.Text);
            command1.Parameters.Add("@nofidelio", MySqlDbType.Int16).Value = Convert.ToInt16(fidelioClient.Text);
            command1.Parameters.Add("@fin_fidelio", MySqlDbType.Date).Value = new DateTime(DateTime.Now.Year+ Convert.ToInt16(duree) , DateTime.Now.Month, DateTime.Now.Day);

            command1.CommandText = maj;
            try
            {
                command1.ExecuteNonQuery();
            }
            catch (MySqlException er)
            {
                MessageBox.Show(" ErreurConnexion : " + er.ToString());
                MainWindow.connexion.Close();
                return;
            }
            MainWindow.connexion.Close();
            AffichageParticulier();
        }
    }
}

