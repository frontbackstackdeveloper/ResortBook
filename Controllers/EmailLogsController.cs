using Microsoft.AspNetCore.Mvc;
using ResortBook.Services;

namespace ResortBook.Controllers;

public class EmailLogsController : Controller
{
    private readonly IEmailLogService _emailLogService;

    public EmailLogsController(IEmailLogService emailLogService)
    {
        _emailLogService = emailLogService;
    }

    public IActionResult Index()
    {
        var logs = _emailLogService.GetLogs();
        return View(logs);
    }
}