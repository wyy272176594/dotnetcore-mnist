using MnistWeb.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MnistWeb.Repository
{
    public class NetRepository : INetRepository
    {
        private mnistContext _context;
        public NetRepository(mnistContext context)
        {
            _context = context;
        }

        public IQueryable<Net> GetAll()
        {
            return _context.Net;
        }

        public Net GetSingle(int id)
        {
            return _context.Net.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Net net)
        {
            _context.Net.Add(net);
        }

        public void Delete(int id)
        {
            Net net = GetSingle(id);
            _context.Net.Remove(net);
        }

        public void Update(Net net)
        {
            _context.Net.Update(net);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

    }
}
