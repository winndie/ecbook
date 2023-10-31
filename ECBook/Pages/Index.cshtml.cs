using ECBook.Data.Entities;
using ECBook.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ECBook.App.Pages;
public class IndexModel : PageModel
{
    public Context DbContext { get; set; }

    public List<Post> Posts { get; set; }

    public readonly string NewsApi;

    public News News { get; set; }

    public IndexModel(Context context, IConfiguration Configuration)
    {
        DbContext = context;
        string url = Configuration["NewsApiUrl"] + Configuration["NewsApiKey"];
        NewsApi = url;
    }

    public async Task OnGetAsync()
    {
        await LoadPosts();
        using (var httpClient = new HttpClient())
        {
            using (HttpResponseMessage response = await httpClient.GetAsync(NewsApi))
            {
                var data = JsonConvert.DeserializeObject<NewsResponse>(await response.Content.ReadAsStringAsync());

                if (data != null && data.Data.Count() > 0)
                {
                    News = data.Data[0];
                }
            }
        }
    }

    private async Task LoadPosts()
    {
        Posts = await DbContext.Post.Include(x => x.Author).ToListAsync();
    }
}
