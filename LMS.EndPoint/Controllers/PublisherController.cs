using LMS.Contracts.Publisher;
using LMS.Domain.PublisherAgg;
using Microsoft.AspNetCore.Mvc;

namespace LMS.EndPoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var publishers = _publisherService.GetAll();
            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var publisher = await _publisherService.GetById(id);
            if (publisher == null)
                return NotFound();

            return Ok(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PublisherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _publisherService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<PublisherDto> Update(Guid id, [FromBody] PublisherDto publisher)
        {
            var updatedPublisher = await _publisherService.Update(id, publisher);

            return updatedPublisher;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _publisherService.Delete(id);
            return NoContent();
        }
    }

}