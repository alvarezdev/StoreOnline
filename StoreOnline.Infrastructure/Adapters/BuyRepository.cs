using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Ports;
using StoreOnline.Infrastructure.Ports;

namespace StoreOnline.Infrastructure.Adapters
{
    [Repository]
    public class BuyRepository : IBuyRepository
    {
        readonly IRepository<Buy> _dataSource;
        public BuyRepository(IRepository<Buy> dataSource) => _dataSource = dataSource 
            ?? throw new ArgumentNullException(nameof(dataSource));

        public async Task<Buy> SaveAsync(Buy buy) => await _dataSource.AddAsync(buy!);
      
    }
}
