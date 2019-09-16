using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
	public class Account
	{
		[Key]
		public int AID {get;set;}

		[Required(ErrorMessage="First name required!")]
		[MinLength(2,ErrorMessage="Name must be at least 2 characters long!")]
		public string FirstName {get;set;}

		[Required(ErrorMessage="Last name is required!")]
		[MinLength(2,ErrorMessage="Name must be at least 2 characters long!")]
		public string LastName {get;set;}

		[Required(ErrorMessage="Email required!")]
		[EmailAddress(ErrorMessage="Invalid email format!")]
		public string Email {get;set;}

		[Required(ErrorMessage="Password required!")]
		[MinLength(8,ErrorMessage="Password must be at least 8 characters long!")]
		[DataType(DataType.Password)]
		public string Password {get;set;}

		public DateTime CreatedAt {get;set;} = DateTime.Now;

		public DateTime UpdatedAt {get;set;} = DateTime.Now;

		public double Balance {get;set;} = 200.00;

		public List<Transaction> Trans {get;set;}

		[NotMapped]
		[Compare("Password",ErrorMessage="Passwords must match!")]
		[DataType(DataType.Password)]
		public string Confirm {get;set;}
	}
}