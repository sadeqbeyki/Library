using LMS.Contracts.Book;
using LMS.Contracts.Translator;
using Microsoft.AspNetCore.Mvc;

namespace LMS.EndPoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslatorController : ControllerBase
    {
        private readonly ITranslatorService _translatorService;

        public TranslatorController(ITranslatorService translatorService)
        {
            _translatorService = translatorService;
        }

        [HttpGet]
        public IActionResult GetAllTranslators()
        {
            var translators = _translatorService.GetAll();
            return Ok(translators);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTranslatorById(Guid id)
        {
            var translator = await _translatorService.GetTranslator(id);
            if (translator == null)
                return NotFound();

            return Ok(translator);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTranslator([FromBody] TranslatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _translatorService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<TranslatorDto> UpdateTranslator(Guid id, [FromBody] TranslatorDto translator)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);
            var updatedTranslator = await _translatorService.UpdateTranslator(id, translator);

            return updatedTranslator;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranslator(Guid id)
        {
            await _translatorService.DeleteTranslator(id);
            return NoContent();
        }
        [HttpGet("TranslatorBooks/{id}")]
        public async Task<ActionResult<List<BookDto>>> GetTranslatorBooks(Guid id)
        {
            var books = await _translatorService.GetTranslatorBooks(id);
            if (books == null)
                return NoContent();
            return Ok(books);
        }
    }

}