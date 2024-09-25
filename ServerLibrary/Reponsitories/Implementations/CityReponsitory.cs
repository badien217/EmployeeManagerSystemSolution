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
    public class CityReponsitory(AppDbContext appDbContext) : IGenericReponsitoryInterface<City>
    {

        public async Task<GeneralResponse> DeleteById(int Id)
        {
            var dep = await appDbContext.Cities.FindAsync(Id);
            if (dep is null) return NotFound();
            appDbContext.Cities.Remove(dep);
            await Commit();
            return Success();
        }

        public async Task<List<City>> GetAll() => await appDbContext.Cities.ToListAsync();


        public async Task<City> GetById(int Id) => await appDbContext.Cities.FindAsync(Id);


        public async Task<GeneralResponse> Insert(City item)
        {
            if (!await CheckName(item.Name!)) return new GeneralResponse(false, "City already added");
            appDbContext.Cities.Add(item);
            await Commit();
            return Success();
        }

        public async Task<GeneralResponse> Update(City item)
        {
            var der = await appDbContext.Cities.FindAsync(item.Id);
            if (der is null) return NotFound();
            der.Name = item.Name;
            await Commit();
            return Success();
        }
        private static GeneralResponse NotFound() => new(false, "Sorry City not found");
        private static GeneralResponse Success() => new(true, "Process complete");
        private async Task Commit() => await appDbContext.SaveChangesAsync();
        private async Task<bool> CheckName(string name)
        {
            var item = await appDbContext.Cities.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
            return item is null;
        }
    }
}
