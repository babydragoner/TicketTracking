﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketTracking.Models;
using TicketTracking.Utility;

namespace TicketTracking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly DBContext _context;
        private CacheHelper _cache;

        public AuthController(CacheHelper cache, ILogger<AuthController> logger, DBContext context)
        {
            _cache = cache;
            _logger = logger;
            _context = context;
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
        public async Task<object> Login(LoginDTO data)
        {
            var oItem = new TicketItem();
            oItem.Token = Guid.NewGuid().ToString();
            _cache.SetStrCache("sys", oItem.Id.ToString());
            var res = new { userId = data.username, token = "fakeToken1", role = "" };
            return new
            {
                code = 0,
                result= res,
                message = "",
                type = "success",
            };
            //return CreatedAtAction(
            //    nameof(GetTicketItem),
            //    new { id = item.Id },
            //    item);
        }
    }
}
