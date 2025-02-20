using System;
using System.Collections.Generic;

namespace PickMe.Core.Models
{
    public class Survey
    {
        public Survey()
        {
            Comments = new List<Comment>();
            Likes = new List<Like>();
            Reports = new List<Report>();
            Votes = new List<Vote>();
            CreatedAt = DateTime.UtcNow;
            IsActive = false;
        }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image1Url { get; set; } = string.Empty;
        public string Image2Url { get; set; } = string.Empty;
        public int Image1Votes { get; set; }
        public int Image2Votes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string CreatedById { get; set; } = string.Empty;
        public virtual ApplicationUser CreatedBy { get; set; } = null!;
        
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
