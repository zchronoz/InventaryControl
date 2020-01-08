using IC.Domain.Entities;
using IC.Domain.Interfaces.Repositories;
using IC.Domain.Interfaces.Services;

namespace IC.Domain.Services
{
    public class EquipmentService : ServiceBase<Equipment>, IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentService(IEquipmentRepository equipmentRepository)
            : base(equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public Equipment GetByCode(string code)
        {
            return _equipmentRepository.GetByCode(code);
        }

        public decimal GetEstimatedValueActs(Equipment equipment, double depreciation)
        {
            return equipment.GetEstimatedValueActs(depreciation);
        }
    }
}