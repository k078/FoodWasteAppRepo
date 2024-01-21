using Core.DomainServices.Interfaces;
using FoodWasteApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodWasteApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private IStudentRepo _studentRepo;
        private IMedewerkerRepo _medewerkerRepo;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IStudentRepo studentRepo, IMedewerkerRepo medewerkerRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _studentRepo = studentRepo;
            _medewerkerRepo = medewerkerRepo;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Overzicht", "Pakket");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                false,
                false);
                if (res.Succeeded && model.Email != null)
                {
                    return RedirectToAction("Overzicht", "Pakket");
                }
                ModelState.AddModelError("Login", "Er gaat iets mis bij de inlogpoging");
            }
            return View(model);


        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isStudentEmail = _studentRepo.GetStudentByEmail(model.Email) != null;
                var isEmployeeEmail = _medewerkerRepo.GetMedewerkerByEmail(model.Email) != null;

                if (isStudentEmail || isEmployeeEmail)
                {
                    var user = new IdentityUser
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        if (isStudentEmail)
                        {
                            var student = _studentRepo.GetStudentByEmail(model.Email);
                            var birthDate = student.geboortedatum.Date;
                            var today = DateTime.Today;
                            var age = today.Year - birthDate.Year;

                            if (birthDate > today.AddYears(-age))
                            {
                                age--;
                            }

                            if (age >= 16)
                            {
                                await _userManager.AddToRoleAsync(user, "Student");
                            }
                            else
                            {
                                await _userManager.DeleteAsync(user);
                                ViewBag.CustomError = "Je moet minimaal 16 jaar zijn om je aan te kunnen melden";
                                return View(model);
                            }
                        }
                        else if (isEmployeeEmail)
                        {
                            await _userManager.AddToRoleAsync(user, "Medewerker");
                        }

                        return RedirectToAction("Login", "Account");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description.ToString());
                    }
                }
                else
                {
                    ViewBag.CustomError = "Je moet een medewerker of student van Avans Hogeschool zijn om je aan te kunnen melden!";
                    return View(model);
                }
            }
            ViewBag.CustomError = "Email/password incorrect!";
            return View(model);
    }



        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
