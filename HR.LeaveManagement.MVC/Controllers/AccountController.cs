using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isLoggedIn = await _authService.Authenticate(loginViewModel.Email!, loginViewModel.Password!);

                if (isLoggedIn)
                    return RedirectToAction("Index", "Home");
                else
                    ModelState.AddModelError("", "Invalid crediantials");

            }

            return View(loginViewModel);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                RegisterModel registerModel = _mapper.Map<RegisterViewModel, RegisterModel>(model);
                var isRegistered = await _authService.Register(registerModel);
                if (isRegistered)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", "Invalid Register Crediantials, please try again");
            }

            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction(nameof(Login));
        }
    }
}
