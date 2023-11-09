using StoreOnline.Domain.Entities;
using StoreOnline.Domain.Ports;
using StoreOnline.Infrastructure.Ports;

namespace StoreOnline.Infrastructure.Adapters
{
    [Repository]
    public class CarRepository : ICarRepository
    {
        readonly IRepository<Car> _dataSource;
        public CarRepository(IRepository<Car> dataSource) => _dataSource = dataSource 
            ?? throw new ArgumentNullException(nameof(dataSource));

        public async Task<Car> SaveAsync(Car car) => await _dataSource.AddAsync(car!);

        public async Task<Car> GetAsync(Guid guid) => await _dataSource.GetOneAsync(guid);        

        public Task<Car> UpdateAsync(Car car) => Task.FromResult(_dataSource.Update(car!));

        public Task<IEnumerable<Car>> GetListAsync() => _dataSource.GetManyAsync();        
    }
}
