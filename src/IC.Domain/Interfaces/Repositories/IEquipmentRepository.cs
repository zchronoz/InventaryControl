using IC.Domain.Entities;

namespace IC.Domain.Interfaces.Repositories
{
    public interface IEquipmentRepository : IRepositoryBase<Equipment>
    {
        Equipment GetByCode(string code);
    }
}