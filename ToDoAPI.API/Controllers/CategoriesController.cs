using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.DATA.EF;
using ToDoAPI.API.Models;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //api/Categories/
        public IHttpActionResult GetCategories()
        {
            List<CategoryViewModel> cat = db.Categories.Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).ToList<CategoryViewModel>();

            if (cat.Count == 0)
            {
                return NotFound();
            }

            return Ok(cat);
        }//End GetCategories()

        //api/Categories/id 
        public IHttpActionResult GetCategory(int id)
        {
            CategoryViewModel cat = db.Categories.Where(c => c.CategoryId == id).Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();
            }

            return Ok(cat);
        }//End GetCategory

        //api/Categories (HttpPost) Create
        public IHttpActionResult PostCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            Category newCategory = new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            };

            db.Categories.Add(newCategory);
            db.SaveChanges();
            return Ok(newCategory);
        }

        //api/Categories/id (HttpPut)
        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Category existingCat = db.Categories.Where(c => c.CategoryId == cat.CategoryId).FirstOrDefault();
            if (existingCat != null)
            {
                existingCat.CategoryId = cat.CategoryId;
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;
                db.SaveChanges();
                return Ok();

            }
            else
            {
                return NotFound();
            }
        }//End PutCategory()

        //api/Categories/id (HttpDelete)
        public IHttpActionResult DeleteCategory(int id)
        {
            Category cat = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
            if (cat != null)
            {
                db.Categories.Remove(cat);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//End DeleteCategory

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); //close the db connection
            }
            base.Dispose(disposing);
        }//END Dispose()

    }//End Class
}//End Namespace
