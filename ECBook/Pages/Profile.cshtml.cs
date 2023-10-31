using System.ComponentModel.DataAnnotations;
using ECBook.Data;
using ECBook.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECBook.App.Pages
{
    public class ProfileModel : PageModel
    {
        public Context DbContext { get; set; }

        public List<Profile> Profiles { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Choose a person to show the profile")]
        [Range(1, 10000, ErrorMessage = "Choose a person to show the profile")]
        public int ProfileId { get; set; }

        public ProfileModel(Context context)
        {
            DbContext = context;
        }

        public async Task OnGetAsync()
        {
            await LoadProfiles();
        }

        public async Task OnPostAsync()
        {
            if (ProfileId != -1)
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
                    SelctedProfile = new ProfileWithLatestFeed(query.profile,query.feeds);
                }
            }

            await LoadProfiles();
        }

        private async Task LoadProfiles()
        {
            Profiles = await DbContext.Profile.ToListAsync();
        }

        public ProfileWithLatestFeed? SelctedProfile { get; set; }
    }
}
