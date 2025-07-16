using MyFinances.Models.Domains;
using SQLite;

namespace MyFinances.Models
{
    public class UnitOfWork
    {
        readonly SQLiteAsyncConnection _context;
        public OperationRepository Operation { get; set; }

        public UnitOfWork(string dbPath)
        {
            _context = new SQLiteAsyncConnection(dbPath);
            _context.CreateTableAsync<Operations>().Wait();
            Operation = new OperationRepository(_context);
        }
    }
}
