﻿using humanResourceProject.Domain.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace humanResourceProject.Infrastructure.EntityTypeConfig
{
    public class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : class, IBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreateDate).IsRequired(true);
            builder.Property(x => x.DeleteDate).IsRequired(false);
            builder.Property(x => x.UpdateDate).IsRequired(false);
            builder.Property(x => x.Status).IsRequired(true);

        }
    }
}
