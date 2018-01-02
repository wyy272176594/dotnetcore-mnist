using System.Linq;
using MnistWeb.DBModels;

namespace MnistWeb.Repository
{
    public interface INetRepository
    {
        void Add(Net net);
        void Delete(int id);
        IQueryable<Net> GetAll();
        Net GetSingle(int id);
        bool Save();
        void Update(Net net);
    }
}