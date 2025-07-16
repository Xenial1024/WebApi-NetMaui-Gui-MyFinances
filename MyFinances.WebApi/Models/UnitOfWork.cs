using MyFinances.WebApi.Models.Domains;
using MyFinances.WebApi.Models.Repositories;

namespace MyFinances.WebApi.Models
{
    public class UnitOfWork(MyFinancesContext context)
    {
        public OperationRepository Operation => new(context);
        public void Complete() => context.SaveChanges();
    }
}
