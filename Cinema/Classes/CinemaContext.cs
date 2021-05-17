using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Cinema
{
    public partial class CinemaContext : DbContext
    {
        public CinemaContext()
        {
        }

        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AllGenre> AllGenres { get; set; }
        public virtual DbSet<AvailablePlace> AvailablePlaces { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<FilmsRaitingsGenre> FilmsRaitingsGenres { get; set; }
        public virtual DbSet<Form> Forms { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MoviesSort> MoviesSorts { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<StateCinema> StateCinemas { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TypesView> TypesViews { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersName> UsersNames { get; set; }
        public virtual DbSet<staff> staff { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-62MFGKR; Database=Cinema; Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<AllGenre>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("All_genres");

                entity.Property(e => e.Genre)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AvailablePlace>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("AvailablePlaces");

                entity.Property(e => e.IdMovie).HasColumnName("ID_movie");

                entity.Property(e => e.IdTicket).HasColumnName("ID_ticket");

                entity.Property(e => e.NumberPlace).HasColumnName("Number_place");

                entity.Property(e => e.NumberRow).HasColumnName("Number_row");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__Clients__A9D105356B7ACABD");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameClient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_client");

                entity.Property(e => e.NumberTelephon)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("Number_telephon");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.EmailNavigation)
                    .WithOne(p => p.Client)
                    .HasForeignKey<Client>(d => d.Email)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Clients__Email__52593CB8");
            });

            modelBuilder.Entity<Film>(entity =>
            {
                entity.HasKey(e => e.IdFilm);

                entity.Property(e => e.IdFilm).HasColumnName("ID_film");

                entity.Property(e => e.AgeLimit).HasColumnName("Age_limit");

                entity.Property(e => e.AirDate)
                    .HasColumnType("date")
                    .HasColumnName("Air_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("End_date");

                entity.Property(e => e.NameFilm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_film");

                entity.Property(e => e.Poster).HasColumnType("image");
            });

            modelBuilder.Entity<FilmsRaitingsGenre>(entity =>
            {
                entity.HasKey(e => e.IdFilm);

                entity.ToView("Films_raitings_genres");

                entity.Property(e => e.IdFilm).HasColumnName("ID_film");

                entity.Property(e => e.AgeLimit).HasColumnName("Age_limit");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Genre)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.NameFilm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_film");

                entity.Property(e => e.Poster).HasColumnType("image");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasKey(e => e.IdForm);

                entity.Property(e => e.IdForm).HasColumnName("ID_form");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPost).HasColumnName("ID_post");

                entity.Property(e => e.NameClient)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telephon)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.IdPost)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Forms_Posts1");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => new { e.IdFilm, e.Genre1 });

                entity.Property(e => e.IdFilm).HasColumnName("ID_film");

                entity.Property(e => e.Genre1)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Genre");

                entity.HasOne(d => d.IdFilmNavigation)
                    .WithMany(p => p.Genres)
                    .HasForeignKey(d => d.IdFilm)
                    .HasConstraintName("FK_Genres_Films");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(e => e.IdMovie)
                    .HasName("PK_Movies_1");

                entity.Property(e => e.IdMovie)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_movie");

                entity.Property(e => e.IdFilm).HasColumnName("ID_film");

                entity.Property(e => e.IdRoom).HasColumnName("ID_room");

                entity.Property(e => e.MovieTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Movie_time");

                entity.HasOne(d => d.IdFilmNavigation)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.IdFilm)
                    .HasConstraintName("FK_Movies_Films");

                entity.HasOne(d => d.IdRoomNavigation)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.IdRoom)
                    .HasConstraintName("FK__Movies__Number_r__47DBAE45");
            });

            modelBuilder.Entity<MoviesSort>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("MoviesSort");

                entity.Property(e => e.IdFilm).HasColumnName("ID_film");

                entity.Property(e => e.IdMovie).HasColumnName("ID_movie");

                entity.Property(e => e.IdRoom).HasColumnName("ID_room");

                entity.Property(e => e.MovieTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Movie_time");

                entity.Property(e => e.NameFilm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_film");

                entity.Property(e => e.SalaryForPlace).HasColumnName("Salary_for_place");

                entity.Property(e => e.TypeView)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Type_view");
            });

            modelBuilder.Entity<Place>(entity =>
            {
                entity.HasKey(e => new { e.IdRoom, e.NumberRow, e.NumberPlace })
                    .HasName("PK__Places__1E6007412BBE5813");

                entity.Property(e => e.IdRoom).HasColumnName("ID_room");

                entity.Property(e => e.NumberRow).HasColumnName("Number_row");

                entity.Property(e => e.NumberPlace).HasColumnName("Number_place");

                entity.HasOne(d => d.IdRoomNavigation)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.IdRoom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Places_Rooms");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.IdPost);

                entity.Property(e => e.IdPost).HasColumnName("ID_post");

                entity.Property(e => e.NamePost)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("Name_post");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => e.IdFilm)
                    .HasName("PK_Raitings");

                entity.Property(e => e.IdFilm)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_film");

                entity.Property(e => e.CountOfPeopleInRaiting).HasColumnName("Count_of_people_in_raiting");

                entity.Property(e => e.Rating1).HasColumnName("Rating");

                entity.HasOne(d => d.IdFilmNavigation)
                    .WithOne(p => p.Rating)
                    .HasForeignKey<Rating>(d => d.IdFilm)
                    .HasConstraintName("FK_Raitings_Films");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__Roles__1E600F7AB709749F");

                entity.Property(e => e.IdRole).HasColumnName("ID_role");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_role");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.IdRoom)
                    .HasName("PK__Rooms__1E600741B143813B");

                entity.Property(e => e.IdRoom).HasColumnName("ID_room");

                entity.Property(e => e.CountPlaces).HasColumnName("Count_places");

                entity.Property(e => e.IdTypeView).HasColumnName("ID_type_view");

                entity.Property(e => e.SalaryForPlace).HasColumnName("Salary_for_place");

                entity.HasOne(d => d.IdTypeViewNavigation)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.IdTypeView)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Rooms_Types_view");
            });

            modelBuilder.Entity<StateCinema>(entity =>
            {
                entity.HasKey(e => e.IdPost);

                entity.ToTable("State_cinema");

                entity.Property(e => e.IdPost)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_post");

                entity.Property(e => e.CountStaffers).HasColumnName("Count_staffers");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithOne(p => p.StateCinema)
                    .HasForeignKey<StateCinema>(d => d.IdPost)
                    .HasConstraintName("FK_State_cinema_Posts");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.IdTicket);

                entity.Property(e => e.IdTicket).HasColumnName("ID_ticket");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdMovie).HasColumnName("ID_movie");

                entity.Property(e => e.ImageTicket).HasColumnType("image");

                entity.Property(e => e.NumberPlace).HasColumnName("Number_place");

                entity.Property(e => e.NumberRow).HasColumnName("Number_row");

                entity.HasOne(d => d.EmailNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.Email)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tickets__Email__5DCAEF64");

                entity.HasOne(d => d.IdMovieNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.IdMovie)
                    .HasConstraintName("FK_Tickets_Movies");
            });

            modelBuilder.Entity<TypesView>(entity =>
            {
                entity.HasKey(e => e.IdTypeView)
                    .HasName("PK__Types_vi__90EFC03E250FC140");

                entity.ToTable("Types_view");

                entity.Property(e => e.IdTypeView)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_type_view");

                entity.Property(e => e.TypeView)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Type_view");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PK__Users__A9D105355BC6BECF");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdRole).HasColumnName("ID_role");

                entity.Property(e => e.PasswordUser)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("Password_user");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK__Users__Number_ro__4F7CD00D");
            });

            modelBuilder.Entity<UsersName>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("UsersNames");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdRole).HasColumnName("ID_role");

                entity.Property(e => e.NameClient)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_client");

                entity.Property(e => e.NumberTelephon)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("Number_telephon");

                entity.Property(e => e.PasswordUser)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("Password_user");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.HasKey(e => e.IdStaffer)
                    .HasName("PK__Staff__0385FCF164859DF6");

                entity.ToTable("Staff");

                entity.Property(e => e.IdStaffer).HasColumnName("ID_staffer");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPost).HasColumnName("ID_post");

                entity.Property(e => e.NameStaffer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_staffer");

                entity.Property(e => e.NumberTelephon)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("Number_telephon");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK_Staff_Posts");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
