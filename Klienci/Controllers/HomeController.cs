using Klienci.Data;
using Klienci.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;
using MailKit.Net.Smtp;

namespace Klienci.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KlienciContext _contex;
        public HomeController(ILogger<HomeController> logger, KlienciContext contex)
        {
            _logger = logger;
            _contex = contex;
        }

        public IActionResult Index()
        {
            var data=_contex.klienci.ToList();
            return View(data);
        }
        public IActionResult DoEdycji()
        {
            var klient = _contex.klienci.ToList();
            return View(klient);
        }
        public IActionResult EdytujKlienta(int id)
        {
            var klient = _contex.klienci.Find(id);
            return View(klient);
        }

        [HttpPost]
        public IActionResult EdytujKlienta(Klient klient)
        {
            if (ModelState.IsValid)
            {
                _contex.klienci.Update(klient);
                _contex.SaveChanges();
                return RedirectToAction("DoEdytuj");
            }
            return View(klient);
        }
        [HttpPost]
        public IActionResult ZapiszEdytowanegoKlienta(Klient klient)
        {
            if (ModelState.IsValid)
            {
                var klientDoEdycji = _contex.klienci.Find(klient.Id);
                klientDoEdycji.Imie = klient.Imie;
                klientDoEdycji.Nazwisko = klient.Nazwisko;
                klientDoEdycji.Email = klient.Email;
                klientDoEdycji.NrTelefonu = klient.NrTelefonu;

                _contex.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(klient);
        }


        public IActionResult UsunKlienta(int id)
        {
            var klient = _contex.klienci.Find(id);
            _contex.klienci.Remove(klient);
            _contex.SaveChanges();
            return new EmptyResult();
        }


        public IActionResult DodajKlienta()
        {
            return View();
        }
        public IActionResult WyslijEmail()
        {
            var klient = _contex.klienci.ToList();
            return View(klient);
        }
        public IActionResult PrzygotowanieEmail(int id)
        {
            var klient = _contex.klienci.Find(id);
            return View(klient);
        }


        [HttpPost]
        public ActionResult DodajKlienta(Klient klient)
        {
            if (ModelState.IsValid)
            {
                _contex.klienci.Add(klient);
                _contex.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(klient);
        }
        [HttpPost]
        public IActionResult Wiadomosc(int klientId, string trescWiadomosci, string nazwaWiadomosci)
        {
            var klient = _contex.klienci.Find(klientId);

            var wiadomosc = new MimeMessage();
            wiadomosc.From.Add(new MailboxAddress("Twoja nazwa firmy", "1.com"));
            wiadomosc.To.Add(new MailboxAddress(klient.Imie + " " + klient.Nazwisko, klient.Email));
            wiadomosc.Subject = nazwaWiadomosci;
            wiadomosc.Body = new TextPart("plain")
            {
                Text = trescWiadomosci
            };

            using (var klientSmtp = new SmtpClient())
            {
                klientSmtp.Connect("localhost", 7102, SecureSocketOptions.StartTls);
                klientSmtp.Authenticate("1.com", "");
                klientSmtp.Send(wiadomosc);
                klientSmtp.Disconnect(true);
            }

            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}