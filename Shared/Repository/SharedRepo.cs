using Shared.Infrastrcuture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repository
{
    public class SharedRepo
    {
        private SharedDbContext _context;

        public SharedDbContext Context { get { return _context; } }
        public async Task ProjectIndividuals()
        {
            
        }
    }
}
