using JetBrains.Annotations;

namespace SoftGrid.UtilityDtos;

public class SmSRequestDto
{
    public string AppSid { get; set; }
    public string SenderId { get; set; }
    public string Body { get; set; }
    public long? Recipient { get; set; }
    [CanBeNull] public string ResponseType { get; set; }
    [CanBeNull] public string CorrelationId { get; set; }
    public bool? BaseEncode { get; set; }
    [CanBeNull] public string StatusCallback { get; set; }
    public bool? Masync { get; set; } = false;
}