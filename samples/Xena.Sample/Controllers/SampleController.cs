using Microsoft.AspNetCore.Mvc;
using Xena.MemoryBus.Interfaces;
using Xena.Sample.MemoryBus.Command;
using Xena.Sample.MemoryBus.Event;
using Xena.Sample.MemoryBus.Query;

namespace Xena.Sample.Controllers;

[Route("[controller]")]
public class SampleController : Controller
{
    private readonly IXenaMemoryBus _memoryBus;

    public SampleController(IXenaMemoryBus memoryBus)
    {
        _memoryBus = memoryBus;
    }

    [HttpPost("command")]
    public async Task<IActionResult> InvokeCommand()
    {
        var simpleCommand = new SimpleCommand();
        await _memoryBus.Send(simpleCommand);

        return Accepted();
    }


    [HttpPost("event")]
    public async Task<IActionResult> InvokeEvent()
    {
        var simpleEvent = new SimpleEvent();
        await _memoryBus.Publish(simpleEvent);

        return Accepted();
    }

    [HttpGet("query")]
    public async Task<IActionResult> InvokeQuery()
    {
        var simpleQuery = new SimpleQuery();
        var result = await _memoryBus.Query(simpleQuery);
        return Ok(result);
    }
}