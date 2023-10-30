using ECBook.Data.Entities;
using ECBook.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECBook.App.Pages;
public class IndexModel : PageModel
{
    public Context DbContext { get; set; }

    public List<Post> Posts { get; set; }

    public IndexModel(Context context)
    {
        DbContext = context;
    }

    public async Task OnGetAsync()
    {
        await LoadPosts();
    }

    private async Task LoadPosts()
    {
        Posts = await DbContext.Post.Include(x => x.Author).ToListAsync();
    }
}
