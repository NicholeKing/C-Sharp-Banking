using System;
using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
	public class Transaction
	{
		[Key]
		public int TID {get;set;}

		[Required(ErrorMessage="Amount required for transaction!")]
		public double Amount {get;set;}

		public DateTime CreatedAt {get;set;} = DateTime.Now;

		public int AID {get;set;}

		public Account Trans {get;set;}
	}
}