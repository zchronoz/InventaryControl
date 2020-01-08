using IC.Domain.Entities;
using IC.Domain.Interfaces.Repositories;
using System.Linq;

namespace IC.Data.Repositories
{
    public class EquipmentRepository : RepositoryBase<Equipment>, IEquipmentRepository
    {
        public Equipment GetByCode(string code)
        {
            return ctx.Equipments.FirstOrDefault(c => c.Code.ToUpper() == code.ToUpper());
        }
    }
}