using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using VendorLibrary.Models;
using VendorLibrary.Services;

namespace VendorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        IVendorService vendorService;

        public VendorController(IVendorService vendorService)
        {
            this.vendorService = vendorService;
        }

        [HttpGet("GetAllVendorsAsync")]
        public async Task<IActionResult> GetAllVendorsAsync() =>
            Ok(await vendorService.GetAllVendors());


        [HttpPost("AddVendorAsync")]
        public async Task<IActionResult> AddVendorAsync(Vendor newVendor)
        {
            try
            {
                var vendor = await vendorService.AddVendorAsync(newVendor);
                return Ok(vendor);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("DeleteVendorAsync")]
        public async Task<IActionResult> DeleteVendorAsync(int vendorId)
        {
            try
            {
                var result = await vendorService.DeleteVendor(vendorId);

                if (result)
                    return Ok();
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("UpdateVendorAsync")]
        public async Task<IActionResult> UpdateVendorAsync(Vendor vendor)
        {
            try
            {
                return Ok(await vendorService.UpdateVendorAsync(vendor));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SendAllVendorsToERPAsync")]
        public async Task<IActionResult> SendAllVendorsToERPAsync()
        {
            await vendorService.SendAllVendorsAsync();
            return Ok();
        }
    }
}
