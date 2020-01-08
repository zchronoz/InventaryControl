using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IC.MVC.ViewModels
{
    public class EquipmentViewModel
    {
        [Key]
        public int EquipmentId { get; set; }

        [Required(ErrorMessage = "Preencha o campo Tipo")]
        [MaxLength(100, ErrorMessage = "Máximo {0} caracteres")]
        [MinLength(2, ErrorMessage = "Minimo {0} caracteres")]
        [DisplayName("Tipo do equipamento")]
        public string TypeEquipment { get; set; }

        [Required(ErrorMessage = "Preencha o campo Modelo")]
        [MaxLength(100, ErrorMessage = "Máximo {0} caracteres")]
        [MinLength(2, ErrorMessage = "Minimo {0} caracteres")]
        [DisplayName("Modelo do equipamento")]
        public string ModelEquipment { get; set; }

        [Required(ErrorMessage = "Informe a data de aquisição")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Data de aquisição")]
        [DataType(DataType.Date)]
        public DateTime DateAcquisition { get; set; }

        [DataType(DataType.Currency)]
        [Range(typeof(decimal), "0", "999999999999,99")]
        [Required(ErrorMessage = "Preencha um valor")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [DisplayName("Valor de aquisição")]
        public decimal ValueAcquisition { get; set; }

        [DisplayName("Código do produto")]
        public string Code { get; set; }

        [DisplayName("Possui imagem?")]
        public string PathImage { get; set; }

        [JsonIgnore]
        [DisplayName("Imagem do produto")]
        public byte[] Image { get; set; }

        [JsonIgnore]
        public byte[] QRCode { get; set; }

        public DateTime DateRegister { get; set; }

        public PhotoViewModel Photo { get; set; }

        [DataType(DataType.Upload)]
        [JsonIgnore]
        [Display(Name = "Imagem do produto")]
        public virtual HttpPostedFileBase UploadPhoto { get; set; }

        [JsonIgnore]
        [Display(Name = "Remover Imagem?")]
        public bool RemoveImage { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Valor estimado")]
        public decimal EstimatedValueActs { get; set; }
    }
}