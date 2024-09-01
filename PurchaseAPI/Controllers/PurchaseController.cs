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
                return Ok(await purchaseService.GetPurchaseItemsForPurchase(PurchaseId));
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
    }
}
