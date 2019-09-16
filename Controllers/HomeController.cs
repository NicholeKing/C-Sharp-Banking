using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Banking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Banking.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
     
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("regCheck")]
        public IActionResult regCheck(Account newAcc)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Accounts.Any(a => a.Email == newAcc.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<Account> Hasher = new PasswordHasher<Account>();
                newAcc.Password = Hasher.HashPassword(newAcc, newAcc.Password);
                dbContext.Add(newAcc);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("AID", newAcc.AID);
                int? Sess = HttpContext.Session.GetInt32("AID");
                return Redirect($"/Main/{Sess}");
            }
            return View("Index");
        }

        [HttpGet("login")]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost("logCheck")]
        public IActionResult logCheck(Log AcctSub)
        {
            if(ModelState.IsValid)
            {
                var acctInDb = dbContext.Accounts.FirstOrDefault(a => a.Email == AcctSub.Email);

                if(acctInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Login");
                    return View("login");
                }
                var hasher = new PasswordHasher<Log>();
                var result = hasher.VerifyHashedPassword(AcctSub, acctInDb.Password, AcctSub.Password);
                if (result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Login");
                    return View("login");
                }
                HttpContext.Session.SetInt32("AID", acctInDb.AID);
                int? Sess = HttpContext.Session.GetInt32("AID");
                return Redirect($"/Main/{Sess}");
            }
            return View("login");
        }

        [HttpGet("Main/{AID}")]
        public IActionResult Main(int AID)
        {
            int? Sess = HttpContext.Session.GetInt32("AID");
            if(Sess == null){
                return Redirect("/");
            }
            if(AID != Sess){
                return Redirect("/logout");
            }
            var User = dbContext.Accounts.FirstOrDefault(a => a.AID == Sess);
            ViewBag.user = User;
            List<Transaction> transByUser = dbContext.Trans.Include(t => t.Trans).Where(n => n.AID == Sess).OrderByDescending(n => n.CreatedAt).ToList();
            ViewBag.trans = transByUser;
            return View();
        }

        [HttpPost("transact")]
        public IActionResult transact(Transaction newTran)
        {
            int? Sess = HttpContext.Session.GetInt32("AID");
            if(ModelState.IsValid)
            {
                var check = dbContext.Accounts.FirstOrDefault(a => a.AID == Sess);
                if(newTran.Amount < 0){
                    if(newTran.Amount + check.Balance < 0)
                    {
                        return Redirect($"Main/{Sess}");
                    } else {
                        check.Balance = newTran.Amount + check.Balance;
                        newTran.AID = check.AID;
                        dbContext.Add(newTran);
                        dbContext.SaveChanges();
                    }
                }
                else if (newTran.Amount == 0){
                    return Redirect($"Main/{Sess}");
                } else {
                    check.Balance = newTran.Amount + check.Balance;
                    newTran.AID = check.AID;
                    dbContext.Add(newTran);
                    dbContext.SaveChanges();
                }
                return Redirect($"Main/{Sess}");
            }
            //NEED TO FIX THIS
            return View("Main", newTran); //NEED TO FIX THIS
        }

        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
