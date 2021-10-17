﻿using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository:Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext  db)
           :base(db)
        {
            _db = db;
        }
        
        public void Update(Category category)
        {
            var objFromDb = _db.Categories.FirstOrDefault(x=>x.Id == category.Id);
            if (objFromDb != null) {
                objFromDb.Name = category.Name;              
            }
        }

    }
}