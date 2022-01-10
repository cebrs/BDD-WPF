using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace BDDVersionWPF
{
    public class Fournisseur : INotifyPropertyChanged
    {
        string siret;
        string nom_entreprise;
        string contact_entreprise;
        string adresse_entreprise;
        int libelle;
        string no_catalogue;
        int delai_livraison;
        int prix_piece;

        public event PropertyChangedEventHandler PropertyChanged;

        //Constructeurs
        public Fournisseur(string siret, string nom_entreprise, string contact_entreprise,
            string adresse_entreprise, int libelle)
        {
            this.siret = siret; 
            this.nom_entreprise = nom_entreprise;
            this.contact_entreprise = contact_entreprise;
            this.adresse_entreprise = adresse_entreprise;
            this.libelle = libelle;
        }
        public Fournisseur(string siret, string nom_entreprise, string catalogue, int delai, int prix, int libelle)
        {
            this.siret = siret;
            this.nom_entreprise = nom_entreprise;
            this.no_catalogue = catalogue;
            this.delai_livraison = delai;
            this.prix_piece = prix;
            this.libelle = libelle;
        }

        // Affichage
        public override string ToString()
        {
            return siret + " " + nom_entreprise + " " + contact_entreprise + " " + adresse_entreprise + " " + libelle;
        }

        //Propriétés
        public string Siret
        {
            get { return siret; }
            set { siret = value; }
        }
        public string Nom_entreprise
        {
            get { return nom_entreprise; }
            set { nom_entreprise = value; OnPropertyChanged("Nom_entreprise"); }
        }
        public string Contact_entreprise
        {
            get { return contact_entreprise; }
            set { contact_entreprise = value; OnPropertyChanged("Contact_entreprise"); }
        }
        public string Adresse_entreprise
        {
            get { return adresse_entreprise; }
            set { adresse_entreprise = value; OnPropertyChanged("Adresse_entreprise"); }
        }
        public int Libelle
        {
            get { return libelle; }
            set { libelle = value; OnPropertyChanged("Libelle"); }
        }

        public string No_catalogue
        {
            get { return no_catalogue; }
            set { no_catalogue = value; OnPropertyChanged("No_catalogue"); }
        }
        public int Delai_livraison
        {
            get { return delai_livraison; }
            set { delai_livraison = value; OnPropertyChanged("Delai_livraison"); }
        }
        public int Prix_piece
        {
            get { return prix_piece; }
            set { prix_piece = value; OnPropertyChanged("Prix_piece"); }
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
        /// Récupère tous les fournisseurs de la BDD
        /// </summary>
        /// <returns></returns>
        public static List<Fournisseur> AffichageFournisseur()
        {
            MainWindow.connexion.Open();
            List<Fournisseur> listeFournisseur = new List<Fournisseur>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT siret, nom_entreprise, contact_entreprise, adresse_entreprise, libellé"
                                + " FROM fournisseur ORDER BY siret;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string siret = "";
            string nom_entreprise = "";
            string contact_entreprise = "";
            string adresse_entreprise = "";
            string libelle = "";

            while (reader.Read())
            {
                siret = reader.GetString(0);
                nom_entreprise = reader.GetString(1);
                contact_entreprise = reader.GetString(2);
                adresse_entreprise = reader.GetString(3);
                libelle = reader.GetString(4);

                Fournisseur f = new Fournisseur(siret, nom_entreprise, contact_entreprise, adresse_entreprise, Convert.ToInt16(libelle));
                listeFournisseur.Add(f);
            }
            MainWindow.connexion.Close();
            return listeFournisseur;
        }

    }
}
