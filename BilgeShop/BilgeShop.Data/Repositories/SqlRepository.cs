using BilgeShop.Data.Context;
using BilgeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Repositories
{
    // *** Generic Repository Pattern ***
    public class SqlRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly BilgeShopContext _db;
        private readonly DbSet<TEntity> _dbSet;
        public SqlRepository(BilgeShopContext db)
        {
            _db = db;
            _dbSet = db.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            Delete(entity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
           
            return _dbSet.FirstOrDefault(predicate);
        }
        

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            // _dbSet.Getall(); -> Koşulsuz , hepsi gelsin.
            // _dbSet.GetAll(x => x.Name == "Ali") -> Adı Ali olanları getir.

            return predicate is not null ? _dbSet.Where(predicate) : _dbSet;
        }

        public TEntity GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.Now;
            _dbSet.Update(entity);
            _db.SaveChanges();
        }
    }
}


// First - Single - FirstOrDefault - SingleOrDefault

// First -> tarama yapar , ilk veriyi bulur geriye döner. Veri yoksa hata verir.

// Single -> tarama yapar, ilk veriyi bulur geriye döner. Veri yoksa veya 1'den fazla aynı veriden varsa hata verir.

// FirstOrDefault -> tarama yapar, ilk veriyi bulur geriye döner. Veri yoksa null döner.

// SingleOrDefault -> tarama yapar, ilk veriyi bulur geriye döner. Veri yoksa null döner, birden fazla veri varsa hata verir.

// ---> BIZ EN ÇOK FIRSTORDEFAULT KULLANACAĞIZ!!