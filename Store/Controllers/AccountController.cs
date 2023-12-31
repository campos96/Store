﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Store.Models;
using System.Security.Claims;
using Store.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Store.Controllers
{
    public class AccountController : Controller
    {
        private readonly StoreContext _context;

        public AccountController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userIdentity = User.Identity as ClaimsIdentity;

            var email = userIdentity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .FirstOrDefault();

            if (email == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var account = _context.Accounts.Where(u => u.Email == email.Value).FirstOrDefault();

            if (account == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var user = _context.User.Where(u => u.AccountId == account.Id).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var userAccount = new UserAccountViewModel { Account = account, User = user };
            return View(userAccount);
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var account = _context.Accounts
                .Where(u => u.Email == login.Email && u.Password == login.Password)
                .FirstOrDefault();

            if (account == null)
            {
                ModelState.AddModelError(nameof(LoginViewModel.Password), "Invalid username or password.");
                return View();
            }

            var user = _context.User.Where(u => u.AccountId == account.Id).FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = login.RememberMe,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel signup)
        {
            if (!ModelState.IsValid)
            {
                return View(signup);
            }

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Username = signup.Username,
                Email = signup.Email,
                Password = signup.Password,
            };

            _context.Add(account);
            await _context.SaveChangesAsync();

            var user = new User
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                Name = signup.Name,
                LastName = signup.LastName,
            };

            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var userIdentity = User.Identity as ClaimsIdentity;

            var email = userIdentity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .FirstOrDefault();

            if (email == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var account = _context.Accounts.Where(u => u.Email == email.Value).FirstOrDefault();
            if (account == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            var user = _context.User.Where(u => u.AccountId == account.Id).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            var userIdentity = User.Identity as ClaimsIdentity;

            var email = userIdentity.Claims
                .Where(c => c.Type == ClaimTypes.Email)
                .FirstOrDefault();

            if (email == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));


        }
        private bool UserExists(Guid id)
        {
            return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
