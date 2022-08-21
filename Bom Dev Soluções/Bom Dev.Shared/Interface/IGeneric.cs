using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interface
{
    public interface IGeneric<T> where T: class
    {
        public Task<int> Insert(T obj);
        public Task Update(T obj);
        public Task Delete(int id);
        public Task<T> GetById(int id);
        public Task<List<T>> Get();
    }
}
