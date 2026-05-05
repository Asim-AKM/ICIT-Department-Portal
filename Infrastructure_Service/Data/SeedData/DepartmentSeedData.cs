using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Service.Data.SeedData
{
    // Infrastructure_Service/Data/SeedData/DepartmentSeed.cs
    using Domain_Service.Entities.Academic;
    using Domain_Service.Enum;

    namespace Infrastructure_Service.Data.SeedData
    {
        public static class DepartmentSeed
        {
            public static List<Department> GetDepartments()
            {
                return new List<Department>
                {
                    new Department
                    {
                        DepartmentId = Guid.Parse("19536958-3ab4-44e4-8848-104dfd93aaf8"),
                        Name = "Computer Science",
                        Code = "CS",
                        Description = "Department of Computer Science - Offers BSCS & MSCS programs",
                        Status = DepartmentStatus.Active
                    },
                    new Department
                    {
                        DepartmentId =  Guid.Parse("2b1c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e"),
                        Name = "Software Engineering",
                        Code = "SE",
                        Description = "Department of Software Engineering - Offers BSSE & MSSE programs",
                        Status = DepartmentStatus.Active
                    },
                    new Department
                    {
                        DepartmentId =  Guid.Parse("3c4d5e6f-7a8b-9c0d-1e2f-3a4b5c6d7e8f"),
                        Name = "Artificial Intelligence",
                        Code = "AI",
                        Description = "Department of Artificial Intelligence - Offers BSAI & MSAI programs",
                        Status = DepartmentStatus.Active
                    },

                     new Department
                    {
                        DepartmentId =  Guid.Parse("4d5e6f7a-8b9c-0d1e-2f3a-4b5c6d7e8f9a"),
                        Name = "Computer Engineering",
                        Code = "CE",
                        Description = "Department of Computer Engineering - Offers BSCE & MSCE programs",
                        Status = DepartmentStatus.Active
                    }

                };
            }
        }
    }
}
