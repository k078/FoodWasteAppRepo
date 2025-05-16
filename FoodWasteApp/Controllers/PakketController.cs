using Microsoft.AspNetCore.Mvc;
using Core.Domain;
using FoodWasteApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;

namespace FoodWasteApp.Controllers
{
    public class PakketController : Controller
    {
        private readonly IPakketRepo _pakketRepo;
        private readonly IPakketService _pakketService;
        private readonly IKantineRepo _kantineRepo;
        private readonly IStudentRepo _studentRepo;
        private readonly IProductRepo _productRepo;
        private readonly IMedewerkerRepo _medewerkerRepo;
        private readonly FoodWasteDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public PakketController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IPakketRepo pakketRepo, IPakketService pakketService, IKantineRepo kantineRepo, IStudentRepo studentRepo, IProductRepo productRepo, IMedewerkerRepo medewerkerRepo, FoodWasteDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _pakketRepo = pakketRepo;
            _pakketService = pakketService;
            _kantineRepo = kantineRepo;
            _studentRepo = studentRepo;
            _productRepo = productRepo;
            _medewerkerRepo = medewerkerRepo;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Overzicht(Stad? stad, Maaltijd? maaltijd, bool? volwassen, bool? warm, string datumFilter)
        {
            var viewModel = new OverzichtViewModel();
            var kantines = _kantineRepo.GetKantines();
            var students = _studentRepo.GetStudents();
            var currentTime = DateTime.Now;
            var vandaag = DateTime.Today;
            var morgen = vandaag.AddDays(1);

            _pakketRepo.VerwijderVerlopenPakketten(currentTime);

            var pakkettenQuery = _pakketRepo.GetBeschikbarePakketten();

            // Bestaande filters
            if (stad.HasValue)
            {
                pakkettenQuery = pakkettenQuery.Where(p => p.stad == stad.Value);
            }

            if (maaltijd.HasValue)
            {
                pakkettenQuery = pakkettenQuery.Where(p => p.maaltijd == maaltijd.Value);
            }

            if (volwassen.HasValue)
            {
                pakkettenQuery = pakkettenQuery.Where(p => p.volwassen == volwassen.Value);
            }

            if (warm.HasValue)
            {
                pakkettenQuery = pakkettenQuery.Where(p => p.warm == warm.Value);
            }

            if (!string.IsNullOrEmpty(datumFilter))
            {
                if (datumFilter == "vandaag")
                {
                    pakkettenQuery = pakkettenQuery.Where(p => p.pickup.Date == vandaag);
                }
                else if (datumFilter == "morgen")
                {
                    pakkettenQuery = pakkettenQuery.Where(p => p.pickup.Date == morgen);
                }
            }

            var pakketten = pakkettenQuery.OrderBy(p => p.pickup).ToList();

            // Vul ViewBag met alle filteropties
            ViewBag.Steden = pakketten.Select(p => p.stad).Distinct().OrderBy(s => s);
            ViewBag.SelectedStad = stad;
            ViewBag.SelectedMaaltijd = maaltijd;
            ViewBag.SelectedVolwassen = volwassen;
            ViewBag.SelectedWarm = warm;
            ViewBag.SelectedDatumFilter = datumFilter;

            if (User.IsInRole("Student"))
            {
                var studentId = _studentRepo.GetStudentByEmail(User.Identity.Name).Id;
                ViewBag.StudentId = studentId;
                viewModel.lijst3 = pakketten;
            }
            else if (User.IsInRole("Medewerker"))
            {
                var medewerker = _medewerkerRepo.GetMedewerkerByEmail(User.Identity.Name);
                var kantine = medewerker.kantine;

                var pakkettenKantine = pakketten.Where(p => p.kantine == kantine).ToList();
                var restPakketten = pakketten.Where(p => p.kantine != kantine).ToList();

                viewModel = new OverzichtViewModel { lijst1 = pakkettenKantine, lijst2 = restPakketten };
                ViewBag.MedewerkerId = medewerker.Id;
                return View(viewModel);
            }

            ViewBag.Students = students;
            ViewBag.Kantines = kantines;
            return View(viewModel);
        }



