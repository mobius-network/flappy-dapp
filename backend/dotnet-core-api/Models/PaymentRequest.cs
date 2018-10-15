using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DotNetCore.API.Models
{
	public class PaymentRequest
	{
		[Required]
		public decimal Amount { get; set; }
		[DataMember(Name="target_address")]
		public string TargetAddress { get; set; }
	}
}
