using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class FilmplattformContext : DbContext
    {
        public FilmplattformContext()
        {
        }

        public FilmplattformContext(DbContextOptions<FilmplattformContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Filmgenre> Filmgenres { get; set; }
        public virtual DbSet<Filmmember> Filmmembers { get; set; }
        public virtual DbSet<Filmperson> Filmpeople { get; set; }
        public virtual DbSet<Following> Followings { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<List> Lists { get; set; }
        public virtual DbSet<Listfilm> Listfilms { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Memberlikelist> Memberlikelists { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Persontype> Persontypes { get; set; }
        public virtual DbSet<Watchevent> Watchevents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Server=127.0.0.1; Port=3306; Database=Filmplattform; Uid=root; Pwd=secret;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("film");

                entity.HasIndex(e => e.Id, "Film_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.BackdropUrl)
                    .HasMaxLength(40)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.LongDescription)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PosterUrl)
                    .HasMaxLength(40)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Filmgenre>(entity =>
            {
                entity.HasKey(e => new { e.GenreId, e.FilmId })
                    .HasName("PRIMARY");

                entity.ToTable("filmgenre");

                entity.HasIndex(e => e.FilmId, "fk_Genre_has_Film_Film1_idx");

                entity.HasIndex(e => e.GenreId, "fk_Genre_has_Film_Genre1_idx");

                entity.HasIndex(e => new { e.GenreId, e.FilmId }, "index4")
                    .IsUnique();

                entity.Property(e => e.GenreId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Genre_Id");

                entity.Property(e => e.FilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Film_Id");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Filmgenres)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Genre_has_Film_Film1");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Filmgenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Genre_has_Film_Genre1");
            });

            modelBuilder.Entity<Filmmember>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.FilmId })
                    .HasName("PRIMARY");

                entity.ToTable("filmmember");

                entity.HasIndex(e => new { e.MemberId, e.FilmId }, "filmMemberUnique")
                    .IsUnique();

                entity.HasIndex(e => e.FilmId, "fk_Member_has_Film_Film1_idx");

                entity.HasIndex(e => e.MemberId, "fk_Member_has_Film_Member1_idx");

                entity.Property(e => e.MemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Member_Id");

                entity.Property(e => e.FilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Film_Id");

                entity.Property(e => e.Like)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.LikeDate).HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Watchlist)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.WatchlistDate).HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Filmmembers)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Film_Film1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Filmmembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Film_Member1");
            });

            modelBuilder.Entity<Filmperson>(entity =>
            {
                entity.HasKey(e => new { e.PersonId, e.FilmId, e.PersonTypeId })
                    .HasName("PRIMARY");

                entity.ToTable("filmperson");

                entity.HasIndex(e => e.PersonTypeId, "fk_FilmPerson_PersonType1_idx");

                entity.HasIndex(e => e.FilmId, "fk_Person_has_Film_Film1_idx");

                entity.HasIndex(e => e.PersonId, "fk_Person_has_Film_Person1_idx");

                entity.HasIndex(e => new { e.PersonId, e.FilmId, e.PersonTypeId }, "index5")
                    .IsUnique();

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Person_Id");

                entity.Property(e => e.FilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Film_Id");

                entity.Property(e => e.PersonTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PersonType_Id");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Filmpeople)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_has_Film_Film1");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Filmpeople)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_has_Film_Person1");

                entity.HasOne(d => d.PersonType)
                    .WithMany(p => p.Filmpeople)
                    .HasForeignKey(d => d.PersonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_FilmPerson_PersonType1");
            });

            modelBuilder.Entity<Following>(entity =>
            {
                entity.HasKey(e => new { e.FollowerId, e.FollowingId })
                    .HasName("PRIMARY");

                entity.ToTable("following");

                entity.HasIndex(e => e.FollowerId, "fk_Following_Member1_idx");

                entity.HasIndex(e => e.FollowingId, "fk_Following_Member2_idx");

                entity.HasIndex(e => new { e.FollowerId, e.FollowingId }, "followerUnique")
                    .IsUnique();

                entity.Property(e => e.FollowerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Follower_Id");

                entity.Property(e => e.FollowingId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Following_Id");

                entity.HasOne(d => d.Follower)
                    .WithMany(p => p.FollowingFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Following_Member1");

                entity.HasOne(d => d.FollowingNavigation)
                    .WithMany(p => p.FollowingFollowingNavigations)
                    .HasForeignKey(d => d.FollowingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Following_Member2");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.HasIndex(e => e.Name, "Genre_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.ToTable("list");

                entity.HasIndex(e => e.Id, "List_Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.MemberId, "fk_List_Member1_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.MemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Member_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Lists)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_List_Member1");
            });

            modelBuilder.Entity<Listfilm>(entity =>
            {
                entity.HasKey(e => new { e.FilmId, e.ListId })
                    .HasName("PRIMARY");

                entity.ToTable("listfilm");

                entity.HasIndex(e => e.FilmId, "fk_Film_has_List_Film1_idx");

                entity.HasIndex(e => e.ListId, "fk_Film_has_List_List1_idx");

                entity.HasIndex(e => new { e.FilmId, e.ListId }, "unique")
                    .IsUnique();

                entity.Property(e => e.FilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Film_Id");

                entity.Property(e => e.ListId)
                    .HasColumnType("int(11)")
                    .HasColumnName("List_Id");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Listfilms)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Film_has_List_Film1");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.Listfilms)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Film_has_List_List1");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.HasIndex(e => e.Id, "Member_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PasswordHash)
                    .HasColumnType("blob")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PasswordSalt)
                    .HasColumnType("blob")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Vorname)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Memberlikelist>(entity =>
            {
                entity.HasKey(e => new { e.MemberId, e.ListId })
                    .HasName("PRIMARY");

                entity.ToTable("memberlikelist");

                entity.HasIndex(e => e.ListId, "fk_Member_has_List_List1_idx");

                entity.HasIndex(e => e.MemberId, "fk_Member_has_List_Member1_idx");

                entity.Property(e => e.MemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Member_Id");

                entity.Property(e => e.ListId)
                    .HasColumnType("int(11)")
                    .HasColumnName("List_Id");

                entity.HasOne(d => d.List)
                    .WithMany(p => p.Memberlikelists)
                    .HasForeignKey(d => d.ListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_List_List1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Memberlikelists)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_List_Member1");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.HasIndex(e => e.Id, "Person_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Bio)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Persontype>(entity =>
            {
                entity.ToTable("persontype");

                entity.HasIndex(e => e.Name, "Name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "idPersonType_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Watchevent>(entity =>
            {
                entity.ToTable("watchevent");

                entity.HasIndex(e => e.Id, "WatchEvent_Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FilmId, "fk_WatchEvent_Film1_idx");

                entity.HasIndex(e => e.MemberId, "fk_WatchEvent_Member1_idx");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.FilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Film_Id");

                entity.Property(e => e.MemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Member_Id");

                entity.Property(e => e.Rating)
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.Text)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.Film)
                    .WithMany(p => p.Watchevents)
                    .HasForeignKey(d => d.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_WatchEvent_Film1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Watchevents)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_WatchEvent_Member1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
