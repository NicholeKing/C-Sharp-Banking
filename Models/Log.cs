using System;
using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
	public class Log
	{
		[Key]
		public int LID {get;set;}

		[Required(ErrorMessage="Email required!")]
		[EmailAddress(ErrorMessage="Invalid email format!")]
		public string Email {get;set;}

		[Required(ErrorMessage="Password required!")]
		public string Password {get;set;}
	}
}