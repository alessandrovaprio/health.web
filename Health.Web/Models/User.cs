using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using LinqToDB.Mapping;


namespace Health.Web.Models
{
    public class User : IHasId<int>
    {
        [PrimaryKey]
        [NotNull]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public bool Admin { get; set; }

		public bool Doctor { get; set; }

        public bool RememberMe { get; set; }

        public DateTime Timestamp { get; set; }

        public string ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                string emailRegex = @"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(email))
                {
                    //ModelState.AddModelError("Email", "Inserisci un indirizzo email corretto");
                    return "Inserisci un indirizzo email corretto";
                }
            }
            else
            {
                //ModelState.AddModelError("Email", "Inserisci un indirizzo email");
                return "Inserisci un indirizzo email";
            }
            return "";
        }
    }


}