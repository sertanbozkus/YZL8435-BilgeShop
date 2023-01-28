using BilgeShop.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }

        //[Required]
        //[MaxLength(30)] bu kısıtlamaları , fluent api ile yapacağım.
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserTypeEnum UserType { get; set; }

    }


    // Configurations diye bir klasör açıp , bu classları oranın içerisinde de tanımlayabilirsiniz.
    public class UserEntityConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Password)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(80);
            



            base.Configure(builder);


        }
    }
}
