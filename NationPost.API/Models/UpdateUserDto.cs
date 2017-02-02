using System;
using System.ComponentModel;

namespace NationPost.API.Models
{
    public class UpdateUserDto
    {
        public string AboutMe { get; set; }
        public bool IsAboutMeVisible { get; set; }

        public string FacebookLink { get; set; }
        public bool IsFacebookLinkVisible { get; set; }

        public string TwitterLink { get; set; }
        public bool IsTwitterLinkVisible { get; set; }

        public string Contact { get; set; }

        public bool IsContactVisible { get; set; }
        public string GoogleLink { get; set; }

        public bool IsGoogleLinkVisible { get; set; }
    }
    public class UpdateArticleDto
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public bool IsValid { get; set; }
        public bool IsVisible { get; set; }

        public Tag[] Tags { get; set; }

    }
}