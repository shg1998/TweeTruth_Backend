﻿using System;
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Post
{
    public class Post:BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }

        public Category Category { get; set; }
        public User.User Author{ get; set; }

    }

    public class PostConfiguration:IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Description).IsRequired();
            builder.HasOne(p => p.Category).WithMany(p=>p.Posts).HasForeignKey(p=>p.CategoryId);
            builder.HasOne(p => p.Author).WithMany(p=>p.Posts).HasForeignKey(p=>p.AuthorId);
        }
    }
}
