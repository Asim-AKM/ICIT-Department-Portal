using Application_Service.RequestAndResponseModel.AnnouncementRequestModel;
using Application_Service.Services.AnnouncementServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application_Service.Services.AnnouncementServices.Implementation.AudienceNotificationResolverService;

namespace Application_Service.Services.AnnouncementServices.Implementation
{
    public class AudienceNotificationResolverService : IAudienceNotificationResolverService
    {

        private readonly IUnitOfWork _uow;

        public AudienceNotificationResolverService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Guid>> ResolveUsersAsync(AnnouncementRequest request)
        {
            var userIds = new List<Guid>();

            // 🔹 Base users query
            var usersQuery = _uow.UserRepo.Query();

            // 🔹 Filter by Department (if provided)
            if (request.DepartmentId.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.DepartmentId == request.DepartmentId);
            }

            var users = await usersQuery.ToListAsync();

            // 🔥 Now apply audience logic
            foreach (var user in users)
            {
                switch (request.AnnouncementTargetAudience)
                {
                    case AnnouncementTargetAudience.Everyone:
                        userIds.Add(user.UserId);
                        break;

                    case AnnouncementTargetAudience.StudentsOnly:
                        if (await IsStudent(user.UserId))
                            userIds.Add(user.UserId);
                        break;

                    case AnnouncementTargetAudience.FacultiesOnly:
                        if (await IsFaculty(user.UserId))
                            userIds.Add(user.UserId);
                        break;

                    case AnnouncementTargetAudience.ClerksOnly:
                        if (await IsClerk(user.UserId))
                            userIds.Add(user.UserId);
                        break;
                }
            }

            return userIds;
        }

        // 🔹 Helpers (simple existence checks)

        private async Task<bool> IsStudent(Guid userId)
        {
            return await _uow.StudentRepo.Query()
                .AnyAsync(s => s.UserId == userId);
        }

        private async Task<bool> IsFaculty(Guid userId)
        {
            return await _uow.FucaltyRepo.Query()
                .AnyAsync(f => f.UserId == userId);
        }

        private async Task<bool> IsClerk(Guid userId)
        {
            return await _uow.ClerkRepo.Query()
                .AnyAsync(c => c.UserId == userId);
        }
    }
}

