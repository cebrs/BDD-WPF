using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace BDDVersionWPF
{
    public class BoutiqueSpecialisee : INotifyPropertyChanged
    {
        int no_compagnie;
        string nom_compagnie;
        string adresse_compagnie;
        string telephone_compagnie;
        string courriel_compagnie;
        int remise;
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructeur
        public BoutiqueSpecialisee(int no, string nom, string adresse, string telephone, 
            string courriel, int remise)
        {
            this.no_compagnie = no;
            this.nom_compagnie = nom;
            this.adresse_compagnie = adresse;
            this.telephone_compagnie = telephone;
            this.courriel_compagnie = courriel;
            this.remise = remise;
        }

        // Affichage
        public override string ToString()
        {
            return no_compagnie + " " + nom_compagnie + " " + adresse_compagnie + " " 
                + telephone_compagnie + " " + courriel_compagnie + " " + remise;
        }

        //Propriétés
        public int No_compagnie
        {
            get { return no_compagnie; }
            set { no_compagnie = value; }
        }
        public string Nom_compagnie
        {
            get { return nom_compagnie; }
            set { nom_compagnie = value; OnPropertyChanged("Nom_compagnie"); }
        }
        public string Adresse_compagnie
        {
            get { return adresse_compagnie; }
            set { adresse_compagnie = value; OnPropertyChanged("Adresse_compagnie"); }
        }
        public string Telephone_compagnie
        {
            get { return telephone_compagnie; }
            set { telephone_compagnie = value; OnPropertyChanged("Telephone_compagnie"); }
        }
        public string Courriel_compagnie
        {
            get { return courriel_compagnie; }
            set { courriel_compagnie = value; OnPropertyChanged("Courriel_compagnie"); }
        }
        public int Remise
        {
            get { return remise; }
            set { remise = value; OnPropertyChanged("Remise"); }
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
        /// Récupère toutes les boutiques spécialisées de la BDD
        /// </summary>
        /// <returns></returns>
        public static List<BoutiqueSpecialisee> AffichageBS()
        {
            MainWindow.connexion.Open();
            List<BoutiqueSpecialisee> listeBS = new List<BoutiqueSpecialisee>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_compagnie, nom_compagnie, adresse_compagnie, telephone_compagnie, courriel_compagnie, remise"
                                + " FROM boutique_spécialisé ORDER BY no_compagnie;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_compagnie = "";
            string nom_compagnie = "";
            string adresse_compagnie = "";
            string telephone_compagnie = "";
            string courriel_compagnie = "";
            string remise = "";

            while (reader.Read())
            {
                no_compagnie = reader.GetString(0);
                nom_compagnie = reader.GetString(1);
                adresse_compagnie = reader.GetString(2);
                telephone_compagnie = reader.GetString(3);
                courriel_compagnie = reader.GetString(4);
                remise = reader.GetString(5);

                BoutiqueSpecialisee bs = new BoutiqueSpecialisee(Convert.ToInt16(no_compagnie),
                    nom_compagnie, adresse_compagnie, telephone_compagnie, courriel_compagnie, 
                    Convert.ToInt16(remise));
                listeBS.Add(bs);
            }
            MainWindow.connexion.Close();
            return listeBS;
        }
    }
}
