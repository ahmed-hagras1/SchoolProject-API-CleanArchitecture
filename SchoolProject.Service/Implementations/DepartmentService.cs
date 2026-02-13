using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        #region Fields
        private readonly IDepartmentRepository _departmentRepository;
        #endregion

        #region Constructor
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Methods / Handle functions
        public async Task<List<Department>> GetDepartmentsListAsync()
        {
            return await _departmentRepository.GetDepartmentsListAsync();
        }
        public async Task<Department> GetDepartmentsByIdWithIncludeAsync(int departmentId)
        {
            return await _departmentRepository.GetDepartmentsByIdWithIncludeAsync(departmentId);
        }

        public Task<bool> IsDepartmentExist(int? departmentId) => _departmentRepository.IsDepartmentExist(departmentId);
        #endregion
    }
}
