using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ConsidWebExerciseDatabaseFirst.Controllers
{
    public class StoreController : Controller
    {
        CompanyStoreContext db = new CompanyStoreContext();

        //---------- GET: List Store ------------
        [HttpGet]
        public ActionResult List()
        {
            return View(db.Stores.ToList());
        }


        // ----------GET: Edit Store ----------
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Stores store = db.Stores.Find(id);
            if (store == null)
                return HttpNotFound();

            ViewBag.StoreName = store.Name;

            return View(store);
        }
        
        // ----------POST: Company Edit----------
        [HttpPost]
        public ActionResult Edit(Stores store)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Modify the database context and save it 
                    db.Entry(store).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                return View(store);
            }

            catch
            {
                return View();
            }
        }
        
        //---------- GET: Company Delete----------
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Stores store = db.Stores.Find(id);
            if (store == null)
                return HttpNotFound();

            return View(store);
        }

        // ----------POST: Store Delete----------
        [HttpPost]
        public ActionResult Delete(Guid id, Stores s)
        {
            try
            {
                Stores store = new Stores();
                if (ModelState.IsValid)
                {
                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    store = db.Stores.Find(id);
                    if (store == null)
                        return HttpNotFound();

                    db.Stores.Remove(store);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                return View(store);
            }

            catch
            {
                return View();
            }

        }
     

                
        // GET: -------- Details Store -----------
        public ActionResult Details(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Stores store = db.Stores.Find(id);
            if (store == null)
                return HttpNotFound();
            
            return View(store);
        }
                  
    }
}
