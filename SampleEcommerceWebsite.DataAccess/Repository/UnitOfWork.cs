﻿using SampleEcommerceWebsite.DataAccess.Data;
using SampleEcommerceWebsite.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleEcommerceWebsite.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }
        //Adding New Model step 5 - updating IUnitOfWork
        public ICompanyRepository Company { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            //Adding New Model step 5 - initialize Company
            Company = new CompanyRepository(_db);
        }
        



        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
