using IC.CrossCutting;
using IC.MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IC.MVC.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly static HttpClient client = new HttpClient();
        private readonly string ServerRoute = WebConfigurationManager.AppSettings["ServerRoute"];

        #region GET

        public async Task<ActionResult> Index()
        {
            try
            {
                List<EquipmentViewModel> equipments = null;
                HttpResponseMessage response = await client.GetAsync(ServerRoute + "api/Equipments");
                if (response.IsSuccessStatusCode)
                {
                    string resposta = await response.Content.ReadAsStringAsync();
                    equipments = new JavaScriptSerializer().Deserialize<List<EquipmentViewModel>>(resposta);
                }
                return View(equipments);
            }
            catch
            {
                return View("Error");
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            EquipmentViewModel equipment;
            try
            {
                HttpResponseMessage response = await client.GetAsync(ServerRoute + "api/Equipments/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string resposta = await response.Content.ReadAsStringAsync();
                    equipment = new JavaScriptSerializer().Deserialize<EquipmentViewModel>(resposta);
                    if (equipment.Photo != null)
                        equipment.Image = Convert.FromBase64String(equipment.Photo.PhotoBytes);

                    return View(equipment);
                }
                return View("_NotFound");
            }
            catch
            {
                return View("Error");
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                EquipmentViewModel equipment;
                HttpResponseMessage response = await client.GetAsync(ServerRoute + "api/Equipments/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string resposta = await response.Content.ReadAsStringAsync();
                    equipment = new JavaScriptSerializer().Deserialize<EquipmentViewModel>(resposta);

                    string base64 = string.Empty;

                    if (equipment.Photo != null)
                    {
                        base64 = equipment.Photo.PhotoBytes;
                        equipment.Photo.PhotoBytes = null;
                    }
                    string json = new JavaScriptSerializer().Serialize(equipment);
                    equipment.QRCode = QRCodeService.GetQRCode(json);

                    if (!string.IsNullOrEmpty(base64))
                    {
                        equipment.Photo.PhotoBytes = base64;
                        equipment.Image = Convert.FromBase64String(equipment.Photo.PhotoBytes);
                    }

                    return View(equipment);
                }
                return View("_NotFound");
            }
            catch
            {
                return View("Error");
            }
        }

        public async Task<ActionResult> DetailsByCode(string id)
        {
            EquipmentViewModel equipment = null;
            try
            {
                HttpResponseMessage response = await client.GetAsync(ServerRoute + "api/EquipmentsByCode/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string resposta = await response.Content.ReadAsStringAsync();
                    equipment = new JavaScriptSerializer().Deserialize<EquipmentViewModel>(resposta);

                    return RedirectToAction("Details", new { id = equipment.EquipmentId });
                }
                return View("_NotFound");
            }
            catch
            {
                return View("Error");
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            EquipmentViewModel equipment;
            try
            {
                HttpResponseMessage response = await client.GetAsync(ServerRoute + "api/Equipments/" + id);
                if (response.IsSuccessStatusCode)
                {
                    string resposta = await response.Content.ReadAsStringAsync();
                    equipment = new JavaScriptSerializer().Deserialize<EquipmentViewModel>(resposta);
                    if (equipment.Photo != null)
                        equipment.Image = Convert.FromBase64String(equipment.Photo.PhotoBytes);

                    return View(equipment);
                }
                return View("_NotFound");
            }
            catch
            {
                return View("Error");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        #endregion GET

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EquipmentViewModel equipmentViewModel)
        {
            try
            {
                var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };

                if (equipmentViewModel.UploadPhoto != null
                    && equipmentViewModel.UploadPhoto.ContentLength > 0)
                {
                    if (!imageTypes.Contains(equipmentViewModel.UploadPhoto.ContentType))
                    {
                        ModelState.AddModelError("ImageUpload", "Escolha uma iamgem GIF, JPG ou PNG.");
                        return View(equipmentViewModel);
                    }
                    equipmentViewModel.Photo = GetPhoto(equipmentViewModel.UploadPhoto);
                }

                equipmentViewModel.UploadPhoto = null;

                string jsonObject = new JavaScriptSerializer().Serialize(equipmentViewModel);
                var contentString = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync(ServerRoute + "api/Equipments/", contentString);

                string resposta = response.Content.ReadAsStringAsync().Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var newEquipment = new JavaScriptSerializer().Deserialize<EquipmentViewModel>(resposta);

                    if (newEquipment.Photo != null)
                        newEquipment.Photo.PhotoBytes = null;

                    string json = new JavaScriptSerializer().Serialize(newEquipment);
                    EmailService.SendEmail("desafiotecnicosamsung2019@gmail.com", $"Produto {newEquipment.TypeEquipment} Cadastrado", newEquipment.Code, "Especificações do produto seguem no qrcode anexo.", QRCodeService.GetBitmapQRCode(json));

                    return RedirectToAction("Details", new { id = newEquipment.EquipmentId });
                }
                else
                {
                    var jsonError = Newtonsoft.Json.Linq.JObject.Parse(resposta);
                    string mensagemError = jsonError.GetValue("Message").ToString();
                    if (mensagemError.ToUpper().Contains("IIS")) mensagemError = "Houve um erro de permissão ao armazenar o arquivo. Contate o desenvolvedor."; // Executar como Administrador
                    throw new Exception(mensagemError);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(equipmentViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EquipmentViewModel equipmentViewModel)
        {
            try
            {
                var imageTypes = new string[]{
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };

                if (equipmentViewModel.UploadPhoto != null
                    && equipmentViewModel.UploadPhoto.ContentLength > 0)
                {
                    if (!imageTypes.Contains(equipmentViewModel.UploadPhoto.ContentType))
                    {
                        ModelState.AddModelError("ImageUpload", "Escolha uma iamgem GIF, JPG ou PNG.");
                        return View(equipmentViewModel);
                    }
                    equipmentViewModel.Photo = GetPhoto(equipmentViewModel.UploadPhoto);
                }
                else if (equipmentViewModel.RemoveImage)
                {
                    equipmentViewModel.Photo = new PhotoViewModel();
                }

                equipmentViewModel.UploadPhoto = null;

                string jsonObject = new JavaScriptSerializer().Serialize(equipmentViewModel);
                var contentString = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                contentString.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PutAsync(ServerRoute + "api/Equipments/" + equipmentViewModel.EquipmentId, contentString);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(equipmentViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(EquipmentViewModel equipmentViewModel)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(ServerRoute + "api/Equipments/" + equipmentViewModel.EquipmentId);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                return View("Error");
            }
            catch
            {
                return View("Error");
            }
        }

        private PhotoViewModel GetPhoto(HttpPostedFileBase file)
        {
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                return new PhotoViewModel()
                {
                    Name = file.FileName,
                    PhotoBytes = Convert.ToBase64String(binaryReader.ReadBytes(file.ContentLength))
                };
            }
        }
    }
}
