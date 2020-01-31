using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjeDetaljiVM
    {
        public int SkolaID { get; set; }
        public string SkolaNaziv { get; set; }
        public int Razred { get; set; }

        public List<Row> Takmicenja { get; set; }

        public TakmicenjeDetaljiVM()
        {
            Takmicenja = new List<Row>();
        }

        public class Row {

            public int TakmicenjeID { get; set; }
            public string Predmet { get; set; }
            public int Razred { get; set; }
            public string Datum { get; set; }
            public int BrojUcesnikaKojiNisuPristupili { get; set; }
            public int MaxBodova { get; set; }
            //NajboljiUcesnik
            public string Skola { get; set; }
            public string Odjeljenje { get; set; }
            public string ImePrezime { get; set; }

        }
    }
}
