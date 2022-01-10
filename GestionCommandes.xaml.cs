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
    /// Logique d'interaction pour GestionCommandes.xaml
    /// </summary>
    public partial class GestionCommandes : Window
    {
        List<Piece> listePieces;
        List<Velo> listeVelos;
        List<BoutiqueSpecialisee> listeBoutiques;
        List<ClientParticulier> listeClients;
        List<Commande> listeCommandes;
        Dictionary<Piece, int> panierPieces = new Dictionary<Piece, int>();
        Dictionary<Velo, int> panierVelos = new Dictionary<Velo, int>();

        //Constructeur
        public GestionCommandes()
        {
            listePieces = Piece.AffichagePiece();
            listeVelos = Velo.AffichageVelo();
            listeCommandes = Commande.AffichageCommande();
            listeBoutiques = BoutiqueSpecialisee.AffichageBS();
            listeClients = ClientParticulier.AffichageParticulier();
            this.DataContext = this;
            InitializeComponent();
            listP.ItemsSource = listePieces;
            listV.ItemsSource = listeVelos;
            listCommandes.ItemsSource = listeCommandes;
            listBoutique.ItemsSource = listeBoutiques;
            listClient.ItemsSource = listeClients;
        }


        //Méthodes

        /// <summary>
        /// Permet d'afficher toutes les listes de la fenêtre XAML
        /// </summary>
        public void AffichageCommande()
        {
            listePieces = Piece.AffichagePiece();
            listeVelos = Velo.AffichageVelo();
            listeCommandes = Commande.AffichageCommande();
            listeBoutiques = BoutiqueSpecialisee.AffichageBS();
            listeClients = ClientParticulier.AffichageParticulier();
            this.DataContext = this;
            listP.ItemsSource = listePieces;
            listV.ItemsSource = listeVelos;
            listCommandes.ItemsSource = listeCommandes;
            listBoutique.ItemsSource = listeBoutiques;
            listClient.ItemsSource = listeClients;
        }

        /// <summary>
        /// Bouton ajouter : ajoute une pièce à la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjouterP(object sender, RoutedEventArgs e)
        {
            foreach (Piece p in listP.SelectedItems)
            {
                panierPieces.Add(p, Convert.ToInt16(quantitePiece.Text));
            }
        }

        /// <summary>
        /// Bouton ajouter : ajoute un vélo à la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AjouterV(object sender, RoutedEventArgs e)
        {
            foreach (Velo v in listV.SelectedItems)
            {
                panierVelos.Add(v, Convert.ToInt16(quantiteVelos.Text));
            }
        }

        /// <summary>
        /// Bouton supprime : supprime une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SuppressionC(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();
            MySqlCommand command = MainWindow.connexion.CreateCommand();

            string supp = "DELETE FROM Commande WHERE No_commande = @no_commande;";

            foreach (Commande c in listCommandes.SelectedItems)
            {
                command.Parameters.Add("@no_commande", MySqlDbType.Int16).Value = c.No_commande;
            }

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
            AffichageCommande();
        }

        /// <summary>
        /// Bouton commande client : réalise une commande client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandeClient(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();

            foreach (ClientParticulier cp in listClient.SelectedItems)
            {
                MySqlCommand command = MainWindow.connexion.CreateCommand();

                string commande = "INSERT INTO commande (no_commande, date_commande, date_livraison, prix) "
                    + " values(@noCommande, @date_commande, @date_livraison, @prix);";

                command.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text);
                command.Parameters.Add("@date_commande", MySqlDbType.Date).Value = DateTime.Now;
                command.Parameters.Add("@date_livraison", MySqlDbType.Date).Value = new DateTime(2021, 06, 08);

                int prix = 0;
                foreach (Piece p in panierPieces.Keys)
                {
                    prix += p.Prix * panierPieces[p];
                }
                foreach (Velo v in panierVelos.Keys)
                {
                    prix += Convert.ToInt16(v.Prix_bicyclette) * panierVelos[v];
                }

                command.Parameters.Add("@prix", MySqlDbType.Int16).Value = prix;

                command.CommandText = commande;
                try
                {
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                catch (MySqlException er)
                {
                    MessageBox.Show(" ErreurConnexion : " + er.ToString());
                    MainWindow.connexion.Close();
                    return;
                }

                foreach (Piece p in panierPieces.Keys)
                {
                    string nb_piece = "INSERT INTO contient_pièces (quantité_pièces, no_commande, no_pièce) "
                        + " Values(@quantite, @noCommande, @noPiece);";
                    string stockPiece = "Update pièce set stock_pièce=@nvstock where No_pièce =@noPiece;";

                    MySqlCommand commandPiece = MainWindow.connexion.CreateCommand();
                    MySqlCommand stock_Piece = MainWindow.connexion.CreateCommand();
                    commandPiece.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text);
                    commandPiece.Parameters.Add("@quantite", MySqlDbType.Int16).Value = panierPieces[p];
                    commandPiece.Parameters.Add("@noPiece", MySqlDbType.VarChar).Value = p.No_piece;
                    commandPiece.CommandText = nb_piece;

                    stock_Piece.Parameters.Add("@nvstock", MySqlDbType.Int16).Value = p.Stock_piece - panierPieces[p];
                    stock_Piece.Parameters.Add("@noPiece", MySqlDbType.VarChar).Value = p.No_piece;
                    stock_Piece.CommandText = stockPiece;

                    try
                    {
                        commandPiece.ExecuteNonQuery();
                        commandPiece.Dispose();
                        stock_Piece.ExecuteNonQuery();
                        stock_Piece.Dispose();
                    }
                    catch (MySqlException er)
                    {
                        MessageBox.Show(" ErreurConnexion : " + er.ToString());
                        MainWindow.connexion.Close();
                        return;
                    }
                }
                foreach (Velo v in panierVelos.Keys)
                {
                    string nb_velo = "INSERT INTO contient_bicyclette (quantité_bicyclette, no_bicyclette, no_commande) "
                        + " Values(@quantite, @noVelo, @noCommande);";
                    string stockVelo = "Update bicyclette set stock_bicyclette=@nvstock where No_bicyclette =@no;";

                    MySqlCommand commandV = MainWindow.connexion.CreateCommand();
                    MySqlCommand stock_Velo = MainWindow.connexion.CreateCommand();
                    commandV.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text); ;
                    commandV.Parameters.Add("@quantite", MySqlDbType.Int16).Value = panierVelos[v];
                    commandV.Parameters.Add("@noVelo", MySqlDbType.Int16).Value = v.No_velo;
                    commandV.CommandText = nb_velo;

                    stock_Velo.Parameters.Add("@nvstock", MySqlDbType.Int16).Value = v.Stock_velo - panierVelos[v];
                    stock_Velo.Parameters.Add("@no", MySqlDbType.Int16).Value = v.No_velo;
                    stock_Velo.CommandText = stockVelo;

                    try
                    {
                        commandV.ExecuteNonQuery();
                        commandV.Dispose();
                        stock_Velo.ExecuteNonQuery();
                        stock_Velo.Dispose();
                    }
                    catch (MySqlException er)
                    {
                        MessageBox.Show(" ErreurConnexion : " + er.ToString());
                        MainWindow.connexion.Close();
                        return;
                    }
                }

                MySqlCommand commandeClient = MainWindow.connexion.CreateCommand();
                string commandeParticulier = "INSERT INTO commande_Client (no_commande, no_client)" +
                    "values (@noCommande, @noClient);";
                commandeClient.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text);
                commandeClient.Parameters.Add("@noClient", MySqlDbType.Int16).Value = cp.No_client;

                commandeClient.CommandText = commandeParticulier;
                try
                {
                    commandeClient.ExecuteNonQuery();
                    commandeClient.Dispose();
                }
                catch (MySqlException er)
                {
                    MessageBox.Show(" ErreurConnexion : " + er.ToString());
                    MainWindow.connexion.Close();
                    return;
                }
            }

            MainWindow.connexion.Close();
            panierPieces = new Dictionary<Piece, int>();
            panierVelos = new Dictionary<Velo, int>();
            AffichageCommande();
        }

        /// <summary>
        /// Bouton commande boutique : réalise une commande boutique
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandeBoutique(object sender, RoutedEventArgs e)
        {
            MainWindow.connexion.Open();

            foreach (BoutiqueSpecialisee bs in listBoutique.SelectedItems)
            {
                MySqlCommand command = MainWindow.connexion.CreateCommand();

                string commande = "INSERT INTO commande (no_commande, date_commande, date_livraison, prix) "
                    + " values(@noCommande, @date_commande, @date_livraison, @prix);";

                command.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text);
                command.Parameters.Add("@date_commande", MySqlDbType.Date).Value = DateTime.Now;
                command.Parameters.Add("@date_livraison", MySqlDbType.Date).Value = new DateTime(2021, 06, 08);

                int prix = 0;
                foreach(Piece p in panierPieces.Keys)
                {
                    prix += p.Prix * panierPieces[p];
                }
                foreach(Velo v in panierVelos.Keys)
                {
                    prix += Convert.ToInt16(v.Prix_bicyclette) * panierVelos[v];
                }
                prix = prix * (100-bs.Remise) / 100;

                command.Parameters.Add("@prix", MySqlDbType.Int16).Value = prix;

                command.CommandText = commande;
                try
                {
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
                catch (MySqlException er)
                {
                    MessageBox.Show(" ErreurConnexion : " + er.ToString());
                    MainWindow.connexion.Close();
                    return;
                }

                foreach (Piece p in panierPieces.Keys)
                {
                    string nb_piece = "INSERT INTO contient_pièces (quantité_pièces, no_commande, no_pièce) "
                        + " Values(@quantite, @noCommande, @noPiece);";
                    string stockPiece = "Update pièce set stock_pièce=@nvstock where No_pièce =@noPiece;";

                    MySqlCommand commandPiece = MainWindow.connexion.CreateCommand();
                    MySqlCommand stock_Piece = MainWindow.connexion.CreateCommand();
                    commandPiece.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text);
                    commandPiece.Parameters.Add("@quantite", MySqlDbType.Int16).Value = panierPieces[p];
                    commandPiece.Parameters.Add("@noPiece", MySqlDbType.VarChar).Value = p.No_piece;
                    commandPiece.CommandText = nb_piece;

                    stock_Piece.Parameters.Add("@nvstock", MySqlDbType.Int16).Value = p.Stock_piece - panierPieces[p];
                    stock_Piece.Parameters.Add("@noPiece", MySqlDbType.VarChar).Value = p.No_piece;
                    stock_Piece.CommandText = stockPiece;

                    try
                    {
                        commandPiece.ExecuteNonQuery();
                        commandPiece.Dispose();
                        stock_Piece.ExecuteNonQuery();
                        stock_Piece.Dispose();
                    }
                    catch (MySqlException er)
                    {
                        MessageBox.Show(" ErreurConnexion : " + er.ToString());
                        MainWindow.connexion.Close();
                        return;
                    }
                }
                foreach (Velo v in panierVelos.Keys)
                {
                    string nb_velo = "INSERT INTO contient_bicyclette (quantité_bicyclette, no_bicyclette, no_commande) "
                        + " Values(@quantite, @noVelo, @noCommande);";
                    string stockVelo = "Update bicyclette set stock_bicyclette=@nvstock where No_bicyclette =@nobic;";

                    MySqlCommand commandV = MainWindow.connexion.CreateCommand();
                    MySqlCommand stock_velo = MainWindow.connexion.CreateCommand();

                    commandV.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text); ;
                    commandV.Parameters.Add("@quantite", MySqlDbType.Int16).Value = panierVelos[v];
                    commandV.Parameters.Add("@noVelo", MySqlDbType.VarChar).Value = v.No_velo;
                    commandV.CommandText = nb_velo;

                    stock_velo.Parameters.Add("@nvstock", MySqlDbType.Int16).Value = v.Stock_velo - panierVelos[v];
                    stock_velo.Parameters.Add("@nobic", MySqlDbType.VarChar).Value = v.No_velo;
                    stock_velo.CommandText = stockVelo;
                    try
                    {
                        commandV.ExecuteNonQuery();
                        commandV.Dispose();
                        stock_velo.ExecuteNonQuery();
                        stock_velo.Dispose();
                    }
                    catch (MySqlException er)
                    {
                        MessageBox.Show(" ErreurConnexion : " + er.ToString());
                        MainWindow.connexion.Close();
                        return;
                    }
                }
                MySqlCommand commandeBS = MainWindow.connexion.CreateCommand();
                string commandeBoutique = "INSERT INTO commande_Boutique (no_commande, no_compagnie)" +
                    "values (@noCommande, @noBoutique);";
                commandeBS.Parameters.Add("@noCommande", MySqlDbType.Int16).Value = Convert.ToInt16(numCommande.Text);
                commandeBS.Parameters.Add("@noBoutique", MySqlDbType.Int16).Value = bs.No_compagnie;

                commandeBS.CommandText = commandeBoutique;
                try
                {
                    commandeBS.ExecuteNonQuery();
                    commandeBS.Dispose();
                }
                catch (MySqlException er)
                {
                    MessageBox.Show(" ErreurConnexion : " + er.ToString());
                    MainWindow.connexion.Close();
                    return;
                }
            }

            panierPieces = new Dictionary<Piece, int>();
            panierVelos = new Dictionary<Velo, int>();
            MainWindow.connexion.Close();
            Piece.AffichagePiece();
            AffichageCommande();
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
