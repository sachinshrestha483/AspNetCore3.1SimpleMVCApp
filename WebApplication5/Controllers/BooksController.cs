using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BooksController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _db.Books.ToListAsync();
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Book book)
        {
           
           if(ModelState.IsValid)
            {
                if(book.Id==0)
                {
                    _db.Books.Add(book);
                }

                else
                {
                    _db.Books.Update(book);
                }

                _db.SaveChanges();

                return RedirectToAction("index");
            }

            return View(book);
        }

        

        public IActionResult Upsert(int?id)
        {
            var book = new Book();
            if(id==null)
            {
                return View(book);
            }
            book = _db.Books.FirstOrDefault(u => u.Id == id);

            if(book==null)
            {
                return NotFound();
            }

            return View(book);

        }

        public IActionResult Delete(int ?id)
        {
            if(id==null)
            {
                return null;
            }

            else
            {
                var book = _db.Books.FirstOrDefault(u => u.Id == id);
                if(book ==null)
                {
                    return null;
                }
                _db.Books.Remove(book);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));

            }
        }


        

    }
}