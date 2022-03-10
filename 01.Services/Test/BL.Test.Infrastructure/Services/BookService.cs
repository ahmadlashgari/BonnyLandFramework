using BL.Framework.Persistence.MongoDB.Interfaces;
using BL.Test.ApplicationServices.Common.Interfaces;
using BL.Test.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Test.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IReadRepository<Book> _readRepository;
        private readonly IRepository<Book> _repository;

        public BookService(IMongoClient mongoClient, IConfiguration configuration, IReadRepository<Book> readRepository, IRepository<Book> repository)
        {
            _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

            var mongoDbConfiguration = configuration.GetSection("MongoDB");
            var mongoDbDatabaseConfiguration = mongoDbConfiguration.GetSection("BookDatabase");

            var mongoDatabase = mongoClient.GetDatabase(mongoDbDatabaseConfiguration.GetValue<string>("DatabaseName"));

            _readRepository.Collection = mongoDatabase.GetCollection<Book>(mongoDbDatabaseConfiguration.GetValue<string>("CollectionName"));
            _repository.Collection = mongoDatabase.GetCollection<Book>(mongoDbDatabaseConfiguration.GetValue<string>("CollectionName"));
        }

        public async Task<Book> CreateAsync(Book entity)
        {
            await _repository.InsertOneAsync(entity);

            return entity;
        }

        public async Task<List<Book>> GetAsync()
        {
            return await _readRepository.GetAllAsync();
        }

        public async Task<Book> GetByIdAsync(string id)
        {
            return await _readRepository.GetByIdAsync(id);
        }
    }
}
