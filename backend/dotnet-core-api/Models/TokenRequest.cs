using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_core_api.Models
{
	public class TokenRequest
	{
		[Required]
		public string Public_Key { get; set; }
		[Required]
		public string Xdr { get; set; }
	}
}
