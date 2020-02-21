
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class CategoryController : Controller
    {
        private ShopContext db = new ShopContext();


        //TODO: Each line should be a separate method in this class
        // List
        public ActionResult List(string speciessearchkey)
        {            

            string query = "Select * from categories";
            if (speciessearchkey != "")
            {
                query = query + " where Name like '%" + speciessearchkey + "%'";
            }

            //what data do we need?
            List<Category> myspecies = db.Categories.SqlQuery(query).ToList();

            return View(myspecies);
        }

        public ActionResult Add()
        {
            //I don't need any information to do add of species.
            return View();
        }

        [HttpPost]
        public ActionResult Add(string categoryName)
        {
            string query = "insert into categories (Name) values (@CategoryName)";
            var parameter = new SqlParameter("@CategoryName", categoryName);
            db.Database.ExecuteSqlCommand(query, parameter);

            return RedirectToAction("List");
        }


        public ActionResult Show(int id)
        {
            string query = "select * from categories where Id = @id";
            var parameter = new SqlParameter("@id", id);
            Category selectedCategory = db.Categories.SqlQuery(query, parameter).FirstOrDefault();

            return View(selectedCategory);
        }

        public ActionResult Update(int id)
        {
            string query = "select * from categories where Id = @id";
            var parameter = new SqlParameter("@Id", id);
            Category selectedCategory = db.Categories.SqlQuery(query, parameter).FirstOrDefault();

            //Category selectedCategory = db.Categories.FirstOrDefault(x => x.Id == id);

            return View(selectedCategory);
        }
        [HttpPost]
        public ActionResult Update(int id, string categoryName)
        {
            string query = "update categories set name = @categoryName where Id = @id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@categoryName", categoryName);
            sqlparams[1] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from categories where Id=@id";
            SqlParameter param = new SqlParameter("@id", id);
            Category selectedcategory = db.Categories.SqlQuery(query, param).FirstOrDefault();
            return View(selectedcategory);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //for the sake of referential integrity, delete all items of category that has been deleted
            string refquery = "delete from items where CategoryId=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(refquery, param); 

            string query = "delete from categories where Id=@id";            
            db.Database.ExecuteSqlCommand(query, param); //same param as before

            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}