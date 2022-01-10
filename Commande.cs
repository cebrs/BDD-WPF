using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDVersionWPF
{
    public class Commande
    {
        int no_commande;
        DateTime date_commande;
        DateTime date_livraison;
        string contient_pieces;
        string contient_velos;
        int prix;

        // Constructeur
        public Commande(int no_commande, DateTime date_commande, DateTime date_livraison, int prix)
        {
            this.no_commande = no_commande;
            this.date_commande = date_commande;
            this.date_livraison = date_livraison;
            this.prix = prix;
        }

        public Commande(int no_commande, DateTime date_commande, DateTime date_livraison, string pieces, string velos, int prix)
        {
            this.no_commande = no_commande;
            this.date_commande = date_commande;
            this.date_livraison = date_livraison;
            this.contient_pieces = pieces;
            this.contient_velos = velos;
            this.prix = prix;
        }

        public int No_commande
        {
            get { return no_commande; }
            set { no_commande = value; }
        }
        public DateTime Date_commande
        {
            get { return date_commande; }
            set { date_commande = value; }
        }
        public DateTime Date_livraison
        {
            get { return date_livraison; }
            set { date_livraison = value; }
        }
        public string Contient_pieces
        {
            get { return contient_pieces; }
            set { contient_pieces = value; }
        }
        public string Contient_velos
        {
            get { return contient_velos; }
            set { contient_velos = value; }
        }
        public int Prix
        {
            get { return prix; }
            set { prix = value; }
        }

        /// <summary>
        /// Récupère toutes les commandes de la BDD
        /// </summary>
        /// <returns></returns>
        static public List<Commande> AffichageCommande()
        {
            MainWindow.connexion.Open();
            List<Commande> listeCommandes = new List<Commande>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT c.no_commande, c.date_commande, c.date_livraison, cb.no_bicyclette, cp.No_pièce, c.prix"
                                + " FROM commande c LEFT JOIN contient_pièces cp ON cp.no_commande = c.no_commande LEFT JOIN contient_bicyclette cb" 
                                + " ON cb.no_commande = c.no_commande ORDER BY no_commande;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_commande = "";
            DateTime date_commande = new DateTime();
            DateTime date_livraison = new DateTime();
            string contient_pieces = "";
            string contient_velos = "";
            string prix = "";

            while (reader.Read())
            {
                no_commande = reader.GetString(0);
                date_commande = DateTime.Parse(reader.GetString(1));
                date_livraison = DateTime.Parse(reader.GetString(2));
                if (!reader.IsDBNull(4))
                {
                    contient_pieces = reader.GetString(4);
                }
                else
                {
                    contient_pieces = "";
                }
                if (!reader.IsDBNull(3))
                {
                    contient_velos = reader.GetString(3);
                }
                else
                {
                    contient_velos = "";
                }
                prix = reader.GetString(5);

                Commande c = new Commande(Convert.ToInt16(no_commande), date_commande, date_livraison, contient_pieces, contient_velos,
                    Convert.ToInt16(prix));
                listeCommandes.Add(c);
            }
            MainWindow.connexion.Close();
            return listeCommandes;
        }
    }
}
