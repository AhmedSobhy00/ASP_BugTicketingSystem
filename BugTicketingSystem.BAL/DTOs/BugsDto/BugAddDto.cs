﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BAL.DTOs.BugsDto
{
    public class BugAddDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
    }
}
