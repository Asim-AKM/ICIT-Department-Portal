using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.StudentServices.Interfaces
{
    public interface ITranscriptService
    {
        Task<ApiResponse<TranscriptDto>> GetStudentTranscriptAsync(Guid userId);
    }
}
