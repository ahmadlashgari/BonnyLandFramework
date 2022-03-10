using BL.Test.ApplicationServices.Common.Interfaces;
using BL.Test.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Threading.Tasks;

namespace BL.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ElasticClient _elasticClient;

        public BookController(IBookService bookService, ElasticClient elasticClient)
        {
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _bookService.GetAsync();

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create()
        {
            var result = await _bookService.CreateAsync(new Book { Name = "Test" });

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CreateElasticSearch()
        {
            await _elasticClient.IndexDocumentAsync<Book>(new Book { Name = "Test" });

            var result = await _elasticClient.SearchAsync<Book>(e => e.Query(q => q.MatchAll()));

            return Ok(result.Documents);
        }
    }
}
