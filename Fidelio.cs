using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BDDVersionWPF
{
    public class Fidelio
    {
        int no_programme;
        string description;
        int cout;
        int duree;
        int rabais;

        //Constructeur
        public Fidelio(int no, string desc, int prix, int temps, int rabais)
        {
            this.no_programme = no;
            this.description = desc;
            this.cout = prix;
            this.duree = temps;
            this.rabais = rabais;
        }

        //Propriétés
        public int No_programme
        {
            get { return no_programme; }
            set { no_programme = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int Cout
        {
            get { return cout; }
            set { cout = value; }
        }
        public int Duree
        {
            get { return duree; }
            set { duree = value; }
        }
        public int Rabais
        {
            get { return rabais; }
            set { rabais = value; }
        }

        //Méthodes
        /// <summary>
        /// Récupère tous les types de Fidélio présents dans la BDD
        /// </summary>
        /// <returns></returns>
        public static List<Fidelio> AffichageFidelio()
        {
            MainWindow.connexion.Open();
            List<Fidelio> fidelio = new List<Fidelio>();

            MySqlCommand command = MainWindow.connexion.CreateCommand();
            command.CommandText = " SELECT no_fidélio, description_fidélio, cout_fidélio, durée, rabais"
                                + " FROM fidélio ORDER BY no_fidélio;";

            MySqlDataReader reader;
            reader = command.ExecuteReader();

            string no = "";
            string description = "";
            string cout = "";
            string duree = "";
            string rabais = "";

            while (reader.Read())
            {
                no = reader.GetString(0);
                description = reader.GetString(1);
                cout = reader.GetString(2);
                duree = reader.GetString(3);
                rabais = reader.GetString(4);
                Fidelio f = new Fidelio(Convert.ToInt16(no), description, Convert.ToInt16(cout), Convert.ToInt16(duree), Convert.ToInt16(rabais));
                fidelio.Add(f);
            }
            MainWindow.connexion.Close();
            return fidelio;
        }
    }
}
