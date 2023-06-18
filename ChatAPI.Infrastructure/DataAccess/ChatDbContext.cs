using ChatAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Infrastructure.DataAccess
{
    public class ChatDbContext : DbContext
	{
		public ChatDbContext(DbContextOptions<ChatDbContext> options)
			: base(options)
		{

		}

        public DbSet<User> User { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Room> Room { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Message>(builder =>
			{
                builder
                    .HasOne(e => e.User)
                    .WithMany(e => e.UserMessages)
                    .OnDelete(DeleteBehavior.NoAction);

                builder
                    .HasOne(e => e.User)
                    .WithMany(e => e.UserMessages)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.HasIndex(i => i.Name).IsUnique();
            });

            modelBuilder.Entity<Room>(builder =>
            {
                builder
                    .HasMany(e => e.Participants)
                    .WithMany(e => e.UserRooms);
            });
        }
    }
}

