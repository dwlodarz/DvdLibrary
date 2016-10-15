namespace DvdLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [KnownType(typeof(Movie))]
    public partial class Movie
    {
        public Movie()
        {
            MovieCopies = new HashSet<MovieCopy>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual ICollection<MovieCopy> MovieCopies { get; set; }
    }
}
