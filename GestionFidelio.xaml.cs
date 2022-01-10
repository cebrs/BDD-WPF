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
using System.IO;
using Newtonsoft.Json;

namespace BDDVersionWPF
{
    /// <summary>
    /// Logique d'interaction pour GestionFidelio.xaml
    /// </summary>
    public partial class GestionFidelio : Window
    {
        //Constructeur
        public GestionFidelio()
        {
            this.DataContext = this;
            InitializeComponent();
            listCP1.ItemsSource = Liste(1);
            listCP2.ItemsSource = Liste(2);
            listCP3.ItemsSource = Liste(3);
            listCP4.ItemsSource = Liste(4);
        }

        //Méthodes
        /// <summary>
        /// Récupère la liste des fidélio existants dans la BDD
        /// </summary>
        /// <param name="nb"></param>
        /// <returns></returns>
        List<ClientParticulier> Liste(int nb)
        {
            List<ClientParticulier> listeCP = new List<ClientParticulier>();
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_client, nom_client, prenom_client, adresse_client, telephone_client, courriel_client, "
                + "no_fidélio, fin_fidélio FROM individu WHERE no_fidélio =@no ORDER BY no_client;";

            command.Parameters.Add("@no", MySqlDbType.Int32).Value = nb;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_client = "";
            string nom_client = "";
            string prenom_client = "";
            string adresse_client = "";
            string telephone_client = "";
            string courriel_client = "";
            string no_fidelio = "";
            DateTime fin_fidelio = new DateTime();

            while (reader.Read())
            {
                no_client = reader.GetString(0);
                nom_client = reader.GetString(1);
                prenom_client = reader.GetString(2);
                adresse_client = reader.GetString(3);
                telephone_client = reader.GetString(4);
                courriel_client = reader.GetString(5);
                if (reader.IsDBNull(6))
                {
                    no_fidelio = null;
                }
                else
                {
                    no_fidelio = reader.GetString(6);
                    fin_fidelio = DateTime.Parse(reader.GetString(7));
                }
                ClientParticulier cp = new ClientParticulier(Convert.ToInt16(no_client), nom_client, prenom_client, adresse_client, telephone_client,
                    courriel_client, Convert.ToInt16(no_fidelio), fin_fidelio);
                listeCP.Add(cp);
            }
            MainWindow.connexion.Close();
            return listeCP;
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
        /// Bouton JSON : créé le fichier JSON des clients à expiration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FichierJSON(object sender, RoutedEventArgs e)
        {
            List<ClientParticulier> clients = new List<ClientParticulier>();

            MainWindow.connexion.Open();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_client, nom_client, prenom_client, no_fidélio, fin_fidélio FROM individu ORDER BY no_client;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_client = "";
            string nom_client = "";
            string prenom_client = "";
            string no_fidelio = "";
            DateTime fin_fidelio = new DateTime();

            while (reader.Read())
            {
                no_client = reader.GetString(0);
                nom_client = reader.GetString(1);
                prenom_client = reader.GetString(2);
                if (!reader.IsDBNull(3))
                {
                    no_fidelio = reader.GetString(3);
                    fin_fidelio = DateTime.Parse(reader.GetString(4));
                }
                if ((DateTime.Now - fin_fidelio).Days < 60)
                {
                    ClientParticulier cp = new ClientParticulier(Convert.ToInt16(no_client), nom_client, prenom_client, Convert.ToInt16(no_fidelio), fin_fidelio);
                    clients.Add(cp);
                }
            }
            MainWindow.connexion.Close();


            //fichier destinataire de la sérialisation
            string fileToWrite = "stocks.json";

            //instanciation des flux d'écriture(writer)
            StreamWriter fileWriter = new StreamWriter(fileToWrite);
            JsonTextWriter jsonWriter = new JsonTextWriter(fileWriter);

            // sérialisation des objets vers le flux d'écriture fichier
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(jsonWriter, clients);

            //fermture des flux (writer)
            jsonWriter.Close();
            fileWriter.Close();
        }
    }
}
