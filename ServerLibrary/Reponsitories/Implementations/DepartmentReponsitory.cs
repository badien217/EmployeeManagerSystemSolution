using BaseLibrary.Entities;
using BaseLibrary.Reponses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Reponsitories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reponsitories.Implementations
{
    public class DepartmentReponsitory(AppDbContext appDbContext): IGenericReponsitoryInterface<Department>
    {

        public async Task<GeneralResponse> DeleteById(int Id)
        {
            var dep = await appDbContext.Departments.FindAsync(Id);
            if (dep is null) return NotFound();
            appDbContext.Departments.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<Department>> GetAll() => await appDbContext.Departments
            .AsNoTracking().Include(x => x.GeneralDepartment).ToListAsync();


        public async Task<Department> GetById(int Id) => await appDbContext.Departments.FindAsync(Id);


        public async Task<GeneralResponse> Insert(Department item)
        {
            var checkIsNull = await CheckName(item.Name);
            if (checkIsNull) return new GeneralResponse(false, "Department already added");
            appDbContext.Departments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Department item)
        {
            var der = await appDbContext.Departments.FindAsync(item.Id);
            if (der is null) return NotFound();
            der.Name = item.Name;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry department not found");
        private static GeneralResponse Success() => new(true, "Process complete");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.GeneralDepartments.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null ? true : false;
        }
    }
}
