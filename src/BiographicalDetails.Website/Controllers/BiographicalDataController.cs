using Microsoft.AspNetCore.Mvc;
using BiographicalDetails.Domain;
using BiographicalDetails.Application.Services;

namespace BiographicalDetails.Website.Controllers
{
    public class BiographicalDataController : Controller
    {
        private readonly BiographicalDetailsService _service;

        public BiographicalDataController(BiographicalDetailsService service)
        {
            _service = service;
        }

        // GET: BiographicalData
        public async Task<IActionResult> Index()
        {
			try
			{
				var biographicalData = await _service.GetAllBiographicalInfosAsync();
				return View(biographicalData);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
        }

        // GET: BiographicalData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biographicalData = await _service.GetBiographicalInfoAsync((int)id);
            if (biographicalData == null)
            {
                return NotFound();
            }

            return View(biographicalData);
        }

        // GET: BiographicalData/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BiographicalData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PreferredPronouns,LevelOfStudy,ImmigrationStatus,SocialInsuranceNumber,UniqueClientIdentifier")] BiographicalData biographicalData)
        {
            if (ModelState.IsValid)
            {
                await _service.SaveBiographicalInfoAsync(biographicalData);
                return RedirectToAction(nameof(Index));
            }
            return View(biographicalData);
        }

        // GET: BiographicalData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biographicalData = await _service.GetBiographicalInfoAsync((int)id);
            if (biographicalData == null)
            {
                return NotFound();
            }
            return View(biographicalData);
        }

        // POST: BiographicalData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PreferredPronouns,LevelOfStudy,ImmigrationStatus,SocialInsuranceNumber,UniqueClientIdentifier")] BiographicalData biographicalData)
        {
            if (id != biographicalData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateBiographicalInfoAsync(biographicalData);
                return RedirectToAction(nameof(Index));
            }
            return View(biographicalData);
        }

        // GET: BiographicalData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biographicalData = await _service.GetBiographicalInfoAsync((int)id);
            if (biographicalData == null)
            {
                return NotFound();
            }

            return View(biographicalData);
        }

        // POST: BiographicalData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var biographicalData = await _service.DeleteBiographicalInfoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
