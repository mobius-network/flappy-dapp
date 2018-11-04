using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.API.Models
{
	public class TokenRequest
	{
		[Required]
		public string PublicKey { get; set; }
		[Required]
		public string Xdr { get; set; }
	}
}
