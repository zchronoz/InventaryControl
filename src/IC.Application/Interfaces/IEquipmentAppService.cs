using IC.Domain.Entities;

namespace IC.Application.Interfaces
{
    public interface IEquipmentAppService : IAppServiceBase<Equipment>
    {
        Equipment GetByCode(string code);

        decimal GetEstimatedValueActs(Equipment equipment, double depreciation);
    }
}