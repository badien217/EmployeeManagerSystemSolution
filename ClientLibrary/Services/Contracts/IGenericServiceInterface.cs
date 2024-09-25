using BaseLibrary.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Contracts
{
    public interface IGenericServiceInterface<T>
    {
        Task<List<T>> GetAll(string BaseUrl);
        Task<T> GetById(int Id,string BaseUrl);
        Task<GeneralResponse> Insert(T item, string BaseUrl);
        Task<GeneralResponse> Update(T item, string BaseUrl);
        Task<GeneralResponse> DeleteById(int Id, string BaseUrl);
    }
}
