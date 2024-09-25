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
    public class TownRepository(AppDbContext appDbContext) : IGenericReponsitoryInterface<Town>
    {

        public async Task<GeneralResponse> DeleteById(int Id)
        {
            var dep = await appDbContext.Town.FindAsync(Id);
            if (dep is null) return NotFound();
            appDbContext.Town.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<Town>> GetAll() => await appDbContext.Town.ToListAsync();


        public async Task<Town> GetById(int Id) => await appDbContext.Town.FindAsync(Id);


        public async Task<GeneralResponse> Insert(Town item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "Town already added");
            appDbContext.Town.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(Town item)
        {
            var der = await appDbContext.Town.FindAsync(item.Id);
            if (der is null) return NotFound();
            der.Name = item.Name;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry Town not found");
        private static GeneralResponse Success() => new(true, "Process complete");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Town.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}