using StoreOnline.Domain.Entities;

namespace StoreOnline.Domain.Ports
{
    public interface IBuyRepository
    {
        Task<Buy> SaveAsync(Buy buy);        
    }
}
