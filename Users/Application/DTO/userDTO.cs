using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.DTO
{
    public class NewUserDTO
    {
        public required string Individual_Insititution_ID { get; set; }
        public required string Individual_Name { get; set; }
        public string? Individual_Email { get; init; }

        public required string Individual_Type_Value { get; set; }
        public string? Individual_Contact_Number_Primary { get; init; }
        public bool Individual_Is_Alumni { get; init; }


        public IEnumerable<ProgramInfoDTO>? Academic_Details { get; init; }

    }


    

}
