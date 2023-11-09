using StoreOnline.Domain.Entities;

namespace StoreOnline.Domain.Ports
{
    public interface ICarRepository
    {
        Task<Car> SaveAsync(Car car);
        Task<Car> GetAsync(Guid guid);
        Task<Car> UpdateAsync
        (Car product);
        Task<IEnumerable<Car>> GetListAsync();        
    }
}
