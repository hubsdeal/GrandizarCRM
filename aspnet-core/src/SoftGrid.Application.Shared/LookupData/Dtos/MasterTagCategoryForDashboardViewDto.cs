using System;
using System.Collections.Generic;
using System.Text;

namespace SoftGrid.LookupData.Dtos
{
    public class MasterTagCategoryForDashboardViewDto: MasterTagCategoryDto
    {
        public List<MasterTagForDashboardViewDto> MasterTags { get; set; }

        public bool DisplayPublic { get; set; }
        public int? DisplaySequence { get; set; }

        public MasterTagCategoryForDashboardViewDto()
        {
            MasterTags= new List<MasterTagForDashboardViewDto>();
        }
    }

    public class MasterTagForDashboardViewDto: MasterTagDto
    {
        public bool IsSelected { get; set; }
    }
}
