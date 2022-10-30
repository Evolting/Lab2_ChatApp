using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lab2_ChatApp.Entity
{
    public partial class ChatAppDBContext : DbContext
    {
        public ChatAppDBContext()
        {
        }

        public ChatAppDBContext(DbContextOptions<ChatAppDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=THEGHOST; database=ChatAppDB;user=sa; password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chat");

                entity.Property(e => e.ChatId).HasColumnName("chatId");

                entity.Property(e => e.ChatName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("chatName");

                entity.Property(e => e.ChatType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("chatType");

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Chats)
                    .UsingEntity<Dictionary<string, object>>(
                        "ChatUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ChatUser_User"),
                        r => r.HasOne<Chat>().WithMany().HasForeignKey("ChatId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ChatUser_Chat"),
                        j =>
                        {
                            j.HasKey("ChatId", "UserId");

                            j.ToTable("ChatUser");

                            j.IndexerProperty<int>("ChatId").HasColumnName("chatId");

                            j.IndexerProperty<int>("UserId").HasColumnName("userId");
                        });
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.MessageId).HasColumnName("messageId");

                entity.Property(e => e.ChatId).HasColumnName("chatId");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_Chat");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Message_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Username, "Unique_username")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
