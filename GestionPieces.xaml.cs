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
    /// Logique d'interaction pour GestionPieces.xaml
    /// </summary>
    public partial class GestionPieces : Window
    {
        List<Piece> listePieces;
        List<Fournisseur> listeFournisseurs;
        List<Fournisseur> allFournisseurs;

        //Constructeur
        public GestionPieces()
        {
            listePieces = Piece.AffichagePiece();
            allFournisseurs = Fournisseur.AffichageFournisseur();
            listeFournisseurs = new List<Fournisseur>();
            this.DataContext = this;

            InitializeComponent();
            listP.ItemsSource = listePieces;
            listF.ItemsSource = allFournisseurs;
            listFourni.ItemsSource = listeFournisseurs;
        }


        //Méthodes
        /// <summary>
        /// Permet d'afficher toutes les listes XAML
        /// </summary>
        private void AffichagePiece()
        {
            listePieces = Piece.AffichagePiece();
            allFournisseurs = Fournisseur.AffichageFournisseur();
            this.DataContext = this;
            listP.ItemsSource = listePieces;
            listF.ItemsSource = allFournisseurs;
        }

        /// <summary>
        /// Permet de sélectionner les fournisseurs de la pièce sélectionnée
        /// </summary>
        /// <param name="no_piece"></param>
        private void SelectionFournisseurs(string no_piece)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            listeFournisseurs = new List<Fournisseur>();

            string selection = "SELECT siret, nom_entreprise, no_catalogue, delai_livraison, prix_pièce, libellé"
                + " FROM fournit NATURAL JOIN fournisseur WHERE no_pièce = @no_piece;";

            command.Parameters.Add("@no_piece", MySqlDbType.VarChar).Value = no_piece;

            command.CommandText = selection;

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string siret = "";
            string nom_entreprise = "";
            string no_catalogue = "";
            string delai = "";
            string prix = "";
            string libelle = "";

            while (reader.Read())
            {
                siret = reader.GetString(0);
                nom_entreprise = reader.GetString(1);
                no_catalogue = reader.GetString(2);
                delai = reader.GetString(3);
                prix = reader.GetString(4);
                libelle = reader.GetString(5);
                Fournisseur f = new Fournisseur(siret, nom_entreprise, no_catalogue, Convert.ToInt16(delai), Convert.ToInt16(prix), Convert.ToInt16(libelle));
                listeFournisseurs.Add(f);
            }

            listFourni.ItemsSource = listeFournisseurs;
            MainWindow.connexion.Close();
        }

        /// <summary>
        /// Permet de supprimer une pièce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuppresionPiece(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            string supp = "DELETE FROM Pièce WHERE No_pièce = @no_piece;";

            command.Parameters.Add("@no_piece", MySqlDbType.VarChar).Value = numeroPiece.Text;

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
            AffichagePiece();
            listeFournisseurs = new List<Fournisseur>();
            listFourni.ItemsSource = listeFournisseurs;
        }

        /// <summary>
        /// Permet d'ajouter une pièce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutPiece(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string insertTable = " INSERT INTO Pièce(No_pièce,stock_pièce,description_pièces, prix)"
                               + "Values (@nopiece, @stockpiece, @description, @prix);";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@nopiece", MySqlDbType.VarChar).Value = numeroPiece.Text;
            command.Parameters.Add("@stockpiece", MySqlDbType.Int16).Value = Convert.ToInt16(stockPiece.Text);
            command.Parameters.Add("@description", MySqlDbType.VarChar).Value = descriptionPiece.Text;
            command.Parameters.Add("@prix", MySqlDbType.Int16).Value = Convert.ToInt16(prixPiece.Text);
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
            AffichagePiece();
        }

        /// <summary>
        /// Sélectionne la pièce sélectionnée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selection(object sender, RoutedEventArgs e)
        {
            foreach (Piece p in listP.SelectedItems)
            {
                numeroPiece.Text = p.No_piece;
                stockPiece.Text = Convert.ToString(p.Stock_piece);
                descriptionPiece.Text = p.Description_piece;
                prixPiece.Text = Convert.ToString(p.Prix);
                SelectionFournisseurs(p.No_piece);
            }
        }

        /// <summary>
        /// Mets à jour la pièce souhaitée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MiseAJourPiece(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            string maj = "UPDATE Pièce set stock_pièce =@stock,description_pièces =@descrip , prix=@prix where No_pièce = @nbpiece";

            MySqlCommand command = MainWindow.connexion.CreateCommand();

            command.Parameters.Add("@nbpiece", MySqlDbType.VarChar).Value = numeroPiece.Text;
            command.Parameters.Add("@stock", MySqlDbType.Int16).Value = Convert.ToInt16(stockPiece.Text);
            command.Parameters.Add("@descrip", MySqlDbType.VarChar).Value = descriptionPiece.Text;
            command.Parameters.Add("@prix", MySqlDbType.Int16).Value = Convert.ToInt16(prixPiece.Text);

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
            AffichagePiece();
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
        /// AJoute un fournisseru pour cette pièce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjoutFournisseur(object sender, RoutedEventArgs e)
        {
            foreach(Fournisseur f in listF.SelectedItems)
            {
                MainWindow.connexion.Open();
                string insertTable = "INSERT INTO fournit(No_catalogue,delai_livraison,prix_pièce, no_pièce, siret)"
                                   + "Values (@noCatalogue, @delai_livraison, @prix_piece, @no_piece, @siret)";

                MySqlCommand command = MainWindow.connexion.CreateCommand();

                command.Parameters.Add("@noCatalogue", MySqlDbType.VarChar).Value = noCatalogue.Text;
                command.Parameters.Add("@delai_livraison", MySqlDbType.Int16).Value = Convert.ToInt16(delai.Text);
                command.Parameters.Add("@prix_piece", MySqlDbType.Int16).Value = Convert.ToInt16(prix.Text);
                command.Parameters.Add("@no_piece", MySqlDbType.VarChar).Value = numeroPiece.Text;
                command.Parameters.Add("@siret", MySqlDbType.VarChar).Value = f.Siret;

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
                SelectionFournisseurs(numeroPiece.Text);
            }
        }
    }
}
