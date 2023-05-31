namespace SoftGrid.TaskManagement.Dtos
{
    public class GetTaskTeamForViewDto
    {
        public TaskTeamDto TaskTeam { get; set; }

        public string TaskEventName { get; set; }

        public string EmployeeName { get; set; }

        public string ContactFullName { get; set; }

    }
}