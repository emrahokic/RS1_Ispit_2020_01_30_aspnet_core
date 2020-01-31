using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class RezultatiDetaljiVM
    {
        public int SkolaID { get; set; }
        public string SkolaNaziv { get; set; }
        public string Predmet { get; set; }
        public int Razred { get; set; }
        public bool Zakljuceno { get; set; }
        public string Datum { get; set; }
        public int TakmicenjeID { get; set; }
        public List<Row> Ucesnici { get; set; }

        public RezultatiDetaljiVM()
        {
            Ucesnici = new List<Row>();
        }

        public class Row
        {

            public int UcesnikID { get; set; }
            public bool Pristupio { get; set; }
            public string PristupioString { get { return Pristupio ? "DA" : "NE"; }  }
            public int Bodovi { get; set; }
            public string Skola { get; set; }
            public string Odjeljenje { get; set; }
            public int BrojUDnevniku { get; set; }

        }
    }
}
