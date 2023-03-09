using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
