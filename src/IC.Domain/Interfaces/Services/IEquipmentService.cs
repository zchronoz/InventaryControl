using IC.Domain.Entities;

namespace IC.Domain.Interfaces.Services
{
    public interface IEquipmentService : IServiceBase<Equipment>
    {
        Equipment GetByCode(string code);

        decimal GetEstimatedValueActs(Equipment equipment, double depreciation);
    }
}