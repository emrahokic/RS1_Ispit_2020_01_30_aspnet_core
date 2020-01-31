﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RS1_Ispit_asp.net_core.EF;
using RS1_Ispit_asp.net_core.EntityModels;
using RS1_Ispit_asp.net_core.ViewModels;

namespace RS1_Ispit_asp.net_core.Controllers
{

    public  class TakmicenjeFormVM
    {
        public int SkolaID { get; set; }
        public string Razred { get; set; }
        public List<SelectListItem> Skole { get; set; }
        public List<SelectListItem> Razredi { get; set; }

        public TakmicenjeFormVM()
        {
            Skole = new List<SelectListItem>();
            Razredi = new List<SelectListItem>();
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

    public class TakmicenjeController : Controller
    {
        private readonly MojContext _db;

        public TakmicenjeController(MojContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //VM Se nalazi vise kontrolera :D 
            TakmicenjeFormVM model = new TakmicenjeFormVM();

            model.Skole = _db.Skola.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Naziv
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(TakmicenjeFormVM model)
        {
            if (model==null && model.SkolaID == null)
            {
                return RedirectToAction(nameof(Index));
            }

          
            //Nakon submitanja forme i biranja skole i razreda koji je opcionalan pravimo redirekciju na akciju Detalji te saljemo parametre SkolaID i Razred ( ?? znaci ako je model.Razred null stavi "0" )
            return RedirectToAction(nameof(Detalji),new { SkolaID= model.SkolaID, Razred = model.Razred ?? "0" });
        }

        public IActionResult Detalji(int SkolaID,int Razred)
        {
            TakmicenjeDetaljiVM model = _db.Skola.Where(x => x.Id == SkolaID).Select(x=>new TakmicenjeDetaljiVM() {
            Razred = Razred ,
            SkolaNaziv = x.Naziv,
            SkolaID = x.Id
            }).FirstOrDefault();




            if (Razred == 0)
            {
                model.Takmicenja = _db.Takmicenje.Where(x => x.SkolaID == SkolaID).Select(x => new TakmicenjeDetaljiVM.Row()
                {

                    Datum = x.Datum.ToString("dd.MM.yyyy"),
                    Razred = x.Razred,
                    BrojUcesnikaKojiNisuPristupili = x.Ucesnici.Where(y => y.Pristupio == false).Count(),
                    /* iz liste Ucesnici uzimamo Max vrijednost za bodove te provjeravamo koji ucesnik ima taj broj bodova i njega uzimamo za najboljek ucesnika*/
                    Skola = x.Ucesnici.Where(y => y.Bodovi == x.Ucesnici.Max(i => i.Bodovi)).Select(y => y.OdjeljenjeStavka.Odjeljenje.Skola.Naziv).FirstOrDefault(),
                    Odjeljenje = x.Ucesnici.Where(y => y.Bodovi == x.Ucesnici.Max(i => i.Bodovi)).Select(y => y.OdjeljenjeStavka.Odjeljenje.Oznaka).FirstOrDefault(),
                    ImePrezime = x.Ucesnici.Where(y => y.Bodovi == x.Ucesnici.Max(i => i.Bodovi)).Select(y => y.OdjeljenjeStavka.Ucenik.ImePrezime).FirstOrDefault(),
                    Predmet = x.Predmet.Naziv,
                    TakmicenjeID = x.TakmicenjeID
                }).ToList();

            }
            else
            {
                model.Takmicenja = _db.Takmicenje.Where(x => x.SkolaID == SkolaID &&  x.Razred == Razred).Select(x => new TakmicenjeDetaljiVM.Row()
                {

                    Datum = x.Datum.ToString("dd.MM.yyyy"),
                    Razred = x.Razred,
                    BrojUcesnikaKojiNisuPristupili = x.Ucesnici.Where(y => y.Pristupio == false).Count(),
                    /* iz liste Ucesnici uzimamo Max vrijednost za bodove te provjeravamo koji ucesnik ima taj broj bodova i njega uzimamo za najboljek ucesnika*/
                    Skola = x.Ucesnici.Where(y => y.Bodovi == x.Ucesnici.Max(i => i.Bodovi)).Select(y => y.OdjeljenjeStavka.Odjeljenje.Skola.Naziv).FirstOrDefault(),
                    Odjeljenje = x.Ucesnici.Where(y => y.Bodovi == x.Ucesnici.Max(i => i.Bodovi)).Select(y => y.OdjeljenjeStavka.Odjeljenje.Oznaka).FirstOrDefault(),
                    ImePrezime = x.Ucesnici.Where(y => y.Bodovi == x.Ucesnici.Max(i => i.Bodovi)).Select(y => y.OdjeljenjeStavka.Ucenik.ImePrezime).FirstOrDefault(),
                    Predmet = x.Predmet.Naziv,
                    TakmicenjeID = x.TakmicenjeID
                }).ToList();

            }



            return View(model);
        }

        public IActionResult DodajTakmicenje(int SkolaID)
        {

            TakmicenjeDodajVM dodajVM = _db.Skola.Where(x => x.Id == SkolaID).Select(x => new TakmicenjeDodajVM()
            {
                SkolaID = x.Id,
                Skola = x.Naziv

            }).FirstOrDefault();

           
            dodajVM.Predmeti = _db.Predmet.GroupBy(x=>x.Naziv).Select(x => x.First()).Select(x=> new SelectListItem() { 
              Text = x.Naziv,
              Value = x.Naziv.ToString()
            }).ToList();



            return View(dodajVM);
        }

        [HttpPost]
        public IActionResult DodajTakmicenje(TakmicenjeDodajVM model)
        {
            //Dodavnanje novog takmicenja

            //Preuzimanje predmeta po nazivu i za izabranu godinu 
            Predmet p = _db.Predmet.Where(x => x.Naziv.Equals(model.Predmet) && x.Razred == model.Razred).FirstOrDefault();

            Takmicenje takmicenje = new Takmicenje()
            {
                SkolaID = model.SkolaID,
                PredmetID = p.Id,
                Razred = model.Razred,
                Datum = model.Datum
            };

            //Dodavanje Ucenika na Takmicenje


            List<TakmicenjeUcesnik> temp = new List<TakmicenjeUcesnik>();

            //preuzimanje ucenika i kreiranje liste Za TakmicenjeUcesnik, uslov : učenici iz svih škola koji imaju zaključnu ocjenu 5 za odabrani predmet

            temp = _db.DodjeljenPredmet.Where(x => x.Predmet.Id == p.Id && x.ZakljucnoKrajGodine == 5).Select(x => new TakmicenjeUcesnik()
            {
                OdjeljenjeStavkaID = x.OdjeljenjeStavkaId,
                Pristupio = false,
                Bodovi = 0
            }).ToList();

            takmicenje.Ucesnici = new List<TakmicenjeUcesnik>();


            // Uslov :  iskjučiti učenike čiji je prosjek ocjena iz svih  predmeta manji od 4.0, provjera prosjeka za svaki dodjeljeniPredmet (DodjeljeniPredmet sadrzi sve informacije koje su potrebne za dodavanje ucenika na Takmicenje(Razred,Ocjene) za jednog ucenika(OdjeljenjeStavka))
            foreach (var item in temp)
            {
                if (_db.DodjeljenPredmet.Where(x=>x.OdjeljenjeStavkaId == item.OdjeljenjeStavkaID && x.OdjeljenjeStavka.Odjeljenje.Razred == takmicenje.Razred).Select(x=>x.ZakljucnoKrajGodine).Average()>4)
                {
                    takmicenje.Ucesnici.Add(item);
                }
            }

            _db.Takmicenje.Add(takmicenje);
            _db.SaveChanges();

            return RedirectToActionPermanent(nameof(Detalji), new { SkolaID = model.SkolaID });
        }

        public IActionResult Rezultati(int TakmicenjeID)
        {

            RezultatiDetaljiVM model = _db.Takmicenje.Where(x => x.TakmicenjeID == TakmicenjeID).Select(x => new RezultatiDetaljiVM() {
                Predmet = x.Predmet.Naziv,
                TakmicenjeID = x.TakmicenjeID,
                Razred = x.Razred,
                Zakljuceno = x.Zakljuceno,
                SkolaID = x.SkolaID,
                Datum = x.Datum.ToString("dd.MM.yyyy"),
                SkolaNaziv = x.Skola.Naziv
            }).FirstOrDefault();

            return View(model);
        }

        public IActionResult RezultatiPV(int TakmicenjeID)
        {
            RezultatiDetaljiVM model = new RezultatiDetaljiVM();
            model.Zakljuceno = _db.Takmicenje.Where(x => x.TakmicenjeID == TakmicenjeID).Select(x => x.Zakljuceno).FirstOrDefault();

            model.Ucesnici = _db.TakmicenjeUcesnik.Where(y => y.TakmicenjeID == TakmicenjeID).Select(y => new RezultatiDetaljiVM.Row()
            {
                Bodovi = y.Bodovi,
                Skola = y.OdjeljenjeStavka.Odjeljenje.Skola.Naziv,
                BrojUDnevniku = y.OdjeljenjeStavka.BrojUDnevniku,
                Odjeljenje = y.OdjeljenjeStavka.Odjeljenje.Oznaka,
                Pristupio = y.Pristupio,
                UcesnikID = y.TakmicenjeUcesnikID
            }).ToList();

            return PartialView(model);
        }

        public IActionResult Prisustvo(int UcesnikID)
        {

            TakmicenjeUcesnik ucesnik = _db.TakmicenjeUcesnik.Where(x => x.TakmicenjeUcesnikID == UcesnikID).FirstOrDefault();

            if (ucesnik!=null)
            {
                ucesnik.Pristupio = !ucesnik.Pristupio;
            }

            _db.SaveChanges();

            return RedirectToAction("RezultatiPV", "Takmicenje",new { TakmicenjeID = ucesnik.TakmicenjeID});
        }

        public IActionResult Zakljucaj(int TakmicenjeID)
        {

            Takmicenje takmicenje= _db.Takmicenje.Where(x => x.TakmicenjeID == TakmicenjeID).FirstOrDefault();

            if (takmicenje != null)
            {
                takmicenje.Zakljuceno = true;
            }

            _db.SaveChanges();

            return RedirectToAction("Rezultati", "Takmicenje", new { TakmicenjeID = takmicenje.TakmicenjeID });
        }
        public IActionResult Uredi(int UcesnikID)
        {

            TakmicenjeUcesnik ucesnik = _db.TakmicenjeUcesnik.Where(x => x.TakmicenjeUcesnikID == UcesnikID).FirstOrDefault();

            if (ucesnik != null)
            {
                ucesnik.Pristupio = !ucesnik.Pristupio;
            }

            _db.SaveChanges();

            return RedirectToActionPermanent(nameof(RezultatiPV), new { TakmicenjeID = ucesnik.TakmicenjeID });
        }
    }
}