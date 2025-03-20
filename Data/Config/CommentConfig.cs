using BlogApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApi.Data.Config
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");  // More consistent naming
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(c => c.Content)
                                    .IsRequired()
                                    .HasMaxLength(1000);

            builder.Property(c => c.CreatedAt)
                                     .IsRequired()
                                     .ValueGeneratedOnAdd();

            // Foreign key for BlogPost (One BlogPost has many Comments)
            builder.HasOne(c => c.BlogPost)
                                  .WithMany(bp => bp.Comments)  // Ensure BlogPost model has List<Comment>
                                  .HasForeignKey(c => c.BlogPostId)
                                  .OnDelete(DeleteBehavior.Cascade);

            // Foreign key for User (A User can have many Comments)
            builder.HasOne(c => c.User)
                                 .WithMany()  // No need for navigation property in ApplicationUser
                                 .HasForeignKey(c => c.UserId)
                                 .OnDelete(DeleteBehavior.SetNull); // Allows anonymous comments
        }
    }
}
