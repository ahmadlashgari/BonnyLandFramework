using BL.Test.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Test.ApplicationServices.Common.Interfaces
{
    public interface IBookService
    {
        Task<List<Book>> GetAsync();
        Task<Book> GetByIdAsync(string id);
        Task<Book> CreateAsync(Book entity);
    }
}
