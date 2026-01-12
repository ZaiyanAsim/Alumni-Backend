using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.DTO
{
    public record projectDTO
    {
        public required string Project_Academic_ID { get; set; }
        public required string Project_Name { get; init; }

        public int? Project_Year { get; init; }

        public required string Project_Type { get; init; }

        public required string Project_Category { get; init; }

        public bool? Is_Mentored { get; init; }

        public bool? Is_Sponsored { get; init; }

    }

}
