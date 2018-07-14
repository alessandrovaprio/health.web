using System;
using LinqToDB.Mapping;
using MySql.Data.EntityFrameworkCore;

namespace Health.Web.Models
{
    public class User
    {
		[PrimaryKey]
        [NotNull]
        public int Id { get; set; }

        public string Name { get; set; }

		public string Surname { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		/*public bool Admin { get; set; }

		public bool Doctor { get; set; }*/

        public DateTime Timestamp { get; set; }

        
    }
}