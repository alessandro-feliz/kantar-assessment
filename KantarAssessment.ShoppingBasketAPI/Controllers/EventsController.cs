using AutoMapper;
using KantarAssessment.ShoppingBasketAPI.Dto;
using KantarAssessment.ShoppingBasketAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KantarAssessment.ShoppingBasketAPI.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventsController(IMapper mapper, IEventService eventService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventService = eventService ?? throw new ArgumentNullException(nameof(eventService));
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _eventService.GetAllEventsAsync();

            return Ok(_mapper.Map<List<EventDto>>(events));
        }
    }
}