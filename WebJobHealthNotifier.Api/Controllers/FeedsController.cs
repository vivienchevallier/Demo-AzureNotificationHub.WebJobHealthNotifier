using System.Collections.Generic;
using System.Web.Http;

namespace WebJobHealthNotifier.Api.Controllers
{
	public class FeedsController : ApiController
	{
		// GET api/<controller>
		public IEnumerable<string> Get()
		{
			return new string[] { "JobsFailing", "JobsSuccessful" };
		}
	}
}