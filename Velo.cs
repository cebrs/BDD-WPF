using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace BDDVersionWPF
{
    public class Velo : INotifyPropertyChanged
    {
        int no_velo;
        int stock_velo;
        string nom_velo;
        string grandeur_velo;
        string ligne_produit;
        string prix_bicyclette;
        DateTime date_intro;
        DateTime date_discon;
        int quantiteVendue;
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructeur
        public Velo(int no, int stock, string nom, string grandeur, string prix, string ligne, DateTime date1, DateTime date2)
        {
            no_velo = no;
            stock_velo = stock;
            nom_velo = nom;
            grandeur_velo = grandeur;
            prix_bicyclette = prix;
            ligne_produit = ligne;
            date_intro = date1;
            date_discon = date2;
        }
        public Velo(int no, int stock, string nom, string grandeur, string prix, string ligne, DateTime date1, DateTime date2, int quantiteVendue)
        {
            no_velo = no;
            stock_velo = stock;
            nom_velo = nom;
            grandeur_velo = grandeur;
            prix_bicyclette = prix;
            ligne_produit = ligne;
            date_intro = date1;
            date_discon = date2;
            this.quantiteVendue = quantiteVendue;
        }

        // Affichage
        public override string ToString()
        {
            return no_velo + " " + stock_velo + " " + nom_velo + " " + grandeur_velo + " " + ligne_produit
                + " " + date_intro + " " + date_discon;
        }

        //Propriétés
        public int No_velo
        {
            get { return no_velo; }
            set { no_velo = value; }
        }
        public int Stock_velo
        {
            get { return stock_velo; }
            set { stock_velo = value; OnPropertyChanged("Stock_velo"); }
        }
        public string Nom_velo
        {
            get { return nom_velo; }
            set { nom_velo = value; OnPropertyChanged("Nom_velo"); }
        }
        public string Grandeur_velo
        {
            get { return grandeur_velo; }
            set { grandeur_velo = value; OnPropertyChanged("Grandeur_velo"); }
        }
        public string Prix_bicyclette
        {
            get { return prix_bicyclette; }
            set { prix_bicyclette = value; OnPropertyChanged("Grandeur_velo"); }
        }
        public string Ligne_produit
        {
            get { return ligne_produit; }
            set { ligne_produit = value; OnPropertyChanged("Ligne_produit"); }
        }
        public DateTime Date_intro
        {
            get { return date_intro; }
            set { date_intro = value; OnPropertyChanged("Nom_velo"); }
        }
        public DateTime Date_discon
        {
            get { return date_discon; }
            set { date_discon = value; OnPropertyChanged("Nom_velo"); }
        }
        public int QuantiteVendue
        {
            get { return quantiteVendue; }
            set { quantiteVendue = value; OnPropertyChanged("QuantiteVendue"); }
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
        /// Permet de récupérer les vélos présents dans la BDD
        /// </summary>
        /// <returns></returns>
        public static List<Velo> AffichageVelo()
        {
            MainWindow.connexion.Open();
            List<Velo> listeVelos = new List<Velo>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_bicyclette, stock_bicyclette, nom_bicyclette, grandeur, prix_bicyclette, ligne_produit, date_intro, date_discontinuation"
                                + " FROM bicyclette ORDER BY no_bicyclette;";

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

                Velo v = new Velo(Convert.ToInt16(no_bicyclette), Convert.ToInt16(stock_bicyclette), nom_bicyclette, grandeur,
                    prix_bicyclette, ligne_produit, date_intro, date_discontinuation);
                listeVelos.Add(v);
            }
            MainWindow.connexion.Close();
            return listeVelos;
        }
    }
}
