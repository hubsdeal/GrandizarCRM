using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.PublicCommon.Dtos
{
    public class GetStoreHourCurrentStatusForPublicViewDto
    {
        public bool IsOpen { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CurrentTime { get; set; }
    }
}
