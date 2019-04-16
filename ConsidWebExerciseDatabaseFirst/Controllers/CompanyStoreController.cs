using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ConsidWebExerciseDatabaseFirst.Controllers
{
    public class CompanyStoreController : Controller
    {
        CompanyStoreContext db = new CompanyStoreContext();

        public static Guid ID;

        //---------- Homepage ------------
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        //---------- GET: Company List ------------
        [HttpGet]
        public ActionResult List()
        {
            return View(db.Companies.ToList());
        }

        //---------- GET: Company Create ----------
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // ---------- POST: Company Create ----------
        [HttpPost]
        public ActionResult Create(Companies company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    company.Id = Guid.NewGuid();
                    db.Companies.Add(company);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                return View(company);
            }

            catch
            {
                return View();
            }
        }

        // ----------GET: Company Edit----------
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            // Check if chosen id and company is correct 
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Companies company = db.Companies.Find(id);
            if (company == null)   
                return HttpNotFound();
            
            return View(company);
        }

        // ----------POST: Company Edit----------
        [HttpPost]
        public ActionResult Edit(Companies company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Modify the database context and save it 
                    db.Entry(company).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                return View(company);
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
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            Companies company = db.Companies.Find(id);
            if(company == null)
                return HttpNotFound();
            
            return View(company);
        }

        // ----------POST: Company Delete----------
        [HttpPost]
        public ActionResult Delete(Guid id, Companies comp)
        {
            try
            {
                Companies company = new Companies();

                if (ModelState.IsValid)
                {
                    if (id == null)
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    
                    company = db.Companies.Find(id);
                    if (company == null)
                        return HttpNotFound();

                    // Find all stores related to the company with CompanyId and store them to a list
                    IEnumerable<Stores> stores = db.Stores.Where(s => s.CompanyId == id).ToList();
                    if (stores != null)
                    {
                        // RemoveRange - delete all records at once within the range
                        db.Stores.RemoveRange(stores);
                    }

                    db.Companies.Remove(company);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
                return View(company);
            }

            catch
            {
                return View();
            }
     
         }

        // ----------Get: CompanyStores-List ----------
        [HttpGet]
        public ActionResult StoreList(Guid id)
        {
            ID = id;
            var store = db.Stores.Where(s => s.CompanyId == id);
            if(store == null)
                return HttpNotFound();
            
            Companies company = new Companies();
            company = db.Companies.Find(ID);
            // Pass the name of Company to the View with viewbag
            ViewBag.CompanyName = company.Name;
            return View(store.ToList());
        }

        //---------- GET: CompanyStore Create ----------
        [HttpGet]
        public ActionResult StoreCreate()
        {
            Companies company = new Companies();
            company = db.Companies.Find(ID);
            ViewBag.CompanyName = company.Name;
            ViewBag.CompanyId = company.Id; 
            return View();
        }

        // ---------- POST: CompanyStore Create ----------
        [HttpPost]
        public ActionResult StoreCreate(Stores store, Companies comp )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
              
                store.CompanyId = ID;
                store.Id = Guid.NewGuid();
                db.Stores.Add(store);
                db.SaveChanges();
                return RedirectToAction("StoreList", new { id = store.CompanyId });
                
            }

            catch
            {
                return View();
            }

        }

        // ----------GET: Edit Store ----------
        [HttpGet]
        public ActionResult StoreEdit(Guid id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Stores store = db.Stores.Find(id);
            if (store == null)
                return HttpNotFound();

            return View(store);
        }

        // ----------POST: Edit Store ----------
        [HttpPost]
        public ActionResult StoreEdit(Stores store)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Modify the database context and save it 
                    db.Entry(store).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("StoreList", new {id = store.CompanyId});
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
        public ActionResult StoreDelete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stores store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }

            return View(store);
        }

        // ----------POST: Store Delete----------
        [HttpPost]
        public ActionResult StoreDelete(Guid id, Stores s)
        {
            try
            {
                Stores store = new Stores();
                if (ModelState.IsValid)
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
 
                    store = db.Stores.Find(id);
                    if (store == null)
                    {
                        return HttpNotFound();
                    }

                    db.Stores.Remove(store);
                    db.SaveChanges();
                    return RedirectToAction("StoreList", new { id = store.CompanyId });
                }
                return View(store);
            }

            catch
            {
                return View();
            }

        }

        // GET: -------- Details Store -----------
        public ActionResult StoreDetails(Guid id)
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
