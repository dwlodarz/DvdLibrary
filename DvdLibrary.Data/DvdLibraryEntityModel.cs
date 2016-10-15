namespace DvdLibrary.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DvdLibraryEntityModel : DbContext
    {
        public DvdLibraryEntityModel()
            : base("name=DvdLibraryEntityModel")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<MovieCopy> MovieCopies { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MovieCopy>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.MovieCopy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.MovieCopies)
                .WithRequired(e => e.Movie)
                .HasForeignKey(e => e.MovieId)
                .WillCascadeOnDelete(false);
        }
    }
}
