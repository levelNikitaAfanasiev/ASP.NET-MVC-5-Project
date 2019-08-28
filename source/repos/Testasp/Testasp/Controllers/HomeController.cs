using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Testasp.Models;



namespace Testasp.Controllers
{
    public class HomeController : Controller
    {

        SoccerContext db = new SoccerContext();

        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Team);
            return View(players.ToList());
        }
        [HttpPost]
        public ActionResult Index(string Position, string player)
        {
            var players = db.Players.Include(p => p.Team);
            if (Position != null)
            {
                if (Position != "все")
                {
                    players = players.Where(p => p.Position == Position);
                }
            }
            if(player != null && player != "")
            {
                players = players.Where(p => p.Name == player);
                if (players == null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(players.ToList());
        }
     
        public ActionResult Delete(int id)
        {
            Player player = db.Players.Find(id);
            db.Entry(player).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Create()
        {
            // Формируем список команд для передачи в представление
            SelectList teams = new SelectList(db.Teams, "Id", "Name");
            ViewBag.Teams = teams;
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(Player player)
        {
            db.Players.Add(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            Player player = new Player();
            player = db.Players.Find(id);
            SelectList teams = new SelectList(db.Teams,"Id","Name");
            ViewBag.Teams = teams;
            return PartialView(player);
        }
        [HttpPost]
        public ActionResult Edit(Player player)
        {
            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ListTeams()
        {
            var teams = db.Teams.ToList();
            return PartialView(teams);
        }

    }
}