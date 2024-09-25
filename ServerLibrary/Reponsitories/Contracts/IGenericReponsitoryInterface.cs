using BaseLibrary.Reponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary.Reponsitories.Contracts
{
    public interface IGenericReponsitoryInterface<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int Id);
        Task<GeneralResponse> Insert(T item);
        Task<GeneralResponse> Update(T item);
        Task<GeneralResponse> DeleteById(int Id);

    }
}
