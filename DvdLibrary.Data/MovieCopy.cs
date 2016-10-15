namespace DvdLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MovieCopy
    {
        public MovieCopy()
        {
            Orders = new HashSet<Order>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int MovieId { get; set; }

        public bool IsAvailable { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
