using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace BDDVersionWPF
{
    public class ClientParticulier : INotifyPropertyChanged
    {
        int no_client;
        string nom_client;
        string prenom_client;
        string adresse_client;
        string telephone_client;
        string courriel_client;
        int no_fidelio;
        DateTime fin_fidelio;
        int nbCommande;
        public event PropertyChangedEventHandler PropertyChanged;

        //Constructeurs
        public ClientParticulier(int no_client, string nom_client, string prenom_client,
            string adresse_client, string telephone_client, string courriel_client, int no_fidelio)
        {
            this.no_client = no_client;
            this.nom_client = nom_client;
            this.prenom_client = prenom_client;
            this.adresse_client = adresse_client;
            this.telephone_client = telephone_client;
            this.courriel_client = courriel_client;
            this.no_fidelio = no_fidelio;
        }
        public ClientParticulier(int no_client, string nom_client, string prenom_client,
            string adresse_client, string telephone_client, string courriel_client, int no_fidelio, DateTime fin_fidelio)
        {
            this.no_client = no_client;
            this.nom_client = nom_client;
            this.prenom_client = prenom_client;
            this.adresse_client = adresse_client;
            this.telephone_client = telephone_client;
            this.courriel_client = courriel_client;
            this.no_fidelio = no_fidelio;
            this.fin_fidelio = fin_fidelio;
        }
        public ClientParticulier(int no_client, string nom_client, string prenom_client, int no_fidelio, int nbCommande)
        {
            this.no_client = no_client;
            this.nom_client = nom_client;
            this.prenom_client = prenom_client;
            this.no_fidelio = no_fidelio;
            this.nbCommande = nbCommande;
        }
        public ClientParticulier(int no_client, string nom_client, string prenom_client, int no_fidelio, DateTime fin_fidelio)
        {
            this.no_client = no_client;
            this.nom_client = nom_client;
            this.prenom_client = prenom_client;
            this.no_fidelio = no_fidelio;
            this.fin_fidelio = fin_fidelio;
        }


        // Affichage
        public override string ToString()
        {
            return no_client + " " + nom_client + " " + prenom_client + " " 
                + adresse_client + " " + telephone_client + " " + courriel_client + " " + no_fidelio;
        }


        //Propriétés
        public int No_client
        {
            get { return no_client; }
            set { no_client = value; }
        }
        public string Nom_client
        {
            get { return nom_client; }
            set { nom_client = value; OnPropertyChanged("Nom_client"); }
        }
        public string Prenom_client
        {
            get { return prenom_client; }
            set { prenom_client = value; OnPropertyChanged("Prenom_client"); }
        }
        public string Adresse_client
        {
            get { return adresse_client; }
            set { adresse_client = value; OnPropertyChanged("Adresse_client"); }
        }
        public string Telephone_client
        {
            get { return telephone_client; }
            set { telephone_client = value; OnPropertyChanged("Telephone_client"); }
        }
        public string Courriel_client
        {
            get { return courriel_client; }
            set { courriel_client = value; OnPropertyChanged("Courriel_client"); }
        }
        public int No_fidelio
        {
            get { return no_fidelio; }
            set { no_fidelio = value; OnPropertyChanged("No_fidelio"); }
        }
        public DateTime Fin_fidelio
        {
            get { return fin_fidelio; }
            set { fin_fidelio = value; OnPropertyChanged("Fin_fidelio"); }
        }
        public int NbCommande
        {
            get { return nbCommande; }
            set { nbCommande = value; OnPropertyChanged("NbCommande"); }
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
        /// Récupère tous les clients particuliers de la BDD
        /// </summary>
        /// <returns></returns>
        public static List<ClientParticulier> AffichageParticulier()
        {
            MainWindow.connexion.Open();
            List<ClientParticulier> listeCParticuliers = new List<ClientParticulier>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_client, nom_client, prenom_client, adresse_client, telephone_client, courriel_client, "
                + "no_fidélio FROM individu ORDER BY no_client;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no_client = "";
            string nom_client = "";
            string prenom_client = "";
            string adresse_client = "";
            string telephone_client = "";
            string courriel_client = "";
            string no_fidelio = "";

            while (reader.Read())
            {
                no_client = reader.GetString(0);
                nom_client = reader.GetString(1);
                prenom_client = reader.GetString(2);
                adresse_client = reader.GetString(3);
                telephone_client = reader.GetString(4);
                courriel_client = reader.GetString(5);
                if (reader.IsDBNull(6))
                {
                    no_fidelio = null;
                }
                else
                {

                    no_fidelio = reader.GetString(6);
                }
                ClientParticulier cp = new ClientParticulier(Convert.ToInt16(no_client), nom_client, prenom_client, adresse_client, telephone_client, 
                    courriel_client, Convert.ToInt16(no_fidelio));
                listeCParticuliers.Add(cp);
            }
            MainWindow.connexion.Close();
            return listeCParticuliers;
        }

    }
}
