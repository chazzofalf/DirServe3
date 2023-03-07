using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Transactions;

namespace DirServe3.Pages
{
	public class FilesModel : PageModel
	{
        
        public string? url { get; set; }
        public string? base_dir { get; set; }
        public string? files_dir { get; set; }
        public string? strip_url
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
                else
                {
                    return null;
                }

            }

        }
        public string? parent_url
        {
            get
            {
                if (url is string ourl)
                {
                    ourl = string.Join('/', ourl.Split('/').Reverse().Skip(1).Reverse().ToArray());
                    return $"/files/{ourl}";
                }
                else
                {
                    return null;
                }

            }
        }
        public string? url_for_entry(string filename, bool isFile)
        {
            var outx = (string?)null;
            if (strip_url is string ostrip_url)
            {
                if (isFile && ostrip_url.Length > 0)
                {
                    outx = $"/files/{ostrip_url}/{filename}";
                }
                else if (isFile)
                {
                    outx = $"/files/{filename}";
                }
                else if (ostrip_url.Length > 0)
                {
                    outx = $"/files/{ostrip_url}/{filename}";
                }
                else
                {
                    outx = $"/files/{filename}";
                }
            }

            return outx;
        }
        private readonly ILogger<IndexModel> _logger;

		public FilesModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
                
        }

		public ActionResult OnGet()
		{
            url = RouteData.Values["url"] as string;
            url = url != null ? url : "/";
            base_dir = AppContext.BaseDirectory;
            if (base_dir.EndsWith('/'))
            {
                base_dir = base_dir.ToCharArray().Reverse().Skip(1).Reverse().Aggregate("", (prev, next) => $"{prev}{next}");

            }
            files_dir = $"{base_dir}/files";
            if (!System.IO.Directory.Exists(files_dir))
            {
                _ = System.IO.Directory.CreateDirectory(files_dir);
            }

            var file_path = $"{files_dir}/{url}";
            if (System.IO.Directory.Exists(file_path))
            {
                return Page();
            }
            else if (System.IO.File.Exists(file_path))
            {
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
            else
            {
                return NotFound();
            }
                
        }
	}
}