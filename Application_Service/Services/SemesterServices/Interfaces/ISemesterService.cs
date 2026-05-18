using Application_Service.Common;
using Application_Service.DTO_s.SemesterDTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.SemesterServices.Interfaces
{
    public interface ISemesterService 
    {
        Task<ApiResponse<List<GetSemeterDto>>> GetSemestersAsync(Guid sessionId);
    }
}
