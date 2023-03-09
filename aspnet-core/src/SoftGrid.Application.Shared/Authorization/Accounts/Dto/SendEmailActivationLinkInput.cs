using System.ComponentModel.DataAnnotations;

namespace SoftGrid.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}