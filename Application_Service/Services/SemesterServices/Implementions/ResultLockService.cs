using Application_Service.Common;
using Application_Service.RequestAndResponseModel.StudentModels;
using Application_Service.Services.SemesterServices.Interfaces;
using Domain_Service.Entities.Academic;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.SemesterServices.Implementions
{
    public class ResultLockService : IResultLockService
    {
        private readonly IUnitOfWork _uow;
        public ResultLockService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<bool> IsResultLockedAsync(Guid semesterId, Guid departmentId)
        {
            return await _uow.ResultLockRepo.Query()
                .AnyAsync(x =>
                    x.SemesterId == semesterId &&
                    x.DepartmentId == departmentId &&
                    x.IsLocked);
        }
        public async Task<ApiResponse<string>> LockResultsAsync( LockResultRequest request,Guid lockedBy)
        {
            var existing = await _uow.ResultLockRepo.Query()
                .FirstOrDefaultAsync(x =>
                    x.SemesterId == request.SemesterId &&
                    x.DepartmentId == request.DepartmentId);

            if (existing == null)
            {
                existing = new ResultLock
                {
                    ResultLockId = Guid.NewGuid(),
                    SemesterId = request.SemesterId,
                    DepartmentId = request.DepartmentId,
                    IsLocked = true,
                    LockedAt = DateTime.UtcNow,
                    LockedBy = lockedBy,
                    Remarks = request.Remarks
                };

                await _uow.ResultLockRepo.CreateAsync(existing);
            }
            else
            {
                existing.IsLocked = true;
                existing.LockedAt = DateTime.UtcNow;
                existing.LockedBy = lockedBy;
                existing.Remarks = request.Remarks;

                await _uow.ResultLockRepo.Update(existing);
            }

            await _uow.SaveChangesAsync();

            return ApiResponse<string>.Success(
                "Results locked successfully",
                "Results locked successfully",
                ResponseType.Ok);
        }
        public async Task<ApiResponse<string>> UnlockResultsAsync(Guid semesterId,Guid departmentId)
        {
            var existing = await _uow.ResultLockRepo.Query()
                .FirstOrDefaultAsync(x =>
                    x.SemesterId == semesterId &&
                    x.DepartmentId == departmentId);

            if (existing == null)
            {
                return ApiResponse<string>.Fail(
                    "Result lock record not found",
                    ResponseType.NotFound);
            }

            existing.IsLocked = false;
            existing.LockedAt = null;
            existing.LockedBy = null;

            await _uow.ResultLockRepo.Update(existing);
            await _uow.SaveChangesAsync();

            return ApiResponse<string>.Success(
                "Results unlocked successfully",
                "Results unlocked successfully",
                ResponseType.Ok);
        }
    }
}
