using Application_Service.Common;
using Application_Service.DTO_s.FacultyDTO_s;
using Application_Service.DTO_s.StudentGradDTO_s;
using Application_Service.DTO_s.SubjectDTO_s;
using Application_Service.RequestAndResponseModel.GradeManagmentModels;
using Domain_Service.Entities.Academic;

namespace Application_Service.Services.FacultyServices.Interfaces
{
    public interface IFacultyService
    {
        Task<ApiResponse<List<GetFacultyDTO>>> GetFacultiesByDepartmentAsync(Guid departmentId);
        Task<ApiResponse<List<GetSubjectDto>>> GetMySubjectsAsync(Guid facultyId);

        Task<ApiResponse<List<GetEnrolledStudentDto>>> GetEnrolledStudentsAsync(Guid subjectId, Guid userId);
        Task<ApiResponse<string>> AssignGradeAsync(CreateOrUpdateGradeRequest request, Guid userId);
       

    }
}
