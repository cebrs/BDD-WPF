using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace BDDVersionWPF
{
    /// <summary>
    /// Logique d'interaction pour GestionStocks.xaml
    /// </summary>
    public partial class GestionStocks : Window
    {
        List<Piece> listePieces;
        List<Velo> listeVelos;
        List<Piece> listeRecommander;
        List<Fournisseur> listeFournisseurs;
        List<Piece> listePiecesVelo;

        //Constructeurs
        public GestionStocks()
        {
            listePieces = Piece.AffichagePiece();
            listeVelos = Velo.AffichageVelo();
            listeRecommander = PiecesARecommander();

            this.DataContext = this;
            InitializeComponent();
            listP.ItemsSource = listePieces;
            listV.ItemsSource = listeVelos;
            listPR.ItemsSource = listeRecommander;
        }

        //Porpriétés
        public List<Piece> ListeRecommander
        {
            get { return listeRecommander; }
            set { listeRecommander = value; }
        }

        //Méthodes

        /// <summary>
        /// Permet d'afficher toutes les listes de la fenêtre
        /// </summary>
        public void AffichageGestionStocks()
        {
            listePieces = Piece.AffichagePiece();
            listeVelos = Velo.AffichageVelo();
            listeRecommander = PiecesARecommander();

            listP.ItemsSource = listePieces;
            listV.ItemsSource = listeVelos;
            listPR.ItemsSource = listeRecommander;
        }

        /// <summary>
        /// Sélectionne les pièces dont le stock est strictement inférieur à 3
        /// </summary>
        /// <returns></returns>
        public List<Piece> PiecesARecommander()
        {
            List<Piece> list = new List<Piece>();
            MainWindow.connexion.Open();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_pièce, stock_pièce, description_pièces, prix"
                                + " FROM pièce WHERE (stock_pièce<3) ORDER BY no_pièce;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_piece = "";
            string stock_piece = "";
            string description_piece = "";
            string prix = "";

            while (reader.Read())
            {
                no_piece = reader.GetString(0);
                stock_piece = reader.GetString(1);
                description_piece = reader.GetString(2);
                prix = reader.GetString(3);
                Piece p = new Piece(no_piece, Convert.ToInt16(stock_piece), description_piece, Convert.ToInt16(prix));
                list.Add(p);
            }
            MainWindow.connexion.Close();

            return list;            
        }

        /// <summary>
        /// Boutton sélectionner : sélectionne les fournisseurs de la pièce
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selection(object sender, RoutedEventArgs e)
        {
            foreach (Piece p in listPR.SelectedItems)
            {
                MainWindow.connexion.Open();
                MySqlCommand command = MainWindow.connexion.CreateCommand();

                listeFournisseurs = new List<Fournisseur>();

                string selection = "SELECT siret, nom_entreprise, no_catalogue, delai_livraison, prix_pièce, libellé"
                    + " FROM fournit NATURAL JOIN fournisseur WHERE no_pièce = @no_piece;";

                command.Parameters.Add("@no_piece", MySqlDbType.VarChar).Value = p.No_piece;

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
        }

        /// <summary>
        /// Bouton commande : commande la pièce auprès du fournisseur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Commande(object sender, RoutedEventArgs e) 
        {
            foreach (Piece p in listPR.SelectedItems)
            {
                foreach (Fournisseur f in listFourni.SelectedItems)
                {
                    MainWindow.connexion.Open();
                    MySqlCommand command = MainWindow.connexion.CreateCommand();

                    string maj = "UPDATE Pièce SET stock_pièce = @nvstock where no_pièce=@no_piece;";

                    command.Parameters.Add("@no_piece", MySqlDbType.VarChar).Value = p.No_piece;
                    command.Parameters.Add("@nvstock", MySqlDbType.Int16).Value = Convert.ToInt16(quantitePiece.Text);

                    command.CommandText = maj;

                    //AJOUTER LE DELAI DU FOURNISSEUR
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
                }
            }
            AffichageGestionStocks();
        }

        /// <summary>
        /// Bouton sélection vélo : sélectionner le vélo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionVelo(object sender, RoutedEventArgs e)
        {
            foreach (Velo v in listV.SelectedItems)
            {
                MainWindow.connexion.Open();
                MySqlCommand command = MainWindow.connexion.CreateCommand();

                listePiecesVelo = new List<Piece>();

                string selection = "SELECT no_pièce, stock_pièce, description_pièces, nombre_nécessaire"
                    + " FROM est_composé NATURAL JOIN pièce WHERE no_bicyclette = @no_velo;";

                command.Parameters.Add("@no_velo", MySqlDbType.Int16).Value = v.No_velo;

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

                listPV.ItemsSource = listePiecesVelo;

                MainWindow.connexion.Close();
            }
        }

        /// <summary>
        /// Bouton créer le vélo : créer un vélo à partir des pièces qui le compose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreerVelo(object sender, RoutedEventArgs e)
        {
            int creer = 0;
            foreach (Piece p in listPV.Items)
            {
                if(p.Stock_piece >= p.Quantite)
                {
                    creer++;
                }
            }
            if (creer == listPV.Items.Count)
            {
                MainWindow.connexion.Open();

                foreach (Velo v in listV.SelectedItems)
                {
                    string maj = "UPDATE Bicyclette set stock_bicyclette =@stock"
                    + " where No_bicyclette = @nobicyclette";

                    MySqlCommand command = MainWindow.connexion.CreateCommand();
                    command.Parameters.Add("@nobicyclette", MySqlDbType.Int16).Value = v.No_velo;
                    command.Parameters.Add("@stock", MySqlDbType.Int16).Value = v.Stock_velo + 1;
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
                }
                MainWindow.connexion.Close();

                MainWindow.connexion.Open();

                
                foreach (Piece p in listPV.Items)
                {
                    string maj2 = "UPDATE Pièce set stock_pièce =@stockp where No_pièce = @nopiece";
                    MySqlCommand command2 = MainWindow.connexion.CreateCommand();

                    command2.Parameters.Add("@nopiece", MySqlDbType.VarChar).Value = p.No_piece;
                    command2.Parameters.Add("@stockp", MySqlDbType.Int16).Value = p.Stock_piece - p.Prix;
                    MessageBox.Show(p.ToString() + " " + " stock : " + Convert.ToString(p.Stock_piece - p.Prix));
                    command2.CommandText = maj2;
                    try
                    {
                        command2.ExecuteNonQuery();
                    }
                    catch (MySqlException er)
                    {
                        MessageBox.Show(" ErreurConnexion : " + er.ToString());
                        MainWindow.connexion.Close();
                        return;
                    }
                }

                MainWindow.connexion.Close();
                listePiecesVelo = new List<Piece>();
                listPV.ItemsSource = listePiecesVelo;
                AffichageGestionStocks();
            }
            else
            {
                MessageBox.Show("Impossible de créer ce vélo, les stocks sont insuffisants");
            }
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
        /// Génère la liste des pièces à recommander dans un fichier XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XML(object sender, RoutedEventArgs e)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Piece>));
            StreamWriter wr = new StreamWriter("stock_pieces.xml");
            xs.Serialize(wr, listeRecommander);
            wr.Close();
            MessageBox.Show("Le fichier a été généré");
        }
    }
}
