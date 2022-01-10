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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Xml;

namespace BDDVersionWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MySqlConnection connexion = new MySqlConnection("SERVER=localhost;PORT=3306;" +
                                        "DATABASE=veloMax;" +
                                        "UID={nom_utilisateur};PASSWORD={mdp_utilisateur}");
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Géstion des pièces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionP(object sender, RoutedEventArgs e)
        {
            GestionPieces gestionPieces = new GestionPieces();
            gestionPieces.Show();
        }

        /// <summary>
        /// Gesiton des vélos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionV(object sender, RoutedEventArgs e)
        {
            GestionVelos gestionVelos = new GestionVelos();
            gestionVelos.Show();
        }

        /// <summary>
        /// Gestions des commandes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionC(object sender, RoutedEventArgs e)
        {
            GestionCommandes gestionCommandes = new GestionCommandes();
            gestionCommandes.Show();
        }

        /// <summary>
        /// Gestion des fournisseurs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionF(object sender, RoutedEventArgs e)
        {
            GestionFournisseurs gestionFournisseurs = new GestionFournisseurs();
            gestionFournisseurs.Show();
        }

        /// <summary>
        /// Gestion des clients particuliers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionCP(object sender, RoutedEventArgs e)
        {
            GestionCP gestionCP = new GestionCP();
            gestionCP.Show();
        }

        /// <summary>
        /// Gestion des boutiques spécialisées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionB(object sender, RoutedEventArgs e)
        {
            GestionBoutiques gestionBoutiques = new GestionBoutiques();
            gestionBoutiques.Show();
        }

        /// <summary>
        /// Ferme la fen^tre XAML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Gestion des stocks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionStock(object sender, RoutedEventArgs e)
        {
            GestionStocks gestionStocks = new GestionStocks();
            gestionStocks.Show();
        }

        /// <summary>
        /// Rapport statistiques
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stats(object sender, RoutedEventArgs e) // A FAIRE
        {
            Statistiques stats = new Statistiques();
            stats.Show();
        }

        /// <summary>
        /// Gestion des programmes de fidélité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestionFidelio(object sender, RoutedEventArgs e)
        {
            GestionFidelio gestionFidelio = new GestionFidelio();
            gestionFidelio.Show();
        }
    }
}
