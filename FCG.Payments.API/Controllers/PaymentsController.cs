using FCG.Payments.API.Extensions;
using FCG.Payments.API.Filters;
using FCG.Payments.Domain.DTOs.Requests;
using FCG.Payments.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Payments.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController(IPaymentService service) : ControllerBase
{

    [HttpPost]
    [AllowAnonymous]
    [ServiceFilter(typeof(ValidationFilter<ProcessPaymentRequest>))]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentRequest request)
    {
        var payments = await service.ProcessPaymentAsync(request);
        return payments.ToActionResult();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> List()
    {
        var payments = await service.ListAsync();
        return payments.ToActionResult();
    }
}
