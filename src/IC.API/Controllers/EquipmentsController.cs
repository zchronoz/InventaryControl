using IC.Application.Interfaces;
using IC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace IC.API.Controllers
{
    public class EquipmentsController : ApiController
    {
        private readonly IEquipmentAppService _equipmentApp;

        public EquipmentsController(IEquipmentAppService equipmentApp)
        {
            _equipmentApp = equipmentApp;
        }

        // GET: api/Equipments
        /// <summary>
        /// Return Collection Equipment
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Equipment> GetEquipments()
        {
            try
            {
                return _equipmentApp.GetAll()?.OrderBy(c => c.EquipmentId);
            }
            catch
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Houve um erro em sua solicitação. Verifique os dados e tente novamente em breve.")
                };
                throw new System.Web.Http.HttpResponseException(resp);
            }
        }

        // GET: api/Equipments/5
        /// <summary>
        /// Return Equipment by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Equipment))]
        public IHttpActionResult GetEquipment(int id)
        {
            try
            {
                Equipment equipment = _equipmentApp.GetById(id);
                if (equipment == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(equipment.PathImage) && File.Exists(equipment.PathImage))
                {
                    equipment.Photo = new Photo { Name = equipment.PathImage };
                    using (FileStream file = new FileStream(equipment.PathImage, FileMode.Open, FileAccess.Read))
                    {
                        var bytes = new byte[file.Length];
                        file.Read(bytes, 0, (int)file.Length);
                        equipment.Photo.PhotoBytes = Convert.ToBase64String(bytes);
                    }
                }

                equipment.EstimatedValueActs = GetEstimatedValueActs(equipment);

                return Ok(equipment);
            }
            catch
            {
                return BadRequest("Houve um erro em sua solicitação. Verifique os dados e tente novamente em breve.");
            }
        }

        // GET: api/Equipments/code
        /// <summary>
        /// Return Equipment by Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [ResponseType(typeof(Equipment))]
        [Route("api/EquipmentsByCode/{code}")]
        public IHttpActionResult GetEquipment(string code)
        {
            try
            {
                Equipment equipment = _equipmentApp.GetByCode(code);
                if (equipment == null)
                {
                    return NotFound();
                }

                return GetEquipment(equipment.EquipmentId);
            }
            catch
            {
                return BadRequest("Houve um erro em sua solicitação. Verifique os dados e tente novamente em breve.");
            }
        }

        // PUT: api/Equipments/5
        /// <summary>
        /// Update Equipment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="equipment"></param>
        /// <returns></returns>
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEquipment(int id, Equipment equipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != equipment.EquipmentId)
            {
                return BadRequest("Valores diferem.");
            }

            try
            {
                _equipmentApp.Update(equipment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                return BadRequest("Houve um erro em sua solicitação. Verifique os dados e tente novamente em breve.");
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Equipments
        /// <summary>
        /// Create Equipment
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        [ResponseType(typeof(Equipment))]
        public IHttpActionResult PostEquipment(Equipment equipment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _equipmentApp.Add(equipment);

                return CreatedAtRoute("DefaultApi", new { id = equipment.EquipmentId }, equipment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Equipments/5
        /// <summary>
        /// Delete Equipment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ResponseType(typeof(Equipment))]
        public IHttpActionResult DeleteEquipment(int id)
        {
            try
            {
                Equipment equipment = _equipmentApp.GetById(id);
                if (equipment == null)
                {
                    return NotFound();
                }

                _equipmentApp.Remove(equipment);

                return Ok(equipment);
            }
            catch
            {
                return BadRequest("Houve um erro em sua solicitação. Verifique os dados e tente novamente em breve.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _equipmentApp.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks Equipment exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool EquipmentExists(int id)
        {
            try
            {
                return _equipmentApp.GetById(id) != null;
            }
            catch
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Houve um erro em sua solicitação. Verifique os dados e tente novamente em breve.")
                };
                throw new System.Web.Http.HttpResponseException(resp);
            }
        }

        /// <summary>
        /// Recupera o valor de depreciação
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns></returns>
        private decimal GetEstimatedValueActs(Equipment equipment)
        {
            try
            {
                return equipment.GetEstimatedValueActs(double.Parse(WebConfigurationManager.AppSettings["DepreciationValue"]));
            }
            catch
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Houve um erro em sua solicitação. Verifique os dados e tente novamente em breve.")
                };
                throw new System.Web.Http.HttpResponseException(resp);
            }
        }
    }
}
