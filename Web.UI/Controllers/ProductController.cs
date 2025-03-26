using API.Features.Product.Dtos;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Blazor.UI.Controllers;

public class ProductController : Controller
{
    private readonly RestClient _restClient;

    public ProductController()
    {
        _restClient = new RestClient("http://localhost:5276");
    }

    public async Task<IActionResult> Index()
    {
        var request = new RestRequest("products", Method.Get);
        var response = await _restClient.ExecuteAsync<List<ProductDto>>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return View(response.Data);
        }
        
        return View(new List<ProductDto>()); // Return empty list if Web.API fails
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductDto product)
    {
        if (!ModelState.IsValid)
        {
            return View(product);
        }

        var request = new RestRequest("products", Method.Post);
        request.AddJsonBody(product);
        var response = await _restClient.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Failed to add product.");
        return View(product);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var request = new RestRequest($"products/{id}", Method.Get);
        var response = await _restClient.ExecuteAsync<ProductDto>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return PartialView(response.Data);
        }
        return NotFound();
    }

    // POST: /Product/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProductDto product)
    {
        if (id != product.Id || !ModelState.IsValid)
        {
            return PartialView(product);
        }

        var request = new RestRequest($"products", Method.Put);
        request.AddJsonBody(product);
        var response = await _restClient.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Failed to update product.");
        return PartialView(product);
    }

  
    // GET: /Product/Delete/1
    public async Task<IActionResult> Delete(Guid id)
    {
        var request = new RestRequest($"products/{id}", Method.Get);
        var response = await _restClient.ExecuteAsync<ProductDto>(request);

        if (response.IsSuccessful && response.Data != null)
        {
            return PartialView(response.Data);
        }

        return NotFound();
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var request = new RestRequest($"products/{id}", Method.Delete);
        var response = await _restClient.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Delete), new { id });
    }
}