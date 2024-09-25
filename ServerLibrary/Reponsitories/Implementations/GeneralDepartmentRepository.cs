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
    public class GeneralDepartmentRepository(AppDbContext appDbContext) : IGenericReponsitoryInterface<GeneralDepartment>
    {

        public async Task<GeneralResponse> DeleteById(int Id)
        {
            var dep = await appDbContext.GeneralDepartments.FindAsync(Id);
            if (dep is null) return NotFound();
            appDbContext.GeneralDepartments.Remove(dep);
            await Commit();
            return Success();

        }

        public async Task<List<GeneralDepartment>> GetAll() => await appDbContext.GeneralDepartments.ToListAsync();


        public async Task<GeneralDepartment> GetById(int Id) => await appDbContext.GeneralDepartments.FindAsync(Id);
        

        public async Task<GeneralResponse> Insert(GeneralDepartment item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Department already added");
            appDbContext.GeneralDepartments.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(GeneralDepartment item)
        {
            var der = await appDbContext.GeneralDepartments.FindAsync(item.Id);
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
            return item is null;
        }
    }
}
