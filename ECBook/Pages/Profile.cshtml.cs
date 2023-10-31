using ECBook.Data;
using ECBook.Data.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECBook.App.Pages
{
    public class ProfileModel : PageModel
    {
        public Context DbContext { get; set; }

        public int ProfileId { get; set; }

        public ProfileWithLatestFeed? SelctedProfile { get; set; }
        public ProfileModel(Context context)
        {
            DbContext = context;
        }

        public async Task OnGetAsync(int profileId)
        {
            ProfileId = profileId;

            if (ProfileId >= 1 && ProfileId <= 1000)
            {
                var profileRepo = DbContext.Profile.AsQueryable();
                var feedRepo = DbContext.Post.AsQueryable();
                var query =
                    await (from p in profileRepo
                           where p.Id == ProfileId
                           select new
                           {
                               profile = p,
                               feeds = (from f in feedRepo where f.AuthorId == p.Id select f).ToList()
                           }).FirstAsync();

                if (query != null)
                {
                    SelctedProfile = new ProfileWithLatestFeed(query.profile, query.feeds);
                }
            }
        }
    }
}
