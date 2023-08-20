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

                builder
                    .HasOne(e => e.Sender)
                    .WithMany(e => e.Messages)
                    .OnDelete(DeleteBehavior.NoAction);

                builder
                    .HasOne(e => e.Room)
                    .WithMany(e => e.Messages)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Room>(builder =>
            {
                builder
                    .HasMany(e => e.Participants)
                    .WithOne(e => e.Room)
                    .OnDelete(DeleteBehavior.NoAction);

                builder
                    .HasIndex(e => e.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Participant>(builder =>
            {
                builder
                    .HasIndex(e => new { e.UserId, e.RoomId })
                    .IsUnique();
            });
        }
    }
}

