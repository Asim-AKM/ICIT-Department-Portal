using Application_Service.Common;
using Application_Service.DTO_s.SubjectDTO_s;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.RequestAndResponseModel.Pagination;
using Application_Service.RequestAndResponseModel.SubjectManagmengModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.SubjectServices.Interfaces
{
    public interface ISubjectService
    {
        Task<ApiResponse<string>> CreateSubject(CreateSubjectRequest request);
        Task<ApiResponse<string>> UpdateSubject(UpdateSubjectRequest request);
        Task<ApiResponse<PaginationResponse<GetSubjectDto>>> GetAllSubject(int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<PaginationResponse<GetSubjectDto>>> GetSubjectsByDepartmentAndSemester(Guid DepartmentId, Guid SemesterId, int pageNumber = 1, int pageSize = 10);
        Task<ApiResponse<string>> DeleteSubject(Guid subjectId);
    }
}
