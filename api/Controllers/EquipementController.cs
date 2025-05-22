 using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using System.Threading.Tasks;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipementController : ControllerBase
    {
        private readonly IEquipementService _equipementService;

        public EquipementController(IEquipementService equipementService)
        {
            _equipementService = equipementService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipementDTO>>> GetAll([FromQuery] EquipementFilterDTO? filter = null)
        {
            try
            {
                var equipements = await _equipementService.GetAllAsync(filter);
                return Ok(equipements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipementDTO>> GetById(int id)
        {
            var equipement = await _equipementService.GetByIdAsync(id);
            if (equipement == null) return NotFound();
            return Ok(equipement);
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<EquipementDTO>> GetByCode(string code)
        {
            var equipement = await _equipementService.GetByCodeAsync(code);
            if (equipement == null) return NotFound();
            return Ok(equipement);
        }

        [HttpPost]
        public async Task<ActionResult<EquipementDTO>> Create(CreateEquipementDTO dto)
        {
            try
            {
                var equipement = await _equipementService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = equipement.idEqpt }, equipement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(int id, UpdateEquipementDTO dto)
        {
            try
            {
                // First get the current equipment data
                var currentEquipement = await _equipementService.GetByIdAsync(id);
                if (currentEquipement == null)
                    return NotFound(new { 
                        success = false,
                        message = $"Équipement avec l'ID {id} non trouvé" 
                    });

                // Check if this is an initial request (all fields are default/empty)
                bool isInitialRequest = 
                    dto.idType == 0 && 
                    dto.idCat == 0 && 
                    dto.idMarq == 0 && 
                    (string.IsNullOrEmpty(dto.design) || dto.design == "string") && 
                    (!dto.idGrpIdq.HasValue || dto.idGrpIdq == 0) && 
                    (string.IsNullOrEmpty(dto.etat) || dto.etat == "string") && 
                    (string.IsNullOrEmpty(dto.numserie) || dto.numserie == "string") && 
                    (string.IsNullOrEmpty(dto.position_physique) || dto.position_physique == "string") && 
                    (!dto.DateMiseService.HasValue || dto.DateMiseService == DateTime.MinValue) && 
                    (!dto.AnnéeFabrication.HasValue || dto.AnnéeFabrication == 0) && 
                    (!dto.DateAcquisition.HasValue || dto.DateAcquisition == DateTime.MinValue) && 
                    (!dto.ValeurAcquisition.HasValue || dto.ValeurAcquisition == 0) && 
                    (!dto.idunite.HasValue || dto.idunite == 0);

                if (isInitialRequest)
                {
                    return Ok(new { 
                        success = true,
                        message = "Voici les informations actuelles de l'équipement. Modifiez uniquement les champs que vous souhaitez changer.",
                        data = new {
                            current = currentEquipement,
                            updateTemplate = new UpdateEquipementDTO
                            {
                                idType = currentEquipement.idType,
                                idCat = currentEquipement.idCat,
                                idMarq = currentEquipement.idMarq,
                                design = currentEquipement.design,
                                idGrpIdq = currentEquipement.idGrpIdq,
                                etat = currentEquipement.etat,
                                numserie = currentEquipement.numserie,
                                position_physique = currentEquipement.position_physique,
                                DateMiseService = currentEquipement.DateMiseService,
                                AnnéeFabrication = currentEquipement.AnnéeFabrication,
                                DateAcquisition = currentEquipement.DateAcquisition,
                                ValeurAcquisition = currentEquipement.ValeurAcquisition,
                                idunite = currentEquipement.idunite
                            }
                        }
                    });
                }

                // Create a new DTO with current values
                var updatedDto = new UpdateEquipementDTO
                {
                    idType = dto.idType > 0 ? dto.idType : currentEquipement.idType,
                    idCat = dto.idCat > 0 ? dto.idCat : currentEquipement.idCat,
                    idMarq = dto.idMarq > 0 ? dto.idMarq : currentEquipement.idMarq,
                    design = !string.IsNullOrEmpty(dto.design) && dto.design != "string" ? dto.design : currentEquipement.design,
                    observation = dto.observation ?? currentEquipement.observation ?? string.Empty,
                    idGrpIdq = dto.idGrpIdq.HasValue && dto.idGrpIdq > 0 ? dto.idGrpIdq : currentEquipement.idGrpIdq,
                    etat = !string.IsNullOrEmpty(dto.etat) && dto.etat != "string" ? dto.etat : currentEquipement.etat,
                    numserie = !string.IsNullOrEmpty(dto.numserie) && dto.numserie != "string" ? dto.numserie : currentEquipement.numserie,
                    position_physique = !string.IsNullOrEmpty(dto.position_physique) && dto.position_physique != "string" ? dto.position_physique : currentEquipement.position_physique,
                    DateMiseService = dto.DateMiseService.HasValue && dto.DateMiseService != DateTime.MinValue ? dto.DateMiseService : currentEquipement.DateMiseService,
                    AnnéeFabrication = dto.AnnéeFabrication.HasValue && dto.AnnéeFabrication > 0 ? dto.AnnéeFabrication : currentEquipement.AnnéeFabrication,
                    DateAcquisition = dto.DateAcquisition.HasValue && dto.DateAcquisition != DateTime.MinValue ? dto.DateAcquisition : currentEquipement.DateAcquisition,
                    ValeurAcquisition = dto.ValeurAcquisition.HasValue && dto.ValeurAcquisition > 0 ? dto.ValeurAcquisition : currentEquipement.ValeurAcquisition,
                    idunite = dto.idunite.HasValue && dto.idunite > 0 ? dto.idunite : currentEquipement.idunite
                };

                var equipement = await _equipementService.UpdateAsync(id, updatedDto);
                if (equipement == null) return NotFound(new { success = false, message = "Équipement non trouvé" });
                
                return Ok(new { 
                    success = true,
                    message = "Équipement mis à jour avec succès",
                    data = equipement
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _equipementService.DeleteAsync(id);
            if (!result) return NotFound(new { success = false, message = "Équipement non trouvé" });
            return Ok(new { success = true, message = "Équipement supprimé avec succès" });
        }
          [HttpGet("non-reformes")]
public async Task<ActionResult<IEnumerable<EquipementDTO>>> GetEquipementsNonReformes()
{
    try
    {
        var equipements = await _equipementService.GetNonReformedAsync();
        return Ok(equipements);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Erreur serveur: {ex.Message}");
    }
}
    }
} 