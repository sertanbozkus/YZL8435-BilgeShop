using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Repositories
{
    // Generic bir Interface
    // where TEntity : class -> TEntity yerine class yazılabilir
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(int id);
        TEntity GetById(int id);
    
        // Sorgu göndermek -> Expression 
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);
        //Geriye sorgu dönsün, ben üzerine ilave yapabileyim.

    }
}


// Repository Kavramı

// Repository -> Depo

// Zaten DbContext üzerinden gelen C R U D işlemleri yapmamıza yarayan metotlar mevcut. Bu metotları Repository içerisinde kendi tanımladığımız metotlarda kullanıp bir nevi özelleştiriyoruz. Örneğin DbContext üzerindeki silme metodu hard delete yaparak bir veriyi tamamen yok ederken , bizim silme metodumuz güncelleme yapıyor.

// Generic Repository -> Her bir veri tablosu için ayrı birer (interface - class ) repository yapısı oluşturmak yerine, bize sunulan generic yapı imkanlarını kullanarak tek ve dinamik bir repository oluşturuyoruz. <T>

// Generic Repository Pattern sayesinde C R U D işlemlerini tek bir noktadan yönetiyoruz.

// O ZAMAN GENERIC REPOSITORY NEDEN ÖNEMLİ -> C R U D işlemi metotlarını özelleştirebiliyoruz ve tek bir noktadan yönetiyoruz.


