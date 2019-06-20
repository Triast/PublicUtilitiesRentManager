using AspNetCore.Identity.Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PublicUtilitiesRentManager.Domain.Entities;
using PublicUtilitiesRentManager.Persistance.Interfaces;
using PublicUtilitiesRentManager.WebUI.Models.AccountViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ITenantRepository _tenantRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UsersController(
            ITenantRepository tenantRepository,
            UserManager<ApplicationUser> manager,
            ILogger<UsersController> logger)
        {
            _tenantRepository = tenantRepository;
            _userManager = manager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var tenants = (await _tenantRepository.GetAllAsync()).ToList();

            return View(_userManager.Users.Select(user => new IndexViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                Tenant = (tenants.FirstOrDefault(t => t.UserId == user.Id) ?? new Tenant { Name = "Отсутствует" }).Name,
                StatusMessage = ""
            }));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    return RedirectToAction("Index");
                }
                AddErrors(result);
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            var tenant = (await _tenantRepository.GetAllAsync()).FirstOrDefault(t => t.UserId == user.Id);
            IndexViewModel model = new IndexViewModel()
            {
                IsEmailConfirmed = user.EmailConfirmed,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Tenant = tenant?.Name ?? "Отсутствует",
                StatusMessage = ""
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            var tenants = await _tenantRepository.GetAllAsync();
            var freeTenants = tenants.Where(t => t.UserId == null);
            var tenantsSelectList = new SelectList(freeTenants, "Id", "Name", freeTenants.First());

            var model = new EditUserViewModel
            {
                Email = user.Email,
                UserName = user.UserName,
                Tenants = tenantsSelectList
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var tenant = await _tenantRepository.GetByIdAsync(model.TenantId);
                        tenant.UserId = user.Id;
                        await _tenantRepository.UpdateAsync(tenant);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> Delete(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            IndexViewModel model = new IndexViewModel()
            {
                IsEmailConfirmed = user.EmailConfirmed,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                StatusMessage = ""
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteConfirm(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}