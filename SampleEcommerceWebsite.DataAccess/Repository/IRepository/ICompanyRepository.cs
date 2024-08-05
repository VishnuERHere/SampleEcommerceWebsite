using SampleEcommerceWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Adding New Model step 4 - creating IRepository
namespace SampleEcommerceWebsite.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
        
    }
}
