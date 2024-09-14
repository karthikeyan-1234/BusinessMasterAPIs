using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using PurchaseLibrary.DTOs;
using PurchaseLibrary.Models;
using PurchaseLibrary.Services;

using System.Text.Json;

namespace PurchaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        IPurchaseService purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            this.purchaseService = purchaseService;
        }

        [HttpGet("GetAllMaterialsAsync")]
        public async Task<IActionResult> GetAllMaterialsAsync()
            => Ok(await purchaseService.GetAllMaterialsAsync());

        //Add New Purchase
        [HttpPost("AddNewPurchaseAsync")]
        public async Task<IActionResult> AddNewPurchaseAsync(NewPurchaseDTO newPurchase)
        {
            try
            {
                return Ok(await purchaseService.AddNewPurchaseAsync(newPurchase));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //AddNewPurchaseWithDetail
        [HttpPost("AddNewPurchaseWithDetailAsync")]
        public async Task<IActionResult> AddNewPurchaseWithDetailAsync(NewPurchaseWithDetailDTO newPurchase)
        {
            try
            {
                return Ok(await purchaseService.AddNewPurchaseWithDetails(newPurchase));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllPurchasesAsync")]
        public async Task<IActionResult> GetAllPurchasesAsync()
        {
            try
            {
                return Ok(await purchaseService.GetAllPurchasesAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPurchaseItemsForPurchaseAsync")]
        public async Task<IActionResult> GetPurchaseItemsForPurchaseAsync(int PurchaseId)
        {
            try
            {
                var result = await purchaseService.GetPurchaseItemsForPurchase(PurchaseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePurchaseAsync")]
        public async Task<IActionResult> UpdatePurchaseAsync(Purchase updatePurchase)
        {
            try
            {
                return Ok(await purchaseService.UpdatePurchaseAsync(updatePurchase));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePurchaseAsync")]
        public async Task<IActionResult> DeletePurchaseAsync(int purchaseId)
        {
            try
            {
                return Ok(await purchaseService.RemovePurchaseAsync(purchaseId));
            }
            catch (Exception)
            {

                return BadRequest("Unable to delete..!!");
            }
        }

        [HttpPut("UpdatePurchaseItemAsync")]
        public async Task<IActionResult> UpdatePurchaseItemAsync(PurchaseItem purchaseItem)
        {
            try
            {
                await purchaseService.UpdatePurchaseItemAsync(purchaseItem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeletePurchaseItemAsync")]
        public async Task<IActionResult> DeletePurchaseItemAsync(int purchaseItemId)
        {
            try
            {
                await purchaseService.RemovePurchaseItemAsync(purchaseItemId);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddNewPurchaseItemAsync")]
        public async Task<IActionResult> AddNewPurchaseItemAsync(NewPurchaseItemDTO newPurchaseItem)
        {
            try
            {
                return Ok(await purchaseService.AddNewPurchaseItemAsync(newPurchaseItem));
            }
            catch(Exception ex)
            {
                return BadRequest("Unable to add purchase item");
            }
        }
    }
}
