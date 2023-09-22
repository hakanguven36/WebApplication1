﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
	public class AccountsViewModel
	{
		public int ID { get; set; }

		[Required]
		[StringLength(30, MinimumLength = 4, ErrorMessage = "4-30 karakter olmalı!")]
		[DisplayName("Kullanıcı Adı")]
		public string username { get; set; }

		[Required]
		[StringLength(12, MinimumLength = 4, ErrorMessage = "4-12 karakter olmalı!")]
		[DataType(DataType.Password)]
		[DisplayName("Şifre")]
		public string password { get; set; }
	}
}