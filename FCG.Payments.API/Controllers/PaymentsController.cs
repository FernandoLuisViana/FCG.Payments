using FCG.Payments.API.Extensions;
using FCG.Payments.API.Filters;
using FCG.Payments.Domain.DTOs.Requests;
using FCG.Payments.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Payments.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController(IPaymentService service) : ControllerBase
{

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilter<ProcessPaymentRequest>))]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        var payments = await service.ProcessPaymentAsync(request);
        return payments.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var payments = await service.ListAsync();
        return payments.ToActionResult();
    }

    [HttpPost("internal")]
    [AllowAnonymous]
    public async Task<IActionResult> ProcessInternalPayment(
    [FromBody] ProcessPaymentRequest request)
    {
        var payments = await service.ProcessPaymentAsync(request);
        return payments.ToActionResult();
    }
}
