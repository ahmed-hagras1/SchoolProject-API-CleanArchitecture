using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Views;
using SchoolProject.Service.Abstracts.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations.Views
{
    public class ViewService : IViewService
    {
        // Inject the generic IViewRepository for your specific View entity
        private readonly IViewRepository<ViewDepartmentStudentCount> _viewRepository;

        public ViewService(IViewRepository<ViewDepartmentStudentCount> viewRepository)
        {
            _viewRepository = viewRepository;
        }

        public async Task<List<ViewDepartmentStudentCount>> GetDepartmentStudentCountListAsync()
        {
            // Call the repository, ensure we aren't tracking, and execute the SQL query
            return await _viewRepository.GetTableNoTracking().ToListAsync();
        }
    }
}