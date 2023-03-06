using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Net.Mime;

namespace DirServe3.Pages
{
	public class FileModel : PageModel
	{
        string? url { get; set; }
        string? base_dir { get; set; }
        string? strip_url
        {
            get
            {
                if (url is string ourl)
                {                    
                    if (ourl.EndsWith('/'))
                    {
                        ourl = ourl.ToCharArray().Reverse().Skip(1).Reverse().Aggregate("", (prev, next) => $"{prev}{next}");

                    }
                    if (ourl.StartsWith('/'))
                    {
                        ourl = ourl.ToCharArray().Skip(1).Aggregate("", (prev, next) => $"{prev}{next}");
                    }
                    return ourl;
                }
                return null;
            }

        }
        private readonly ILogger<IndexModel> _logger;

		public FileModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}
        string? parent_url
        {
            get
            {
                if (url is string ourl)
                {                    
                    ourl = string.Join('/', ourl.Split('/').Reverse().Skip(1).Reverse().ToArray());
                    return $"/dir/{ourl}";
                }
                return null;
                
            }
        }
        
        public ActionResult OnGet()
		{
            
            url = RouteData.Values["url"] as string;
            url = url != null ? url : "/";
            base_dir = AppContext.BaseDirectory;
            var file_path = $"{base_dir}/{url}";
            
            new FileExtensionContentTypeProvider().TryGetContentType(file_path, out var contentType);
            contentType = contentType ?? "application/octet-stream";
            try
            {
                var fs = new System.IO.FileStream(file_path, FileMode.Open, FileAccess.Read);
                {
                    return File(fs, contentType);
                }

            }
            catch 
            {
                return NotFound();
            }



        }
    }
}