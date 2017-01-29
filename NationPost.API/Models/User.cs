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

        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed")]
        public String FirstName { get; set; }

        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed")]
        public String LastName { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(350), MinLength(2)]
        public String Email { get; set; }

        [MaxLength(15, ErrorMessage = "Only 15 characters are allowed"), MinLength(5), Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<Article> Articles { get; set; }

        public string AboutMe { get; set; }

        public bool IsAboutMeVisible { get; set; }



        public string FacebookLink { get; set; }
        public bool IsFacebookLinkVisible { get; set; }

        public string TwitterLink { get; set; }
        public bool IsTwitterLinkVisible { get; set; }


        public string Contact { get; set; }
        public string IsContactVisible { get; set; }

        //public DbGeography coords { get; set; }
        public String Token { get; set; }
        public string GoogleLink { get; internal set; }
        public string IsGoogleLinkVisible { get; internal set; }

        [MaxLength(100)]
        public string UserName { get; set; }
    }
}