using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

    }


    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasQueryFilter(x => x.IsDeleted == false); // Tüm sorguları silinmemis veriler üzerinden calıstırır.

            builder.Property(x => x.CreatedDate).IsRequired(); // null olamaz
            builder.Property(x => x.ModifiedDate).IsRequired(false); // null olabilir.

            builder.HasKey(x => x.Id); // Id'yi primary key yap. ( bunu zaten EFCore otomatik yapıyor.)
        }
    }
}
