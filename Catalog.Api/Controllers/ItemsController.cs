using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Catalog.Api.Repositories;
using Catalog.Api.Entities;
using Catalog.Api.Dtos;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;

        public ItemsController(IItemsRepository repository)
        {
            _repository = repository;
        }

        // GET /api/items/
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string name = null)
        {
            var items = (await _repository.GetItemsAsync())
                .Select(item => item.AsDto());

            // filter items
            if (!string.IsNullOrWhiteSpace(name))
            {
                items = items.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            }

            return items;
        }

        // GET /api/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _repository.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        // POST /api/items/
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
        }

        // PUT /api/items/{id}
        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Price = updateItemDto.Price;

            // can use with-expression on record types :-) 
            // Item updatedItem = existingItem with
            // {
            //     Name = updateItemDto.Name,
            //     Price = updateItemDto.Price
            // };

            await _repository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        // DELETE /api/items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await _repository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await _repository.DeleteItemAsync(id);

            // var allItems = _repository.GetItems().Select(item => item.AsDto());

            // return Ok(allItems);
            return NoContent();
        }
    }
}
