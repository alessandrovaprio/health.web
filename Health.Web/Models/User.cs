using System;
using LinqToDB.Mapping;


namespace Health.Web.Models
{
    public class User : IHasId<int>
    {
		[PrimaryKey]
        [NotNull]
        public int Id { get; set; }

        public string Name { get; set; }

		public string Surname { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public bool Admin { get; set; }

		public bool Doctor { get; set; }

        public bool RememberMe { get; set; }

        public DateTime Timestamp { get; set; }

        
    }
}