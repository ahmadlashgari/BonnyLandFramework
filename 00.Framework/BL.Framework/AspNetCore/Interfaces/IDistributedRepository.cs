using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Framework.AspNetCore.Interfaces
{
	public interface IDistributedRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync();
		Task<T> GetByKeyAsync(string key);
		Task<T> UpdateAsync(string key, T entity);
		Task DeleteAsync(string key);
	}
}
