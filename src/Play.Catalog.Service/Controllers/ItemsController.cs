using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{

    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        // private static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        // };

        private readonly ItemsRepository itemsRepository = new();

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync())
                .Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = (await itemsRepository.GetAsync(id));

            if (item == null)
            {
                return NotFound();
            }

            return item.AsDto();
            // var item = items.Where(item => item.Id == id).SingleOrDefault();
            // if (item == null)
            // {
            //     return NotFound();
            // }

            // return item;
        }

        //POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);

            // var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            // items.Add(item);

            // return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        //Put /items/{id}   public ActionResult<ItemDto> Put(Guid id, UpdateItemDto updateItemDto)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();

            // var existingItem = items.Where(item => item.Id == id).SingleOrDefault();

            // if (existingItem == null)
            // {
            //     return NotFound();
            // }

            // var updatedItem = existingItem with
            // {
            //     Name = updateItemDto.Name,
            //     Description = updateItemDto.Description,
            //     Price = updateItemDto.Price
            // };

            // var index = items.FindIndex(existingItem => existingItem.Id == id);
            // items[index] = updatedItem;

            // return NoContent();
        }

        //Delete /items/{id} 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var item = await itemsRepository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(item.Id);

            return NoContent();

            // var index = items.FindIndex(existingItem => existingItem.Id == id);
            // if (index < 0)
            //     return NotFound();

            // items.RemoveAt(index);

            // return NoContent();
        }
    }
}