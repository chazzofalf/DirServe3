using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DirServe3.Pages
{
	public class DirModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public DirModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			
		}
	}
}