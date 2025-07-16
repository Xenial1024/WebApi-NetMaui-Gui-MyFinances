using MyFinances.Models.Domains;
using SQLite;

namespace MyFinances.Models
{
    public class OperationRepository(SQLiteAsyncConnection context)
    {
        private readonly SQLiteAsyncConnection _context = context;

        public async Task<int> AddAsync(Operations operation)
            => await _context.InsertAsync(operation);
        
        public async Task UpdateAsync(Operations operation)
            => await _context.UpdateAsync(operation);

        public async Task DeleteAsync(Operations operation)
            => await _context.DeleteAsync(operation);

        public async Task<IEnumerable<Operations>> GetAsync()
            => await _context.Table<Operations>().ToListAsync();
        
        public async Task<Operations> GetAsync(int id)
            => await _context.Table<Operations>()
                .FirstAsync(x => x.Id == id);

        public async Task<IEnumerable<Operations>> GetAsync(byte operationCount, int pageNumber)
            => await _context.Table<Operations>()
            .Skip((pageNumber - 1) * operationCount)
            .Take(operationCount)
            .ToListAsync();
        public async Task<int> GetTotalCountAsync()
            => await _context.Table<Operations>().CountAsync();
    }
}
