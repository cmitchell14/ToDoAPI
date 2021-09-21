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
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class ToDoController : ApiController
    {
        ToDoEntities db = new ToDoEntities();

        //api/ToDo/
        public IHttpActionResult GetToDo()
        {
            List<ToDoViewModel> ToDo = db.TodoItems.Include("Category").Select(t => new ToDoViewModel()
            {
                //Assing parameter for ToDo Items
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).ToList<ToDoViewModel>();

            if (ToDo.Count == 0)
            {
                return NotFound(); //404 error
            }

            return Ok(ToDo);
        }//End GetToDo()

        //api/ToDo/id
        public IHttpActionResult GetToDo(int id)
        {
            ToDoViewModel toDo = db.TodoItems.Include("Category").Where(t => t.TodoId == id).Select(t => new ToDoViewModel()
            {
                TodoId = t.TodoId,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }

            }).FirstOrDefault();

            if (toDo == null)
            {
                return NotFound();
            }

            return Ok(toDo);
        }//End GetToDo()

        //api/ToDo/ (HttpPost)
        public IHttpActionResult PostToDo(ToDoViewModel toDo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            TodoItem newToDo = new TodoItem()
            {
                Action = toDo.Action,
                Done = toDo.Done,
                CategoryId = toDo.CategoryId
            };

            db.TodoItems.Add(newToDo);
            db.SaveChanges();
            return Ok(newToDo);
        }//End PostToDo

        //api/Todo (HttpPut)
        public IHttpActionResult PutToDo(ToDoViewModel toDo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            TodoItem existToDo = db.TodoItems.Where(t => t.TodoId == toDo.TodoId).FirstOrDefault();
            if (existToDo != null)
            {
                existToDo.TodoId = toDo.TodoId;
                existToDo.Action = toDo.Action;
                existToDo.Done = toDo.Done;
                existToDo.CategoryId = toDo.CategoryId;
                db.SaveChanges();
                return Ok();

            }
            else
            {
                return NotFound();
            }
        }//End PutToDo

        //api/ToDo/ (HttpDelete)
        public IHttpActionResult DeleteResource(int id)
        {
            TodoItem toDo = db.TodoItems.Where(t => t.TodoId == id).FirstOrDefault();

            if (toDo != null)
            {
                db.TodoItems.Remove(toDo);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }//End DeleteToDo

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);    
        }


    }//End Class
}//End Namespace
