using MaterialLibrary.DTOs;
using MaterialLibrary.Models;
using MaterialLibrary.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaterialsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        IMaterialService materialService;

        public MaterialController(IMaterialService materialService)
        {
            this.materialService = materialService;
        }

        #region Materials

        [HttpPost("AddNewMaterialAsync")]
        public async Task<IActionResult> AddNewMaterialAsync(NewMaterialDTO newMaterial)
        {
            return Ok(await materialService.AddNewMaterialAsync(newMaterial));
        }

        [HttpPut("UpdateMaterialAsync")]
        public async Task<IActionResult> UpdateMaterialAsync(Material updateMaterial)
        {
            if (await materialService.UpdateMaterialAsync(updateMaterial))
                return Ok();

            return BadRequest();
        }

        [HttpGet("GetAllMaterialsAsync")]   //Get all materials
        public async Task<IActionResult> GetAllMaterialsAsync() => Ok(await materialService.GetallMaterialsAsync());

        [HttpDelete("DeleteMaterialAsync")]
        public async Task<IActionResult> DeleteMaterialAsync(int id)
        {
            if (await materialService.DeleteMaterialAsync(id))
                return Ok();

            return NotFound();
        }

        [HttpPost("SendAllMaterialsToERPAsync")]
        public async Task<IActionResult> SendAllMaterialsToERPAsync()
        {
            await materialService.SendAllMaterialsAsync();
            return Ok("All Material Types Sent..!!");
        }

        #endregion

        #region Material Types

        [HttpPost("AddNewMaterialTypeAsync")]
        public async Task<IActionResult> AddNewMaterialTypeAsync(NewMaterialTypeDTO  newMaterial)
        {
            return Ok(await materialService.AddNewMaterialTypeAsync(newMaterial));
        }

        [HttpPut("UpdateMaterialTypeAsync")]
        public async Task<IActionResult> UpdateMaterialTypeAsync(MaterialType updateMaterialType)
        {
            if (await materialService.UpdateMaterialTypeAsync(updateMaterialType))
                return Ok();

            return BadRequest();
        }

        [HttpGet("GetAllMaterialTypesAsync")]
        public async Task<IActionResult> GetAllMaterialTypesAsync() => Ok(await materialService.GetMaterialTypesAsync());

        [HttpDelete("DeleteMaterialTypeAsync")]
        public async Task<IActionResult> DeleteMaterialTypeAsync(int id)
        {
            try
            {
                if (await materialService.DeleteMaterialTypeAsync(id))
                    return Ok();

                return BadRequest("Unable to delete material.!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SendAllMaterialTypesToERPAsync")]
        public async Task<IActionResult> SendAllMaterialTypesToERPAsync()
        {
            await materialService.SendAllMaterialTypesAsync();
            return Ok("All Material Types Sent..!!");
        }

        #endregion
    }
}
