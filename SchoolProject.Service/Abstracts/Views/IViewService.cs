using SchoolProject.Data.Entities.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts.Views
{
    public interface IViewService
    {
        // Define the contract to get the list of departments and their student counts
        Task<List<ViewDepartmentStudentCount>> GetDepartmentStudentCountListAsync();
    }
}