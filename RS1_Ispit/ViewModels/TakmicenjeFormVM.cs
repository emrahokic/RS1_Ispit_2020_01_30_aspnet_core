
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_Ispit_asp.net_core.ViewModels
{
    public class TakmicenjeFormVM
    {
        public int SkolaID { get; set; }
        public string Razred { get; set; }
        public List<SelectListItem> Skole { get; set; }
        public List<SelectListItem> Razredi { get; set; }

        public TakmicenjeFormVM()
        {
            Skole = new List<SelectListItem>();
            Razredi = new List<SelectListItem>();

            //Dodavanje razreda u listu za DropDown
            Razredi.Add(new SelectListItem()
            {
                Text = "Odaberite Razred",
                Value = null,
                Disabled = true,
                Selected = true
            });
            Razredi.Add(new SelectListItem()
            {
                Text = "Svi razredi",
                Value = "0"
            });
            for (int i = 1; i < 5; i++)
            {
                Razredi.Add(new SelectListItem()
                {
                    Value = i.ToString(),
                    Text = i.ToString(),
                });
            }
        }
    }
}
