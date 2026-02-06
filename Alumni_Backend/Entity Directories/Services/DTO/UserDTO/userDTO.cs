using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Directories.Services.DTO
{
    public class NewUserDTO
    {
        public required string Institution_ID { get; set; }
        public required string Name { get; set; }
        public string? Email { get; init; }

        public required int Type_ID { get; set; }
        public required string Type_Value { get; set; }
        public string? Contact_Number { get; init; }
        public bool Is_Alumni { get; init; }

        public required int Client_ID { get; set; }

        public required int Campus_ID { get; set; }




        public IEnumerable<AcademicDetailsDTO>? Academic_Details { get; init; }

    }


    public class AcademicDetailsDTO
    {
        public int?    Program_ID { get; init; }
        public string? Program_Value { get; init; }

        public int?    Department_ID { get; init; }
        public string? Department_Value { get; init; }

        public int?    Enrollment_Year { get; init; }
        public int?    Graduation_Year { get; init; }


        public int?    Designation_ID  {get;init;}
        public string? Designation_Value {  get; init; }

    }






    
    
}
