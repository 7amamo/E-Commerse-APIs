﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class SpecParams
    {
        public string? Sort { get; set; }
		public int PageIndex { get; set; } = 1;
		private int pageSize =5;

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > 10 ? 10 : value; }
		}


		private string? search;

		public string? Search
		{
			get { return search; }
			set { search = value.ToLower(); }
		}



	}
}
