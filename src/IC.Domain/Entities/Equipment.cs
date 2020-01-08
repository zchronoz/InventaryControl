using System;

namespace IC.Domain.Entities
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string TypeEquipment { get; set; }
        public string ModelEquipment { get; set; }
        public DateTime DateAcquisition { get; set; }
        public decimal ValueAcquisition { get; set; }
        public string Code { get; set; }
        public string PathImage { get; set; }
        public DateTime DateRegister { get; set; }

        public virtual Photo Photo { get; set; }

        public virtual decimal EstimatedValueActs { get; set; }

        public decimal GetEstimatedValueActs(double depreciation)
        {
            int monthsAcquisition = this.DateAcquisition.Month + (this.DateAcquisition.Year * 12);
            int monthsActs = DateTime.Now.Month + (DateTime.Now.Year * 12);

            var months = monthsActs - monthsAcquisition;
            var depreciationPercentage = Convert.ToDecimal((months * depreciation) / 100);

            return this.ValueAcquisition - (this.ValueAcquisition * depreciationPercentage);
        }
    }
}