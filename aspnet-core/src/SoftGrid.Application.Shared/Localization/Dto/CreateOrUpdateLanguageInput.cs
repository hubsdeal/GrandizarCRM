using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}