namespace DvdLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int MovieCopyId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime RentedUntil { get; set; }

        public virtual Client Client { get; set; }

        public virtual MovieCopy MovieCopy { get; set; }
    }
}
