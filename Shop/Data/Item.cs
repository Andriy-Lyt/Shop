namespace Shop.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }
        
        public virtual Category Category { get; set; }

        public string PicExtension { get; set; }

        public bool HasPic { get; set; }

        public int Price { get; set; }

    }
}
