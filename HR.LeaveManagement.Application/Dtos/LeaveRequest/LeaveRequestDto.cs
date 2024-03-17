﻿using HR.LeaveManagement.Application.Dtos.Common;
using HR.LeaveManagement.Application.Dtos.LeaveType;

namespace HR.LeaveManagement.Application.Dtos.LeaveRequest
{
    public class LeaveRequestDto : BaseDto
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int LeaveTypeId { get; set; }

        public LeaveTypeDto LeaveType { get; set; }

        public DateTime DateRequested { get; set; }

        public string? RequestComments { get; set; }

        public DateTime? DateActioned { get; set; }

        public bool? Approved { get; set; }

        public bool Cancelled { get; set; }
    }
}
