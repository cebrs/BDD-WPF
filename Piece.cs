using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace BDDVersionWPF
{
    public class Piece : INotifyPropertyChanged
    {
        string no_piece;
        int stock_piece;
        string description_piece;
        int quantite;
        int prix;
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructeurs
        public Piece() { }
        public Piece(string no, int stock, string description, int prix)
        {
            no_piece = no;
            stock_piece = stock;
            description_piece = description;
            this.prix = prix;
        }
        public Piece(string no_piece, int stock_piece, string description_piece, int quantite, int prix)
        {
            this.no_piece = no_piece;
            this.stock_piece = stock_piece;
            this.description_piece = description_piece;
            this.quantite = quantite;
            this.prix = prix;
        }

        // Affichage
        public override string ToString()
        {
            return no_piece + " " + stock_piece + " " + description_piece;
        }

        //Propriétés
        public string No_piece
        {
            get { return no_piece; }
            set { no_piece = value; }
        }
        public int Stock_piece
        {
            get { return stock_piece; }
            set { stock_piece = value; OnPropertyChanged("Stock_piece");}
        }
        public string Description_piece
        {
            get { return description_piece; }
            set { description_piece = value; OnPropertyChanged("Description_piece"); }
        }
        public int Quantite
        {
            get { return quantite; }
            set { quantite = value; OnPropertyChanged("Quantite"); }
        }
        public int Prix
        {
            get { return prix; }
            set { prix = value; OnPropertyChanged("Quantite"); }
        }


        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        //Méthodes
        /// <summary>
        /// Permet de récupérer la liste des pièces dans la BDD
        /// </summary>
        /// <returns></returns>
        public static List<Piece> AffichagePiece()
        {
            MainWindow.connexion.Open();
            List<Piece> listesPieces = new List<Piece>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_pièce, stock_pièce, description_pièces, prix"
                                + " FROM pièce ORDER BY no_pièce;";

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
                listesPieces.Add(p);
            }
            MainWindow.connexion.Close();
            return listesPieces;
        }

    }
}
