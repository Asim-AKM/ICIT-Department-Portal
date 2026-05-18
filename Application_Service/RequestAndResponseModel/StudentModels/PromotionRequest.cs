using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application_Service.RequestAndResponseModel.StudentModels
{
    public class PromotionRequest
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid SemesterId { get; set; }
        public Guid DepartmentId { get; set; }
    }
    public class BatchPromotionRequest
    {
        public Guid SemesterId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
