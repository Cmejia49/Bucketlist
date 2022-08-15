using Category.Api.Data.Interface;
using Category.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Net;

namespace Category.Api.Controllers
{
    public class ClassificationController : ControllerBase
    {
        private readonly ILogger<ClassificationController> _logger;
        private readonly IClassification _context;
        public ClassificationController(ILogger<ClassificationController> logger, IClassification context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Classification>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Classification>>> GetClassifications()
        {
            var classifications = await _context
                                        .Classifications
                                        .Find(p => true)
                                        .ToListAsync();
            return Ok(classifications);
        }

        [HttpGet("{id:length(24)}", Name = "GetClassification")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Classification), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Classification>> GetClassificationsById(string id)
        {
            var classifications = await _context
                                        .Classifications
                                        .Find(p => p.Id == id)
                                        .FirstOrDefaultAsync();

            if (classifications == null)
            {
                _logger.LogError($"Product with id: {id}, not found.");
                return NotFound();
            }

            return Ok(classifications);
        }

        [Route("[action]/{name}", Name = "GetClassificationByName")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Classification>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Classification>>> GetClassificationsByName(string name)
        {
            FilterDefinition<Classification> filter = Builders<Classification>.Filter.ElemMatch(p => p.Name, name);

            var classifications = await _context
                                        .Classifications
                                        .Find(filter)
                                        .ToListAsync();

            if (classifications == null)
            {
                _logger.LogError($"Classification with name: {name} not found.");
                return NotFound();
            }
            return Ok(classifications);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Classification), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Classification>> CreateClassification([FromBody] Classification classification)
        {
            await _context.Classifications.InsertOneAsync(classification);

            return CreatedAtRoute("GetProduct", new { id = classification.Id }, classification);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Classification), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Classification), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateClassification([FromBody] Classification classification)
        {
            var updateResult = await _context
                                          .Classifications
                                          .ReplaceOneAsync(filter: g => g.Id == classification.Id, replacement: classification);

            var result = updateResult.IsAcknowledged
               && updateResult.ModifiedCount > 0;

            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteClassification")]
        [ProducesResponseType(typeof(Classification), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Classification), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteClassificationById(string id)
        {
            FilterDefinition<Classification> filter = Builders<Classification>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Classifications
                                                .DeleteOneAsync(filter);

            var result = deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;

            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
