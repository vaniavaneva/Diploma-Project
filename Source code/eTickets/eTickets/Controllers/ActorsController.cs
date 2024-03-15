using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	public class ActorsController : Controller
	{
		private readonly IActorsService _service;

		public ActorsController(IActorsService service)
		{
			_service = service;
		}

		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var data = await _service.GetAllAsync();
			return View(data);
		}

		//Get: Actors/Create
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("FullName,ProfilePictureURL,Bio")] Actor actor)
		{
			if (!ModelState.IsValid)
			{
				return View(actor);
			}
			await _service.AddAsync(actor);
			return RedirectToAction(nameof(Index));
		}

		//Get: Actors/Details/1
		[AllowAnonymous]
		public async Task<IActionResult> Details(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);

			if (actorDetails == null) return View("NotFound");
			return View(actorDetails);
		}

		//Get: Actors/Edit/1
		public async Task<IActionResult> Edit(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null) return View("NotFound");
			return View(actorDetails);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureURL,Bio")] Actor actor)
		{
			if (!ModelState.IsValid)
			{
				return View(actor);
			}
			await _service.UpdateAsync(id, actor);
			return RedirectToAction(nameof(Index));
		}

		//Get: Actors/Delete/1
		public async Task<IActionResult> Delete(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null)
			{
				return View("NotFound");
			}

			// Check if the actor has associated movies before allowing deletion
			bool hasAssociatedMovies = await _service.HasAssociatedMoviesAsync(id);
			if (hasAssociatedMovies)
			{
				ModelState.AddModelError("", "Cannot delete actor with associated movies.");
				return View("Delete", actorDetails);
			}

			return View(actorDetails);
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var actorDetails = await _service.GetByIdAsync(id);
			if (actorDetails == null)
			{
				return View("NotFound");
			}

			// Check if the actor has associated movies
			if (await _service.HasAssociatedMoviesAsync(id))
			{
				return View("DeleteError");
			}

			await _service.DeleteAsync(id);
			return RedirectToAction(nameof(Index));
		}

	}
}
