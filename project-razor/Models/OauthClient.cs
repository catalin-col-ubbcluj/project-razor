using System.ComponentModel.DataAnnotations;

namespace project_razor.Models
{
    public class OauthClient
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Client ID")]
        public string ClientId { get; set; } = string.Empty;

        [Required]
        [Url]
        [Display(Name = "Redirect URI")]
        public string RedirectUri { get; set; } = string.Empty;
    }
}
