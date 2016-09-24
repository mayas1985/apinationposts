using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity.Spatial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NationPost.API.Models
{
    [Table("Users", Schema = "NP")]    
    public class User
    {
        public Guid UserId { get; set; }
        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed"), MinLength(5), Required]
        public String UserName { get; set; }

        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed")]
        public String FirstName { get; set; }

        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed")]
        public String LastName { get; set; }

        public String Email { get; set; }

        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed"), MinLength(5), Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<Article> Articles { get; set; }

        public DbGeography coords { get; set; }

        public String Token { get; set; }
    }
}