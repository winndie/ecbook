using System.ComponentModel.DataAnnotations;
using ECBook.Data;
using ECBook.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECBook.App.Pages
{
    public class CreateModel : PageModel
    {
        public Context DbContext { get; set; }

        public List<Profile> Profiles { get; set; }

        public bool Success { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Choose a person to post on behalf of")]
        [Range(1, 10000, ErrorMessage = "Choose a person to post on behalf of")]
        public int ProfileId { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Write a post")]
        [MinLength(1, ErrorMessage = "Post needs some content")]
        public string ContentInput { get; set; }

        public CreateModel(Context context)
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
                DbContext.Post.Add(new Post { AuthorId = ProfileId, Content = ContentInput, DateTimePosted = DateTimeOffset.Now });
                await DbContext.SaveChangesAsync();
                Success = true;
            }

            await LoadProfiles();
        }

        private async Task LoadProfiles()
        {
            Profiles = await DbContext.Profile.ToListAsync();
        }
    }
}
