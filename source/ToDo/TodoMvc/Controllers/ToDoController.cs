using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoMvc.Services;
using TodoMvc.Models.View;
using TodoMvc.Models;

namespace TodoMvc.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ITodoItemService _todoItemsService;

        public ToDoController(ITodoItemService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }

        public async Task<IActionResult> Index()
        {
            // Acessar os dados
            var todoItems = await _todoItemsService
                .GetIncompleteItemsAsync();
            // Montar uma Model
            var viewModel = new ToDoViewModel
            {
                Items = todoItems
            };
            // Retornar View
            return View(viewModel);
        }

        public async Task<IActionResult> AddItem(NewToDoItem newToDoItem)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var successful = await _todoItemsService
                .AddItemAsync(newToDoItem);

            if (!successful)
                return BadRequest(new { Error = "Could not add Item"});

            return Ok();
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var successful = await _todoItemsService
                .MarkDoneAsync(id);

            return Ok();
        }
    }
}