        [HttpGet]
        [Authorize(Roles = "Student, Medewerker")]
        public IActionResult GereserveerdOverzicht()
        {
            if (User.IsInRole("Student"))
            {


                if (User == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var kantines = _kantineRepo.GetKantines();
                var students = _studentRepo.GetStudents();
                var studentId = _studentRepo.GetStudentByEmail(User.Identity.Name).Id;
                ViewBag.StudentId = studentId;
                var pakketten = _pakketRepo.GetGereserveerdePakketten(studentId);

                ViewBag.Students = students;
                ViewBag.Kantines = kantines;
                return View(pakketten);
            }
            else
            {
                if (User == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var kantines = _kantineRepo.GetKantines();
                var students = _studentRepo.GetStudents();
                var pakketten = _pakketRepo.GetPakketten().Where(p => p.gereserveerd != null).ToList();

                ViewBag.Students = students;
                ViewBag.Kantines = kantines;
                return View(pakketten);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult PakketDetails(int pakketId)
        {
             var pakket = _pakketRepo.GetPakketById(pakketId);
            if (pakket == null)
            {
                return NotFound();
            }

            var producten = _pakketRepo.GetPakketProducten(pakketId).ToList();

            var student = _studentRepo.GetStudentByEmail(User.Identity.Name);
            ViewBag.StudentId = student.Id;

            var isGereserveerd = _pakketRepo
                .GetGereserveerdePakketten(student.Id)
                .Any(p => p.Id == pakketId);

            ViewBag.IsGereserveerd = isGereserveerd;

            var pakketDetailsViewModel = new PakketDetailsViewModel
            {
                pakket = pakket,
                producten = producten
            };

            return View(pakketDetailsViewModel);
        }


        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult ReserveerPakket(int pakketId, int studentId)
        {
            try
            {
                var vandaag = DateTime.Today;
                var pakket = _pakketRepo.GetPakketById(pakketId);

                // Controleer of het pakket is verlopen
                if (pakket.pickUpMax < DateTime.Now)
                {
                    ViewBag.CustomError = "Dit pakket is verlopen!";
                    return View("Overzicht", _pakketRepo.GetBeschikbarePakketten());
                }
                var kantines = _kantineRepo.GetKantines();
                ViewBag.Kantines = kantines;
                var studentGeboortedatum = _studentRepo.GetStudentById(studentId).geboortedatum;
                var studentLeeftijd = DateTime.Now.Year - studentGeboortedatum.Year;
                var reservatiedatum = pakket.pickup.Date;
                var bestaandReservatie = _pakketRepo.GetGereserveerdePakketten(studentId)
                    .Where(p => p.pickup.Date == reservatiedatum)
                    .ToList();
                var viewModel = new OverzichtViewModel();
                viewModel.lijst3 = _pakketRepo.GetBeschikbarePakketten().OrderBy(p=>p.pickup).ToList();
                ViewBag.StudentId = _studentRepo.GetStudentByEmail(User.Identity.Name).Id;
                if(pakket.volwassen==true && studentLeeftijd < 18)
                {
                    ViewBag.CustomError = "Je moet minimaal 18 jaar zijn om dit pakket te reserveren!";
                    return View("Overzicht", viewModel);
                }
                if (bestaandReservatie.Count() >= 1)
                {
                    ViewBag.CustomError = "Je hebt al een reservering staan voor deze datum!";
                    return View("Overzicht", viewModel);
                }

                if (!_pakketService.reserveerPakket(pakketId, studentId))
                {
                    ViewBag.CustomError = "Pakket is niet meer beschikbaar!";
                    return View("Overzicht", viewModel);
                }
                return RedirectToAction("GereserveerdOverzicht");
            }
            catch (Exception ex)
            {
                ViewBag.CustomError = ex.Message;
                return RedirectToAction("Overzicht", ex.Message);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Student")]
        public IActionResult CancelReservering(int pakketId, int studentId)
        {
            var student = _studentRepo.GetStudentByEmail(User.Identity.Name);
            if (student.Id != studentId)
            {
                return RedirectToAction("GereserveerdOverzicht");
            }

            var pakket = _pakketRepo.GetPakketById(pakketId);
            if (pakket.gereserveerd == null)
            {
                return RedirectToAction("GereserveerdOverzicht");
            }

            if (pakket.gereserveerd.Id != studentId)
            {
                return RedirectToAction("GereserveerdOverzicht");
            }

            pakket.gereserveerd = null;
            _pakketRepo.UpdatePakket(pakket);
            return RedirectToAction("GereserveerdOverzicht");

        }
        [HttpGet]
        [Authorize(Roles = "Medewerker")]
        public IActionResult AddPakket()
        {
            var medewerker = _medewerkerRepo.GetMedewerkerByEmail(User.Identity.Name);
            var kantineIdMedewerker = medewerker.kantineId;
            var kantineMedewerker = _kantineRepo.GetKantineById(kantineIdMedewerker);
            var kantineOndersteuntWarmeMaaltijden = kantineMedewerker.warm;

            var kantines = _kantineRepo.GetKantines();
            ViewBag.Kantines = kantines;
            var producten = _productRepo.GetProducten();

            var addPakketViewModel = new AddPakketViewModel
            {
                producten = producten.ToList(),
                pickup = DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute),
                pickUpMax = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0),
                warm = kantineOndersteuntWarmeMaaltijden ? false : false,
                KantineOndersteuntWarmeMaaltijden = kantineOndersteuntWarmeMaaltijden
            };

            return View(addPakketViewModel);
        }



        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public IActionResult AddPakket(AddPakketViewModel addPakketViewModel, List<int>? selectedProducten)
        {
            try
            {
                var medewerker = _medewerkerRepo.GetMedewerkerByEmail(User.Identity.Name);
                var kantiness = _kantineRepo.GetKantines();
                var mederkerLocatie = medewerker.kantine.locatie;
                var producten = _productRepo.GetProducten();
                var selectedProductenList = selectedProducten != null
                    ? producten.Where(p => selectedProducten.Contains(p.Id)).ToList()
                    : new List<Product>();

                bool isVolwassen = selectedProductenList.Any(p => p.alcohol);
                var kantine = medewerker.kantine;
                var currentTime = DateTime.Now;
                var timeDifference = addPakketViewModel.pickup - currentTime;

                // Validatie
                if (string.IsNullOrWhiteSpace(addPakketViewModel.titel))
                {
                    ModelState.AddModelError("titel", "Titel is verplicht");
                }
                if (addPakketViewModel.pickup == null)
                {
                    ModelState.AddModelError("pickup", "Ophaaltijd is verplicht");
                }
                if (addPakketViewModel.pickUpMax == null)
                {
                    ModelState.AddModelError("pickUpMax", "PickupMax is verplicht");
                }
                if (addPakketViewModel.prijs == 0 || addPakketViewModel.prijs < 0.50)
                {
                    ModelState.AddModelError("prijs", "Prijs moet minimaal 50 cent zijn");
                }
                if (selectedProductenList.Count == 0)
                {
                    ModelState.AddModelError("producten", "Selecteer minimaal één product");
                }

                if (addPakketViewModel.pickup != null && addPakketViewModel.pickup < currentTime)
                {
                    ModelState.AddModelError("pickup", "Ophaaltijd moet in de toekomst liggen");
                }
                if (addPakketViewModel.pickUpMax != null && addPakketViewModel.pickUpMax < addPakketViewModel.pickup)
                {
                    ModelState.AddModelError("pickUpMax", "PickupMax moet later zijn dan ophaaltijd");
                }

                if (ModelState.IsValid)
                {
                    if (timeDifference.TotalHours < 48 && addPakketViewModel.pickup >= currentTime)
                    {
                        // ✅ Alleen foutmelding als maaltijd warm is, maar kantine GEEN warme maaltijden ondersteunt
                        if (!addPakketViewModel.warm || (addPakketViewModel.warm && kantine.warm))
                        {
                            var pakket = new Pakket
                            {
                                titel = addPakketViewModel.titel,
                                pickup = addPakketViewModel.pickup,
                                pickUpMax = addPakketViewModel.pickUpMax,
                                prijs = addPakketViewModel.prijs,
                                maaltijd = addPakketViewModel.maaltijd,
                                stad = kantine.stad,
                                volwassen = isVolwassen,
                                kantine = kantine,
                                producten = selectedProductenList,
                                warm = addPakketViewModel.warm
                            };

                            if (pakket.pickUpMax > pakket.pickup)
                            {
                                _pakketRepo.AddPakket(pakket);
                                return RedirectToAction("Overzicht");
                            }
                            else
                            {
                                ModelState.AddModelError("pickupMax", "PickupMax moet later zijn dan pickup!");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("warm", "Uw kantine ondersteunt geen warme maaltijden!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("pickup", "Ophaaltijd mag maar 2 dagen vanaf vandaag zijn!");
                    }
                }

                // Bij fout: laad opnieuw
                var kantines = _kantineRepo.GetKantines();
                addPakketViewModel.producten = producten.ToList();
                ViewBag.Kantines = kantines;
                return View(addPakketViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("CustomError", "Er is iets misgegaan bij het toevoegen van een nieuw pakket: " + ex.Message);
                var kantines = _kantineRepo.GetKantines();
                ViewBag.Kantines = kantines;
                return View(addPakketViewModel);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public IActionResult DeletePakket(int id)
        {
            _pakketRepo.DeletePakket(id);
            return RedirectToAction("Overzicht");
        }
        [HttpGet]
        [Authorize(Roles = "Medewerker")]
        public IActionResult UpdatePakket(int id)
        {
            try
            {
                var pakket = _pakketRepo.GetPakketById(id);
                var medewerker = _medewerkerRepo.GetMedewerkerByEmail(User.Identity.Name);
                var kantine = medewerker.kantine;
                if (pakket == null)
                {
                    return NotFound();
                }

                var producten = _productRepo.GetProducten();
                var geselecteerdeProducten = _context.Pakketten
                    .Where(p => p.Id == id)
                    .SelectMany(p => p.producten.Select(p => p.Id))
                    .ToList();
                var productCheckModel = producten.Select(p => new ProductCheckModel
                {
                    product = p,
                    IsChecked = geselecteerdeProducten.Contains(p.Id)
                }).ToList();

                var pakketViewModel = new AddPakketViewModel
                {
                    Id = pakket.Id,
                    titel = pakket.titel,
                    kantine = pakket.kantine,
                    pickup = pakket.pickup,
                    pickUpMax = pakket.pickUpMax,
                    volwassen = pakket.volwassen,
                    prijs = pakket.prijs,
                    maaltijd = pakket.maaltijd,
                    producten = producten.ToList(),
                    selectedProducten = geselecteerdeProducten,
                    productchecks = productCheckModel
                };

                return View(pakketViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Er is iets fout gegaan bij het toevoegen van een nieuw pakket " + ex.Message);
                return View(new AddPakketViewModel());
            }
        }
        [HttpPost]
        [Authorize(Roles = "Medewerker")]
        public IActionResult UpdatePakket(AddPakketViewModel updatePakketViewModel)
        {
            try
            {
                var producten = _productRepo.GetProducten();
                var medewerker = _medewerkerRepo.GetMedewerkerByEmail(User.Identity.Name);
                updatePakketViewModel.producten = producten.ToList();
                if (ModelState.IsValid)
                {
                    var kantine = medewerker.kantine;
                    var bestaandPakket = _pakketRepo.GetPakketById(updatePakketViewModel.Id);

                    if (bestaandPakket == null)
                    {
                        return NotFound();
                    }

                    var productenInPakket = _pakketRepo.GetPakketProducten(updatePakketViewModel.Id);
                    var verwijderProducten = productenInPakket
                        .Where(product => !updatePakketViewModel
                        .selectedProducten
                        .Contains(product.Id))
                        .ToList();

                    foreach (var verwijderProduct in verwijderProducten)
                    {
                        _pakketRepo.DeleteProductenNieuwPakket(bestaandPakket.Id, verwijderProduct.Id);
                    }

                    var toevoegenProduct = updatePakketViewModel
                        .selectedProducten
                        .Where(productId => !productenInPakket
                        .Any(p => p.Id == productId))
                        .ToList();

                    foreach (var product in toevoegenProduct)
                    {
                        _pakketRepo.AddProducten(bestaandPakket.Id, product);
                    }

                    bool isVolwassen = producten.Any(p => updatePakketViewModel.selectedProducten.Contains(p.Id) && p.alcohol);

                    bestaandPakket.titel = updatePakketViewModel.titel;
                    bestaandPakket.kantine = kantine;
                    bestaandPakket.maaltijd = updatePakketViewModel.maaltijd;
                    bestaandPakket.pickup = updatePakketViewModel.pickup;
                    bestaandPakket.pickUpMax = updatePakketViewModel.pickUpMax;
                    bestaandPakket.prijs = updatePakketViewModel.prijs;
                    bestaandPakket.volwassen = isVolwassen;
                    bestaandPakket.stad = bestaandPakket.stad;

                    var currentTime = DateTime.Now;
                    var timeDifference = updatePakketViewModel.pickup - currentTime;

                    if (timeDifference.TotalHours < 48 && updatePakketViewModel.pickup >= currentTime)
                    {
                        if (updatePakketViewModel.pickup < updatePakketViewModel.pickUpMax)
                        {
                            _pakketRepo.UpdatePakket(bestaandPakket);
                            return RedirectToAction("Overzicht");
                        }
                        else
                        {
                            ModelState.AddModelError("pickUpMax", "PickUpMax mag niet kleiner zijn dan de pickup tijd");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("pickup", "Ophaaltijd mag maar 2 dagen vanaf vandaag zijn.");
                    }
                }

                var kantines = _kantineRepo.GetKantines();
                ViewBag.Kantines = kantines;
                return View(updatePakketViewModel);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("Error", "Er gaat iets mis bij het updaten van dit pakket");
                var kantines = _kantineRepo.GetKantines();
                ViewBag.Kantines = kantines;
                return View(updatePakketViewModel);
            }
        }

    }
}
