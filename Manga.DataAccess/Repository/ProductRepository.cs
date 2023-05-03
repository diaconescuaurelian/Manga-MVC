using Manga.DataAccess.Data;
using Manga.DataAccess.Repository.IRepository;
using Manga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manga.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Price = obj.Price;
                objFromDb.Author = obj.Author;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.VolumeNumber = obj.VolumeNumber;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                if(obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
