using Application_Service.Services.StudentServices.Interfaces;
using Domain_Service.Entities.StudentModule;
using Domain_Service.Enum;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Application_Service.Services.StudentServices.Implementation
{
    public class StudentService : IStudentService
    {
        public async Task<string> UploadStudentsFromExcelAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++) // skip header
            {
                var student = new Student
                {
                    //StudentId = Guid.NewGuid(),
                    //RollNumber = worksheet.Cells[row, 1].Text,
                    //RegistrationNumber = worksheet.Cells[row, 2].Text,
                    //SamesterId = Guid.Parse(worksheet.Cells[row, 3].Text),
                    //Email = worksheet.Cells[row, 4].Text,
                    //Status = StudentStatus.Pending
                };

                //await _studentRepo.AddStudentAsync(student);
            }

            return $"{rowCount - 1} students uploaded successfully.";
        }

    }
}
