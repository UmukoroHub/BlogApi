using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BlogApi.Models;

namespace BlogApi.Data.Config
{
    public class BlogPostConfig : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> builder)
        {
            builder.ToTable("BlogPosts");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(bp => bp.Title)
                                     .IsRequired()
                                     .HasMaxLength(100);

            builder.Property(bp => bp.Content)
                                     .IsRequired()
                                     .HasMaxLength(100000);

            builder.Property(bp => bp.CreatedAt)
                                     .IsRequired()
                                     .ValueGeneratedOnAdd();

            // Foreign Key - A User (Author) can have multiple BlogPosts
            builder.HasOne(bp => bp.Author)
                                   .WithMany()  // No need for navigation in ApplicationUser
                                   .HasForeignKey(bp => bp.UserId)
                                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship with Comments
            builder.HasMany(bp => bp.Comments)
                                    .WithOne(c => c.BlogPost)
                                    .HasForeignKey(c => c.BlogPostId)
                                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
