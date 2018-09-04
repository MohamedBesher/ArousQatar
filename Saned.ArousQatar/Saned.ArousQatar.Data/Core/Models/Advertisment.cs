using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Saned.ArousQatar.Data.Core.Models
{
    public sealed class Advertisment : IEntityBase
    {
        public Advertisment()
        {
            CreateDate = DateTime.Now;
            NumberOfViews = 0;
            NumberOfLikes = 0;
            IsPaided = false;
            IsExpired= true;
            IsArchieved = false;
            AdvertismentImages = new List<AdvertismentImage>();
            Favoriteses = new List<Favorite>();
            Commentses = new List<Comment>();
            Likes = new List<Like>();
            Complaints = new List<Complaint>();
        }

        public int Id { get; set; }

        [DefaultValue("true")]
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
        public bool IsPaided { get; set; }
        public string Description { get; set; }
        public int NumberOfViews { get; set; }
        public int NumberOfLikes { get; set; }
        public decimal? PaidEdPrice { get; set; }
        public bool IsArchieved { get; set; } = false;
        public DateTime CreateDate { get; private set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsExpired { get; set; } 
        public decimal Cost { get; set; } 


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public AdvertismentPrice AdvertismentPrice { get; set; }
        public int? AdvertismentPriceId { get; set; }

        public ICollection<AdvertismentImage> AdvertismentImages { get; set; }
        public ICollection<Favorite> Favoriteses { get; set; }
        public ICollection<Comment> Commentses { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Complaint> Complaints { get; set; }

        public void Archieve()
        {
            IsArchieved = true;
        }

        public void Modify(string name,
            string description,
            decimal? paidEdPrice,
            DateTime? startDate, DateTime? endDate,
            int categoryId,
            int? advertisementPriceId,
            bool isPaided,
            bool isActive, decimal cost)
        {
            Name = name;
            Description = description;
            PaidEdPrice = paidEdPrice;
            StartDate = startDate;
            EndDate = endDate;
            CategoryId = categoryId;
            AdvertismentPriceId = advertisementPriceId;
            IsPaided = isPaided;
            IsActive = isActive;
            Cost = cost;
        }

        public void Modify(string name,
            string description,             
            int categoryId,
            bool isActive, decimal cost)
        {
            Name = name;
            Description = description;          
            CategoryId = categoryId;        
            IsActive = isActive;
            Cost = cost;

        }


        public void SetExpired(bool isExpired, DateTime? startDate, DateTime? endDate)
        {
            IsExpired = isExpired;
            StartDate = startDate;
            EndDate = endDate;

        }
    }
}
