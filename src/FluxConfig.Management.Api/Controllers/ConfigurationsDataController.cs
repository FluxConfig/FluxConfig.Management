using FluxConfig.Management.Api.FiltersAttributes.Auth.Contexts;
using FluxConfig.Management.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.Controllers;

//TODO: Add
[ApiController]
[Route("configurations/data")]
public class ConfigurationsDataController: ControllerBase
{
    private readonly IConfigurationsDataService _configurationsDataService;
    private readonly IRequestAuthContext _requestAuthContext;

    public ConfigurationsDataController(IConfigurationsDataService dataService, IRequestAuthContext requestAuthContext)
    {
        _configurationsDataService = dataService;
        _requestAuthContext = requestAuthContext;
    }
}