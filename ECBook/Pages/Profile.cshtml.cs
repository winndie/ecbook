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
                var posts = await DbContext.Post
                    .Include(x => x.Author)
                    .Where(x => x.AuthorId == ProfileId).ToListAsync();

                if (posts != null && posts.Count() > 0)
                {
                    Post = posts.OrderByDescending(x => x.DateTimePosted).First();
                }
            }

            await LoadProfiles();
        }

        private async Task LoadProfiles()
        {
            Profiles = await DbContext.Profile.ToListAsync();
        }

        public Post? Post { get; set; } = null;
    }
}
