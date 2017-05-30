using System;
using Newtonsoft.Json;

namespace BadiMeischter.Model
{
	public class Coordinates
	{
		[JsonProperty(PropertyName = "0")]
		public string Lon { get; set; }

		[JsonProperty(PropertyName = "1")]
		public string Lat { get; set; }

	}
}
