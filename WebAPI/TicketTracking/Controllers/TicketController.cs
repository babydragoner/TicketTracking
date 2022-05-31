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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketItem>>> GetTicketItems()
        {
            return await _context.TicketItems.ToListAsync();
        }

        /// <summary>
        /// Deletes a specific TicketItem.
        /// </summary>
        /// <param name="id">ticket id</param>
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

        /// <summary>
        /// Update a TicketItem.
        /// </summary>
        /// <param name="id">a TicketDTO id</param>
        [HttpPut("{id}")]
        [PermissionAttribute(ActionType.Update)]
        public async Task<ActionResult<TicketItem>> UpdateTicketItem(Guid id, TicketDTO item)
        {
            var oItem = await _context.TicketItems.FindAsync(id);
            if (oItem == null)
            {
                return NotFound();
            }

            oItem.Title = item.Title;
            oItem.Summary = item.Summary;
            oItem.Description = item.Description;
            oItem.TicketType = item.TicketType;
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

            return oItem;
        }
        /// <summary>
        /// Complete a TicketItem.
        /// </summary>
        /// <param name="id">a TicketDTO id</param>
        [HttpPut("complete/{id}")]
        [PermissionAttribute("complete")]
        public async Task<ActionResult<TicketItem>> UpdateStatus(Guid id)
        {
            var oItem = await _context.TicketItems.FindAsync(id);
            if (oItem == null)
            {
                return NotFound();
            }

            oItem.Status = TicketStatus.Finish;
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

            return oItem;
        }
        /// <summary>
        /// Creates a TicketItem.
        /// </summary>
        /// <param name="item">a TicketDTO item</param>
        /// <returns>A newly created TicketItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Ticket
        ///     {
        ///         "title": "string",
        ///         "summary": "string",
        ///         "description": "string",
        ///         "ticketType": 1,
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        [PermissionAttribute(ActionType.Create)]
        [HttpPost]
        public async Task<ActionResult<TicketItem>> CreateTicketItem(TicketDTO item)
        {
            this.HttpContext.Items["ResourceName"] = "";
            var oItem = new TicketItem();
            oItem.SetTicketItem(item);
            oItem.Id = Guid.NewGuid();
            oItem.Status = TicketStatus.Open;
            oItem.CreUser = "sys";
            oItem.CreDate = DateTime.UtcNow;
            _context.TicketItems.Add(oItem);
            await _context.SaveChangesAsync();

            return oItem;
        }

        [PermissionAttribute(ActionType.Delete)]
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

    }
}
