using Shop.Data;
using Shop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class ItemController : Controller
    {
        private ShopContext db = new ShopContext();

        // GET: Item/List
        public ActionResult List(string itemsearchkey)
        {
            string query = "Select * from Items";

            //can we access the search key?            
            if (itemsearchkey != "")
            {
                //modify the query to include the search key
                query = query + " where Name like '%" + itemsearchkey + "%'";                
            }
           //List - array, item - type of data, 
            List<Item> items = db.Items.SqlQuery(query).ToList();
            return View(items);
        }

        // GET: Item/Show/5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Item item = db.Items.SqlQuery("select * from items where Id=@itemId", new SqlParameter("@itemId", id)).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            
            string query = "select * from categories inner join items on items.CategoryId = Categories.Id where items.Id = @id";
            SqlParameter param = new SqlParameter("@id", id);
            Category category = db.Categories.SqlQuery(query, param).FirstOrDefault();

            ShowItem viewmodel = new ShowItem();
            viewmodel.Item = item;
            viewmodel.Category = category;

            return View(viewmodel);
        }


        protected void ProcessFile(HttpPostedFileBase file, int id, ref bool hasPic, ref string picExtension)
        {
            hasPic = false;
            picExtension = "";
            
            if (file == null)
            {
                return;
            }

            //checking to see if the file size is greater than 0 (bytes)
            if (file.ContentLength == 0)
            {
                return;

            }
                                    
            var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
            var extension = Path.GetExtension(file.FileName).Substring(1);

            if (!valtypes.Contains(extension))
            {
                return;
            }
                    
            try
            {
                //file name is the id of the image
                string fn = id + "." + extension;
                
                string path = Path.Combine(Server.MapPath("~/Content/Items/"), fn);

                //save the file
                file.SaveAs(path);
                //if these are all successful then we can set these fields
                hasPic = true;
                picExtension = extension;
            }
            catch (Exception ex)
            {
                // :TODO handle exception
            }
        }

        //THE [HttpPost] Means that this method will only be activated on a POST form submit to the following URL
        //URL: /Items/Add
        [HttpPost]
        public ActionResult Add(string itemName, int categoryId, int itemPrice)
        {
            string query = "insert into Items (Name, CategoryId, HasPic, PicExtension, Price) values (@itemName, @categoryId, 0, '',@itemPrice)";

            SqlParameter[] sqlparams = new SqlParameter[] {
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@categoryId", categoryId),
                new SqlParameter("@itemPrice", itemPrice)
            };
            
            db.Database.ExecuteSqlCommand(query, sqlparams);
            
            return RedirectToAction("List");
        }


        public ActionResult Add()
        {            
            List<Category> categories = db.Categories.SqlQuery("select * from Categories").ToList();
            return View(categories);
        }

        public ActionResult Update(int id)
        {            
            Item item = db.Items.SqlQuery("select * from items where Id=@itemId", new SqlParameter("@itemId", id)).FirstOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
                        
            List<Category> categories = db.Categories.ToList();

            UpdateItem viewmodel = new UpdateItem();
            viewmodel.Item = item;
            viewmodel.Categories = categories;

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Update(int id, string name, int categoryId, int price, HttpPostedFileBase pic)
        {
            //start off with assuming there is no picture

            bool hasPic = false;
            string picExtension = "";

            ProcessFile(pic, id, ref hasPic, ref picExtension);


            string query = "update items set name=@name, categoryId=@categoryId, hasPic=@hasPic, picExtension=@picExtension, Price=@itemPrice where Id=@id";
            SqlParameter[] sqlparams = new SqlParameter[] {
                new SqlParameter("@name", name),
                new SqlParameter("@categoryId", categoryId),
                new SqlParameter("@id", id),
                new SqlParameter("@hasPic", hasPic),
                new SqlParameter("@picExtension", picExtension),
                new SqlParameter("@itemPrice", price)
            };

            db.Database.ExecuteSqlCommand(query, sqlparams);
            
            return RedirectToAction("List");
        }

        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from items where Id = @id";
            SqlParameter param = new SqlParameter("@id", id);
            Item item = db.Items.SqlQuery(query, param).FirstOrDefault();

            return View(item);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string query = "delete from items where Id = @id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

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