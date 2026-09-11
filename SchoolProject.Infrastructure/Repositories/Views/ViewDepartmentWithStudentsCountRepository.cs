using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities.Views;
using SchoolProject.Infrastructure.Data; // Assuming this is where AppDbContext is
using SchoolProject.Service.Abstracts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Repositories.Views
{
    public class ViewDepartmentWithStudentsCountRepository : IViewRepository<ViewDepartmentStudentCount>
    {
        private readonly AppDbContext _context;

        // 1. Inject the DbContext
        public ViewDepartmentWithStudentsCountRepository(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 🟢 READ METHODS (These work perfectly!)
        // ==========================================

        public IQueryable<ViewDepartmentStudentCount> GetTableNoTracking()
        {
            // Always use AsNoTracking for views! It is much faster.
            return _context.Set<ViewDepartmentStudentCount>().AsNoTracking().AsQueryable();
        }

        public IQueryable<ViewDepartmentStudentCount> GetTableAsTracking()
        {
            // We return AsNoTracking here too, because tracking a read-only view wastes RAM.
            return _context.Set<ViewDepartmentStudentCount>().AsNoTracking().AsQueryable();
        }

        public async Task<ViewDepartmentStudentCount> GetByIdAsync(int id)
        {
            // Assuming DeptId is the identifier we want to search by
            return await _context.Set<ViewDepartmentStudentCount>()
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(v => v.DeptId == id);
        }

        // ==========================================
        // 🛑 WRITE METHODS (Blocked intentionally)
        // ==========================================

        public Task<ViewDepartmentStudentCount> AddAsync(ViewDepartmentStudentCount entity)
        {
            throw new InvalidOperationException("Cannot insert data. This SQL View is Read-Only.");
        }

        public Task AddRangeAsync(ICollection<ViewDepartmentStudentCount> entities)
        {
            throw new InvalidOperationException("Cannot insert data. This SQL View is Read-Only.");
        }

        public Task UpdateAsync(ViewDepartmentStudentCount entity)
        {
            throw new InvalidOperationException("Cannot update data. This SQL View is Read-Only.");
        }

        public Task UpdateRangeAsync(ICollection<ViewDepartmentStudentCount> entities)
        {
            throw new InvalidOperationException("Cannot update data. This SQL View is Read-Only.");
        }

        public Task DeleteAsync(ViewDepartmentStudentCount entity)
        {
            throw new InvalidOperationException("Cannot delete data. This SQL View is Read-Only.");
        }

        public Task DeleteRangeAsync(ICollection<ViewDepartmentStudentCount> entities)
        {
            throw new InvalidOperationException("Cannot delete data. This SQL View is Read-Only.");
        }

        public Task SaveChangesAsync()
        {
            throw new InvalidOperationException("Cannot save changes. This SQL View is Read-Only.");
        }
    }
}