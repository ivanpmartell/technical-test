using Microsoft.AspNetCore.Mvc;
using BiographicalDetails.Domain;
using BiographicalDetails.Application.Services;
using Microsoft.AspNetCore.Authorization;
using BiographicalDetails.Website.Models;
using BiographicalDetails.Website.Models.Mappers;
using BiographicalDetails.Application.Errors;

namespace BiographicalDetails.Website.Controllers;

[Authorize]
public class BiographicalDetailsController : Controller
{
    private readonly BiographicalDetailsService _service;
    private readonly BiographicalDataRequestsMapper _mapper;

	public BiographicalDetailsController(BiographicalDetailsService service, BiographicalDataRequestsMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    // GET: BiographicalData
    public async Task<IActionResult> Index()
    {
		var result = await _service.GetAllBiographicalInfosAsync();
        var biographicalDataCollection = _mapper.MapToBiographicalDetailsViewModelCollection(result);
		return View(biographicalDataCollection);
    }

    // GET: BiographicalData/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var result = await _service.GetBiographicalInfoAsync((int)id);
        if (result == null)
            return NotFound();

        var biographicalDetailsModel = _mapper.MapToBiographicalDetailsViewModel(result);
        return View(biographicalDetailsModel);
    }

    // GET: BiographicalData/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BiographicalData/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,PreferredPronouns,LevelOfStudy,ImmigrationStatus,SocialInsuranceNumber,UniqueClientIdentifier")] BiographicalDetailsModel submission)
    {
        if (ModelState.IsValid)
        {
            var biographicalData = _mapper.MapFrom(submission);
            try
            {
                var addedData = await _service.SaveBiographicalInfoAsync(biographicalData);
				if (addedData is not null)
					return RedirectToAction(nameof(Details), new { id = addedData.Id });

				ModelState.AddModelError(String.Empty, "Could not create the biographical data. Please try again later.");
			}
            catch (SINException ex)
            {
				ModelState.AddModelError(nameof(submission.SocialInsuranceNumber), ex.Message);
			}
            catch (UCIException ex)
            {
				ModelState.AddModelError(nameof(submission.UniqueClientIdentifier), ex.Message);
			}
        }

		return View(submission);
    }

    // GET: BiographicalData/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var result = await _service.GetBiographicalInfoAsync((int)id);
        if (result == null)
            return NotFound();

        var biographicalDetailsModel = _mapper.MapToBiographicalDetailsViewModel(result);
        return View(biographicalDetailsModel);
    }

    // POST: BiographicalData/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PreferredPronouns,LevelOfStudy,ImmigrationStatus,SocialInsuranceNumber,UniqueClientIdentifier")] BiographicalDetailsModel submission)
    {
        if (id != submission.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var biographicalData = _mapper.MapFrom(submission);
                if (await _service.UpdateBiographicalInfoAsync(biographicalData))
                    return RedirectToAction(nameof(Details), new { id = submission.Id });

				ModelState.AddModelError(String.Empty, "Could not update the biographical data. Please try again later.");
			}
            catch (SINException ex)
            {
                ModelState.AddModelError(nameof(submission.SocialInsuranceNumber), ex.Message);
            }
            catch (UCIException ex)
            {
                ModelState.AddModelError(nameof(submission.UniqueClientIdentifier), ex.Message);
            }
		}

        return View(submission);
    }

    // GET: BiographicalData/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var result = await _service.GetBiographicalInfoAsync((int)id);
        if (result == null)
            return NotFound();

        var biographicalDetailsModel = _mapper.MapToBiographicalDetailsViewModel(result);
		return View(biographicalDetailsModel);
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
