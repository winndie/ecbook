﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECBook.Data.Entities
{
    public class ProfileWithLatestFeed : Profile
    {
        public readonly Post? LatestPost;
        public ProfileWithLatestFeed(Profile profile, List<Post> posts)
        {
            this.Id = profile.Id;
            this.FirstName = profile.FirstName;
            this.LastName = profile.LastName;
            this.JobTitle = profile.JobTitle;
            this.LatestPost = 
                posts != null && posts.Count() > 0 ?
                posts.OrderByDescending(x => x.DateTimePosted).First() : null;
        }

    }
}
