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
    /// Logique d'interaction pour Statistiques.xaml
    /// </summary>
    public partial class Statistiques : Window
    {
        //Constructeur
        public Statistiques()
        {
            this.DataContext = this;
            InitializeComponent();
            listePieces.ItemsSource = QuantiteItem();
            listeVelos.ItemsSource = QuantiteItem2();;
            Montant.Text = MoyenneMontant();
            Piece.Text = MoyennePiece();
            Velo.Text = MoyenneVelo();
            listCP.ItemsSource = MeilleursClients();
        }

        /// <summary>
        /// Récupère le nombre de vente par pièce
        /// </summary>
        /// <returns></returns>
        private List<Piece> QuantiteItem()
        {
            MainWindow.connexion.Open();
            List<Piece> listesPieces = new List<Piece>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_pièce, stock_pièce, description_pièces, prix, sum(quantité_pièces)"
                                + " FROM pièce NATURAL JOIN contient_pièces GROUP BY no_pièce ORDER BY sum(quantité_pièces) DESC;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_piece = "";
            string stock_piece = "";
            string description_piece = "";
            string prix = "";
            string sum = "";

            while (reader.Read())
            {
                no_piece = reader.GetString(0);
                stock_piece = reader.GetString(1);
                description_piece = reader.GetString(2);
                prix = reader.GetString(3);
                sum = reader.GetString(4);

                Piece p = new Piece(no_piece, Convert.ToInt16(stock_piece), description_piece, Convert.ToInt16(sum),Convert.ToInt16(prix));
                listesPieces.Add(p);
            }
            MainWindow.connexion.Close();
            return listesPieces;
        }

        /// <summary>
        /// Récupère le nombre de ventes par vélo
        /// </summary>
        /// <returns></returns>
        private List<Velo> QuantiteItem2()
        {
            MainWindow.connexion.Open();
            List<Velo> listesVelos = new List<Velo>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_bicyclette, stock_bicyclette, nom_bicyclette, grandeur, prix_bicyclette, ligne_produit,"
                                + " date_intro, date_discontinuation, sum(quantité_bicyclette)"
                                + " FROM bicyclette NATURAL JOIN contient_bicyclette GROUP BY no_bicyclette ORDER BY sum(quantité_bicyclette) DESC;"; 

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_bicyclette = "";
            string stock_bicyclette = "";
            string nom_bicyclette = "";
            string grandeur = "";
            string prix_bicyclette = "";
            string ligne_produit = "";
            DateTime date_intro = new DateTime();
            DateTime date_discontinuation = new DateTime();
            int sum;

            while (reader.Read())
            {
                no_bicyclette = reader.GetString(0);
                stock_bicyclette = reader.GetString(1);
                nom_bicyclette = reader.GetString(2);
                grandeur = reader.GetString(3);
                prix_bicyclette = reader.GetString(4);
                ligne_produit = reader.GetString(5);
                date_intro = DateTime.Parse(reader.GetString(6));
                date_discontinuation = DateTime.Parse(reader.GetString(7));
                sum = Convert.ToInt16(reader.GetString(8));

                Velo v = new Velo(Convert.ToInt16(no_bicyclette), Convert.ToInt16(stock_bicyclette), nom_bicyclette, grandeur,
                    prix_bicyclette, ligne_produit, date_intro, date_discontinuation, sum);
                listesVelos.Add(v);
            }
            MainWindow.connexion.Close();
            return listesVelos;
        }

        /// <summary>
        /// Récupère le montant moyen d'une commande
        /// </summary>
        /// <returns></returns>
        public string MoyenneMontant()
        {
            string montant="";
            MainWindow.connexion.Open();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT AVG(prix) FROM Commande;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    montant = reader.GetString(0);
                }
            }
            MainWindow.connexion.Close();
            return montant;
        }

        /// <summary>
        /// Récupère le nombre moyen de pièces par commande
        /// </summary>
        /// <returns></returns>
        public string MoyennePiece()
        {
            string nbpiece = "";
            double moyenne = 0;
            MainWindow.connexion.Open();
            int count = 0;

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " select avg(quantité_pièces) from commande c left join contient_bicyclette cb on cb.no_commande = c.no_commande" 
                                +" left join contient_pièces cp on cp.no_commande = c.no_commande group by c.no_commande; ";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    nbpiece = reader.GetString(0);
                    moyenne += Convert.ToDouble(nbpiece);
                }
                count++;
            }
            MainWindow.connexion.Close();
            return Convert.ToString(moyenne/count);
        }

        /// <summary>
        /// Récupère le nombre moyen de vélos par commande
        /// </summary>
        /// <returns></returns>
        public string MoyenneVelo()
        {
            string nbVelo = "";
            double moyenne = 0;
            MainWindow.connexion.Open();
            int count = 0;

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " select avg(quantité_bicyclette) from commande c left join contient_bicyclette cb on cb.no_commande = c.no_commande"
                                + " left join contient_pièces cp on cp.no_commande = c.no_commande group by c.no_commande; ";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    nbVelo = reader.GetString(0);
                    moyenne += Convert.ToDouble(nbVelo);
                }
                count++;
            }
            MainWindow.connexion.Close();
            return Convert.ToString(moyenne / count);
        }

        /// <summary>
        /// Récupère la liste des clients ayant fait le plus d'achat
        /// </summary>
        /// <returns></returns>
        public List<ClientParticulier> MeilleursClients()
        {
            MainWindow.connexion.Open();
            List<ClientParticulier> listeCParticuliers = new List<ClientParticulier>();
            int count = 0;

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT c.no_client, c.nom_client, c.prenom_client, c.no_fidélio, sum(quantité_pièces), sum(quantité_bicyclette)"
                + "FROM individu c NATURAL JOIN commande_Client cc NATURAL JOIN commande co LEFT JOIN contient_pièces cp ON co.no_commande = cp.no_commande" 
                + " LEFT JOIN contient_bicyclette cb ON cb.No_commande = co.no_commande GROUP BY c.no_client ORDER BY sum(quantité_pièces), sum(quantité_bicyclette);";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_client = "";
            string nom_client = "";
            string prenom_client = "";
            int no_fidelio = 0;
            int sum = 0;
            int sum2 = 0;

            while (reader.Read() && count <5)
            {
                if (!reader.IsDBNull(1))
                {
                    nom_client = reader.GetString(1);
                }
                if (!reader.IsDBNull(2))
                {
                    prenom_client = reader.GetString(2);
                }
                if (!reader.IsDBNull(3))
                {
                    no_fidelio = reader.GetInt16(3);
                }
                else
                {
                    no_fidelio = 0;
                }
                if(!reader.IsDBNull(4))
                {
                    sum = reader.GetInt16(4);
                }
                else
                {
                    sum = 0;
                }
                if(!reader.IsDBNull(5))
                {
                    sum2 = reader.GetInt16(5);
                }
                else
                {
                    sum2 = 0;
                }
                if (!reader.IsDBNull(0))
                {
                    no_client = reader.GetString(0);
                    ClientParticulier cp = new ClientParticulier(Convert.ToInt16(no_client), nom_client, prenom_client, no_fidelio, (sum+sum2));
                    listeCParticuliers.Add(cp);
                    count++;
                }
            }
            MainWindow.connexion.Close();
            return listeCParticuliers;
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
