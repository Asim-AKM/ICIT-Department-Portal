using Domain_Service.Entities.Shared;
using Domain_Service.RepoInterfaces.NotificationRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Service.Persistance.Repositories.NotificationRepo_s
{
    public class NotificationRepo : Repository<Notification> , INotificationRepo
    {
        private readonly ApplicationDbContext _context;
        public NotificationRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
