using IC.Application.Interfaces;
using IC.CrossCutting;
using IC.Domain.Entities;
using IC.Domain.Interfaces.Services;
using System;

namespace IC.Application
{
    public class EquipmentAppService : AppServiceBase<Equipment>, IEquipmentAppService
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentAppService(IEquipmentService equipmentService) : base(equipmentService)
        {
            _equipmentService = equipmentService;
        }

        public override void Add(Equipment obj)
        {
            obj.Code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

            while (_equipmentService.GetByCode(obj.Code) != null)
                obj.Code = Guid.NewGuid().ToString().Substring(0, 8);

            if (obj.Photo != null)
                obj.PathImage = FileService.CreateFile(obj.Code, obj.Photo);

            base.Add(obj);
        }

        public override void Remove(Equipment obj)
        {
            FileService.DeleteFile(obj.PathImage);
            base.Remove(obj);
        }

        public override void Update(Equipment obj)
        {
            if (obj.Photo != null)
                obj.PathImage = FileService.UpdateFile(obj.PathImage, obj.Code, obj.Photo);

            base.Update(obj);
        }

        public Equipment GetByCode(string code)
        {
            return _equipmentService.GetByCode(code);
        }

        public decimal GetEstimatedValueActs(Equipment equipment, double depreciation)
        {
            return _equipmentService.GetEstimatedValueActs(equipment, depreciation);
        }
    }
}