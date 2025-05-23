﻿using System.Threading.Tasks;
using BestStoreMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BestStoreMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Register()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) {
                return View(registerDto);
            }
            //create new acc and authenticate user
            var user = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                CreatedAt= DateTime.Now,
            };

            var result = await userManager.CreateAsync(user,registerDto.Password);

            if (result.Succeeded) {
                //success registration
                await userManager.AddToRoleAsync(user, "client");
                //sign in new user
                await signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }
            
            //register failed -> show error
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerDto);
        }

        public async Task<IActionResult> Logout()
        {
            if (signInManager.IsSignedIn(User))
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
            var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password,
                loginDto.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login attempt.";
            }

            return View(loginDto);
        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
