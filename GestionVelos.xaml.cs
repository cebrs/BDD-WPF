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
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class GestionVelos : Window
    {
        List<Velo> listeVelos;
        List<Piece> listePiecesVelo;
        List<Piece> potentiels;

        //Constructeur
        public GestionVelos()
        {
            listeVelos = Velo.AffichageVelo();
            potentiels = Piece.AffichagePiece();
            this.DataContext = this;

            InitializeComponent();
            listV.ItemsSource = listeVelos;
            listP.ItemsSource = potentiels;
        }

        //Méthodes

        /// <summary>
        /// Permet d'afficher toutes les listes XAML
        /// </summary>
        private void AffichageVelo()
        {
            listeVelos = Velo.AffichageVelo();
            potentiels = Piece.AffichagePiece();
            this.DataContext = this;
            listV.ItemsSource = listeVelos;
        }

        /// <summary>
        /// Permet de suppirmer le vélo souhaité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuppresionVelo(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            string supp = " DELETE FROM Bicyclette WHERE No_bicyclette = @no_bicyclette";

            command.Parameters.Add("@no_bicyclette", MySqlDbType.Int16).Value = Convert.ToInt16(numeroVelo.Text);
            

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
            AffichageVelo();
            SelectionPieces(Convert.ToInt16(numeroVelo.Text));
        }

        /// <summary>
        /// Permet d'ajouter le vélo souhaité
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutVelo(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string insertTable = " INSERT INTO Bicyclette(No_bicyclette,Stock_bicyclette,nom_bicyclette,grandeur,prix_bicyclette,ligne_produit,date_intro,date_discontinuation)"
                               + "Values (@nobicyclette, @stockbicyclette, @nom, @grandeur, @prix, @ligne, @date_intro, @date_discon)";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@nobicyclette", MySqlDbType.Int16).Value = Convert.ToInt16(numeroVelo.Text);
            command.Parameters.Add("@stockbicyclette", MySqlDbType.Int16).Value = Convert.ToInt16(stockVelo.Text);
            command.Parameters.Add("@nom", MySqlDbType.VarChar).Value = nomVelo.Text;
            command.Parameters.Add("@grandeur", MySqlDbType.VarChar).Value = grandeurVelo.Text;
            command.Parameters.Add("@prix", MySqlDbType.VarChar).Value = prixVelo.Text;
            command.Parameters.Add("@ligne", MySqlDbType.VarChar).Value = ligneVelo.Text;
            command.Parameters.Add("@date_intro", MySqlDbType.Date).Value = dateIntro.SelectedDate;
            command.Parameters.Add("@date_discon", MySqlDbType.Date).Value = dateDiscon.SelectedDate;

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
            AffichageVelo();
        }

        /// <summary>
        /// Permet de récupérer les attributs du vélo sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selection(object sender, RoutedEventArgs e)
        {
            foreach (Velo v in listV.SelectedItems)
            {
                numeroVelo.Text = Convert.ToString(v.No_velo);
                stockVelo.Text = Convert.ToString(v.Stock_velo);
                nomVelo.Text = v.Nom_velo;
                grandeurVelo.Text = v.Grandeur_velo;
                prixVelo.Text = v.Prix_bicyclette;
                ligneVelo.Text = v.Ligne_produit;
                dateIntro.SelectedDate = v.Date_intro;
                dateDiscon.SelectedDate = v.Date_discon;
                SelectionPieces(v.No_velo);
            }
        }

        /// <summary>
        /// Permet de mettre à jour le vélo sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiseAJourVelo(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string maj = "UPDATE Bicyclette set stock_bicyclette =@stock,nom_bicyclette =@nom, grandeur =@grandeur,"
                + " prix_bicyclette =@prix, ligne_produit =@ligne, date_intro =@date_intro," 
                + "date_discontinuation =@date_discon where No_bicyclette = @nobicyclette";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@nobicyclette", MySqlDbType.Int16).Value = Convert.ToInt16(numeroVelo.Text);
            command.Parameters.Add("@stock", MySqlDbType.Int16).Value = Convert.ToInt16(stockVelo.Text);
            command.Parameters.Add("@nom", MySqlDbType.VarChar).Value = nomVelo.Text;
            command.Parameters.Add("@grandeur", MySqlDbType.VarChar).Value = grandeurVelo.Text;
            command.Parameters.Add("@prix", MySqlDbType.VarChar).Value = prixVelo.Text;
            command.Parameters.Add("@ligne", MySqlDbType.VarChar).Value = ligneVelo.Text;
            command.Parameters.Add("@date_intro", MySqlDbType.Date).Value = dateIntro.SelectedDate;
            command.Parameters.Add("@date_discon", MySqlDbType.Date).Value = dateDiscon.SelectedDate;

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
            AffichageVelo();
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
        /// Sélectionne les pièces constituant le vélo sélectionné
        /// </summary>
        /// <param name="no_velo"></param>
        private void SelectionPieces(int no_velo)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            listePiecesVelo = new List<Piece>();

            string selection = "SELECT no_pièce, stock_pièce, description_pièces, nombre_nécessaire"
                + " FROM est_composé NATURAL JOIN pièce WHERE no_bicyclette = @no_velo;";

            command.Parameters.Add("@no_velo", MySqlDbType.Int16).Value = no_velo;

            command.CommandText = selection;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_piece = "";
            string stock_piece = "";
            string description_piece = "";
            string quantite = "";

            while (reader.Read())
            {
                no_piece = reader.GetString(0);
                stock_piece = reader.GetString(1);
                description_piece = reader.GetString(2);
                quantite = reader.GetString(3);

                Piece p = new Piece(no_piece, Convert.ToInt16(stock_piece), description_piece, Convert.ToInt16(quantite));
                listePiecesVelo.Add(p);
            }
            MainWindow.connexion.Close();
            listPV.ItemsSource = listePiecesVelo;
        }

        /// <summary>
        /// Permet d'ajouter une pièce à la liste des pièces constituant le vélo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutPiece(object sender, RoutedEventArgs e)
        {
            foreach(Piece p in listP.SelectedItems)
            {
                MainWindow.connexion.Open();
                string insertTable = " INSERT INTO est_composé(No_pièce,nombre_nécessaire, no_bicyclette)"
                                   + "Values (@nopiece, @nombre, @nobicyclette)";

                MySqlCommand command = MainWindow.connexion.CreateCommand();

                command.Parameters.Add("@nopiece", MySqlDbType.VarChar).Value = p.No_piece;
                command.Parameters.Add("@nombre", MySqlDbType.Int16).Value = Convert.ToInt16(quantity.Text);
                command.Parameters.Add("@nobicyclette", MySqlDbType.Int16).Value = Convert.ToInt16(numeroVelo.Text);

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
                SelectionPieces(Convert.ToInt16(numeroVelo.Text));
            }
        }
    }
}
