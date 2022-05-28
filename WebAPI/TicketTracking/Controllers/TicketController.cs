using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Portal.Filter;
using Shared.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketTracking.Models;

namespace TicketTracking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TicketController : ControllerBase
    {

        private readonly ILogger<TicketController> _logger;
        private readonly DBContext _context;

        public TicketController(ILogger<TicketController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// Get Operation
        /// </summary>
        /// <remarks>
        /// Sample value of message
        /// 
        ///     Hi Sukhpinder
        ///     
        /// </remarks>
        /// <param name="message"></param>
        /// <returns></returns>

        [PermissionAttribute(ActionType.Search)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketItem>>> GetTicketItems()
        {
            return await _context.TicketItems.ToListAsync();
        }

        /// <summary>
        /// Deletes a specific TicketItem.
        /// </summary>
        /// <param name="id">todo id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [SwaggerResponse(200, "銷售活動異動紀錄", typeof(TicketItem))]
        public async Task<ActionResult<TicketItem>> GetTicketItem(Guid id)
        {
            var item = await _context.TicketItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicketItem(Guid id, TicketDTO item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            var oItem = await _context.TicketItems.FindAsync(id);
            if (oItem == null)
            {
                return NotFound();
            }

            oItem.Id = item.Id;
            oItem.Title = item.Title;
            oItem.Summary = item.Summary;
            oItem.Description = item.Description;
            oItem.TicketType = item.TicketType;
            oItem.Status = item.Status;
            oItem.UpdUser = "sys";
            oItem.UpdDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TicketItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        /// <summary>
        /// Creates a TicketItem.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A newly created TicketItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Ticket
        ///     {
        ///        "id": 1,
        ///        "name": "Item #1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        public async Task<ActionResult<TicketItem>> CreateTicketItem(TicketDTO item)
        {
            var oItem = new TicketItem();
            oItem.SetTicketItem(item);
            oItem.Id = Guid.NewGuid();
            oItem.CreUser = "sys";
            oItem.CreDate = DateTime.UtcNow;
            _context.TicketItems.Add(oItem);
            await _context.SaveChangesAsync();

            return oItem;
            //return CreatedAtAction(
            //    nameof(GetTicketItem),
            //    new { id = item.Id },
            //    item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketItem(Guid id)
        {
            var item = await _context.TicketItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.TicketItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketItemExists(Guid id) =>
             _context.TicketItems.Any(e => e.Id == id);

        //private static TicketItem ItemToDTO(TicketItem item) =>
        //    new TicketItem
        //    {
        //        Id = item.Id,
        //        Title = item.Title,
        //        Summary = item.Summary,
        //        Description = item.Description,
        //        TicketType = item.TicketType,
        //        Summary = item.Summary,
        //        Summary = item.Summary,
        //        Summary = item.Summary,
        //        Summary = item.Summary,
        //        Summary = item.Summary,
        //    };
    }
}
