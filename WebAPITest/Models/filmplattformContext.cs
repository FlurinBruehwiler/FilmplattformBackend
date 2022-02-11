using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebAPITest.Models
{
    public partial class filmplattformContext : DbContext
    {
        public filmplattformContext()
        {
        }

        public filmplattformContext(DbContextOptions<filmplattformContext> options)
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
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Memberlikelist> Memberlikelists { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Persontype> Persontypes { get; set; }
        public virtual DbSet<Watchevent> Watchevents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("Server=127.0.0.1; Port=3306; Database=filmplattform; Uid=root; Pwd=secret;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>(entity =>
            {
                entity.ToTable("film");

                entity.HasIndex(e => e.FilmId, "Film_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.FilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Film_Id");

                entity.Property(e => e.FilmLongDescription)
                    .HasMaxLength(500)
                    .HasColumnName("Film_LongDescription")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FilmReleaseDate).HasColumnName("Film_ReleaseDate");

                entity.Property(e => e.FilmShortDescription)
                    .HasMaxLength(100)
                    .HasColumnName("Film_ShortDescription")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FilmTitle)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("Film_Title");
            });

            modelBuilder.Entity<Filmgenre>(entity =>
            {
                entity.HasKey(e => new { e.FilmGenreGenreId, e.FilmGenreFilmId })
                    .HasName("PRIMARY");

                entity.ToTable("filmgenre");

                entity.HasIndex(e => e.FilmGenreFilmId, "fk_Genre_has_Film_Film1_idx");

                entity.HasIndex(e => e.FilmGenreGenreId, "fk_Genre_has_Film_Genre1_idx");

                entity.HasIndex(e => new { e.FilmGenreGenreId, e.FilmGenreFilmId }, "index4")
                    .IsUnique();

                entity.Property(e => e.FilmGenreGenreId)
                    .HasColumnType("int(11)")
                    .HasColumnName("FilmGenre_Genre_Id");

                entity.Property(e => e.FilmGenreFilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("FilmGenre_Film_Id");

                entity.HasOne(d => d.FilmGenreFilm)
                    .WithMany(p => p.Filmgenres)
                    .HasForeignKey(d => d.FilmGenreFilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Genre_has_Film_Film1");

                entity.HasOne(d => d.FilmGenreGenre)
                    .WithMany(p => p.Filmgenres)
                    .HasForeignKey(d => d.FilmGenreGenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Genre_has_Film_Genre1");
            });

            modelBuilder.Entity<Filmmember>(entity =>
            {
                entity.HasKey(e => new { e.FilmMemberMemberId, e.FilmMemberFilmId })
                    .HasName("PRIMARY");

                entity.ToTable("filmmember");

                entity.HasIndex(e => e.FilmMemberFilmId, "fk_Member_has_Film_Film1_idx");

                entity.HasIndex(e => e.FilmMemberMemberId, "fk_Member_has_Film_Member1_idx");

                entity.Property(e => e.FilmMemberMemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("FilmMember_Member_Id");

                entity.Property(e => e.FilmMemberFilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("FilmMember_Film_Id");

                entity.Property(e => e.FilmMemberLike)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("FilmMember_Like")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FilmMemberLikeDate)
                    .HasColumnName("FilmMember_Like_Date")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FilmMemberWatchlist)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("FilmMember_Watchlist")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.FilmMemberWatchlistDate)
                    .HasColumnName("FilmMember_Watchlist_Date")
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.FilmMemberFilm)
                    .WithMany(p => p.Filmmembers)
                    .HasForeignKey(d => d.FilmMemberFilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Film_Film1");

                entity.HasOne(d => d.FilmMemberMember)
                    .WithMany(p => p.Filmmembers)
                    .HasForeignKey(d => d.FilmMemberMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_Film_Member1");
            });

            modelBuilder.Entity<Filmperson>(entity =>
            {
                entity.HasKey(e => new { e.FilmPersonPersonId, e.FilmPersonFilmId, e.FilmPersonPersonTypeId })
                    .HasName("PRIMARY");

                entity.ToTable("filmperson");

                entity.HasIndex(e => e.FilmPersonPersonTypeId, "fk_FilmPerson_PersonType1_idx");

                entity.HasIndex(e => e.FilmPersonFilmId, "fk_Person_has_Film_Film1_idx");

                entity.HasIndex(e => e.FilmPersonPersonId, "fk_Person_has_Film_Person1_idx");

                entity.HasIndex(e => new { e.FilmPersonPersonId, e.FilmPersonFilmId, e.FilmPersonPersonTypeId }, "index5")
                    .IsUnique();

                entity.Property(e => e.FilmPersonPersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("FilmPerson_Person_Id");

                entity.Property(e => e.FilmPersonFilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("FilmPerson_Film_Id");

                entity.Property(e => e.FilmPersonPersonTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("FilmPerson_PersonType_Id");

                entity.HasOne(d => d.FilmPersonFilm)
                    .WithMany(p => p.Filmpeople)
                    .HasForeignKey(d => d.FilmPersonFilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_has_Film_Film1");

                entity.HasOne(d => d.FilmPersonPerson)
                    .WithMany(p => p.Filmpeople)
                    .HasForeignKey(d => d.FilmPersonPersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Person_has_Film_Person1");

                entity.HasOne(d => d.FilmPersonPersonType)
                    .WithMany(p => p.Filmpeople)
                    .HasForeignKey(d => d.FilmPersonPersonTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_FilmPerson_PersonType1");
            });

            modelBuilder.Entity<Following>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("following");

                entity.HasIndex(e => e.FollowingFollowerMemberId, "fk_Following_Member1_idx");

                entity.HasIndex(e => e.FollowingFollowingMemberId1, "fk_Following_Member2_idx");

                entity.Property(e => e.FollowingFollowerMemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Following_Follower_Member_Id");

                entity.Property(e => e.FollowingFollowingMemberId1)
                    .HasColumnType("int(11)")
                    .HasColumnName("Following_Following_Member_Id1");

                entity.HasOne(d => d.FollowingFollowerMember)
                    .WithMany()
                    .HasForeignKey(d => d.FollowingFollowerMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Following_Member1");

                entity.HasOne(d => d.FollowingFollowingMemberId1Navigation)
                    .WithMany()
                    .HasForeignKey(d => d.FollowingFollowingMemberId1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Following_Member2");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.HasIndex(e => e.GenreName, "Genre_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.GenreId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Genre_Id");

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("Genre_name");
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.ToTable("list");

                entity.HasIndex(e => e.ListId, "List_Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.ListCreatorMemberId, "fk_List_Member1_idx");

                entity.Property(e => e.ListId)
                    .HasColumnType("int(11)")
                    .HasColumnName("List_Id");

                entity.Property(e => e.ListCreatorMemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("List_Creator_Member_Id");

                entity.Property(e => e.ListName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("List_Name");

                entity.HasOne(d => d.ListCreatorMember)
                    .WithMany(p => p.Lists)
                    .HasForeignKey(d => d.ListCreatorMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_List_Member1");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("member");

                entity.HasIndex(e => e.MemberId, "Member_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.MemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Member_Id");

                entity.Property(e => e.MemberBio)
                    .HasMaxLength(45)
                    .HasColumnName("Member_Bio")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.MemberEmail)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("Member_Email");

                entity.Property(e => e.MemberName)
                    .HasMaxLength(45)
                    .HasColumnName("Member_Name")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.MemberUsername)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("Member_Username");

                entity.Property(e => e.MemberVorname)
                    .HasMaxLength(45)
                    .HasColumnName("Member_Vorname")
                    .HasDefaultValueSql("'NULL'");
            });

            modelBuilder.Entity<Memberlikelist>(entity =>
            {
                entity.HasKey(e => new { e.MemberLikeListMemberId, e.MemberLikeListListId })
                    .HasName("PRIMARY");

                entity.ToTable("memberlikelist");

                entity.HasIndex(e => e.MemberLikeListListId, "fk_Member_has_List_List1_idx");

                entity.HasIndex(e => e.MemberLikeListMemberId, "fk_Member_has_List_Member1_idx");

                entity.Property(e => e.MemberLikeListMemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("MemberLikeList_Member_Id");

                entity.Property(e => e.MemberLikeListListId)
                    .HasColumnType("int(11)")
                    .HasColumnName("MemberLikeList_List_Id");

                entity.HasOne(d => d.MemberLikeListList)
                    .WithMany(p => p.Memberlikelists)
                    .HasForeignKey(d => d.MemberLikeListListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_List_List1");

                entity.HasOne(d => d.MemberLikeListMember)
                    .WithMany(p => p.Memberlikelists)
                    .HasForeignKey(d => d.MemberLikeListMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Member_has_List_Member1");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("person");

                entity.HasIndex(e => e.PersonId, "Person_Id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.PersonId)
                    .HasColumnType("int(11)")
                    .HasColumnName("Person_Id");

                entity.Property(e => e.PersonBio)
                    .HasMaxLength(45)
                    .HasColumnName("Person_Bio")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.PersonName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("Person_Name");
            });

            modelBuilder.Entity<Persontype>(entity =>
            {
                entity.ToTable("persontype");

                entity.HasIndex(e => e.Name, "Name_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.PersonTypeId, "idPersonType_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.PersonTypeId)
                    .HasColumnType("int(11)")
                    .HasColumnName("PersonType_Id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Watchevent>(entity =>
            {
                entity.ToTable("watchevent");

                entity.HasIndex(e => e.WatchEventId, "WatchEvent_Id_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.WatchEventFilmId, "fk_WatchEvent_Film1_idx");

                entity.HasIndex(e => e.WatchEventMemberId, "fk_WatchEvent_Member1_idx");

                entity.Property(e => e.WatchEventId)
                    .HasColumnType("int(11)")
                    .HasColumnName("WatchEvent_Id");

                entity.Property(e => e.WatchEventDate).HasColumnName("WatchEvent_Date");

                entity.Property(e => e.WatchEventFilmId)
                    .HasColumnType("int(11)")
                    .HasColumnName("WatchEvent_Film_Id");

                entity.Property(e => e.WatchEventMemberId)
                    .HasColumnType("int(11)")
                    .HasColumnName("WatchEvent_Member_Id");

                entity.Property(e => e.WatchEventRating)
                    .HasColumnType("int(11)")
                    .HasColumnName("WatchEvent_Rating")
                    .HasDefaultValueSql("'NULL'");

                entity.Property(e => e.WatchEventText)
                    .HasMaxLength(45)
                    .HasColumnName("WatchEvent_Text")
                    .HasDefaultValueSql("'NULL'");

                entity.HasOne(d => d.WatchEventFilm)
                    .WithMany(p => p.Watchevents)
                    .HasForeignKey(d => d.WatchEventFilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_WatchEvent_Film1");

                entity.HasOne(d => d.WatchEventMember)
                    .WithMany(p => p.Watchevents)
                    .HasForeignKey(d => d.WatchEventMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_WatchEvent_Member1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
