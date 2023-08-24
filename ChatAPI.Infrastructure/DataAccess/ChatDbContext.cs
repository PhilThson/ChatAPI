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

        public DbSet<Participant> Participant { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Room> Room { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Message>(builder =>
			{
                builder
                    .Property(e => e.SendTime)
                    .HasDefaultValueSql("getdate()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Room>(builder =>
            {
                builder
                    .HasMany(e => e.Participants)
                    .WithOne(e => e.Room)
                    .OnDelete(DeleteBehavior.Cascade);

                builder
                    .HasMany(e => e.Messages)
                    .WithOne(e => e.Room)
                    .OnDelete(DeleteBehavior.Cascade);

                builder
                    .HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Participant>(builder =>
            {
                builder
                    .HasIndex(e => new { e.UserId, e.RoomId })
                    .IsUnique();

                builder
                    .HasMany(p => p.Messages)
                    .WithOne(p => p.Sender)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}

