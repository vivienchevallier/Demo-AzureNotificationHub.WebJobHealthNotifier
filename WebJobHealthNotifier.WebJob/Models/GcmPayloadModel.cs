using Newtonsoft.Json;

namespace WebJobHealthNotifier.WebJob.Models
{
	public sealed class GcmPayloadModel
	{
		[JsonProperty("data")]
		public object Data { get; set; }
	}
}
