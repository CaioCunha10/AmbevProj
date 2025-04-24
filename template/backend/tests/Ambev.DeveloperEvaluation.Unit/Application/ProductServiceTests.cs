using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Entities.Products;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<ProductService>> _logger;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repository = new Mock<IProductRepository>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<ProductService>>();
            _service = new ProductService(_repository.Object, _mapper.Object, _logger.Object);
        }

        // CREATE TEST
        [Fact]
        public async Task CreateAsync_ShouldAddProduct_WhenDtoIsValid()
        {
            var dto = new ProductCreateDto { ProductCode = "P001", Name = "Product 1", Price = 10, Category = "Cat1" };
            var product = ProductEntity.Create(dto.ProductCode, dto.Name, dto.Price, dto.Category);

            _mapper.Setup(m => m.Map<ProductEntity>(dto)).Returns(product);
            _repository.Setup(r => r.CreateAsync(product)).Returns(Task.CompletedTask);
            _mapper.Setup(m => m.Map<ProductReadDto>(product)).Returns(new ProductReadDto { Name = dto.Name });

            var result = await _service.CreateAsync(dto);

            _repository.Verify(r => r.CreateAsync(product), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
        }

        // GET BY ID - EXISTS
        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenExists()
        {
            var id = Guid.NewGuid();
            var entity = ProductEntity.Create("P001", "Test", 15, "Cat");
            entity.Id = id;
            var dto = new ProductReadDto();

            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mapper.Setup(m => m.Map<ProductReadDto>(entity)).Returns(dto);

            var result = await _service.GetByIdAsync(id);

            Assert.NotNull(result);
        }

        // GET BY ID - NOT FOUND
        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ProductEntity)null);

            var result = await _service.GetByIdAsync(id);

            Assert.Null(result);
        }

        // GET ALL - EMPTY
        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoProducts()
        {
            _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ProductEntity>());
            _mapper.Setup(m => m.Map<IEnumerable<ProductReadDto>>(It.IsAny<IEnumerable<ProductEntity>>()))
                .Returns(new List<ProductReadDto>());

            var result = await _service.GetAllAsync();

            Assert.Empty(result);
        }

        // GET ALL - WITH PRODUCTS
        [Fact]
        public async Task GetAllAsync_ShouldReturnList_WhenProductsExist()
        {
            var products = new List<ProductEntity>
            {
                ProductEntity.Create("P001", "Product 1", 10, "Cat1"),
                ProductEntity.Create("P002", "Product 2", 20, "Cat2")
            };
            var dtos = new List<ProductReadDto> { new(), new() };

            _repository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);
            _mapper.Setup(m => m.Map<IEnumerable<ProductReadDto>>(products)).Returns(dtos);

            var result = await _service.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        // UPDATE - SUCCESS
        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenExists()
        {
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDto { Name = "Updated", Description = "Updated Desc", Price = 50 };
            var entity = ProductEntity.Create("P001", "Old", 10, "Cat");
            entity.Id = id;

            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _repository.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

             _mapper.Setup(m => m.Map(dto, entity))
                .Callback<ProductUpdateDto, ProductEntity>((src, dest) =>
                {
                    dest.Name = src.Name;
                    dest.Description = src.Description;
                    dest.Price = src.Price;
                });

            var result = await _service.UpdateAsync(id, dto);

            Assert.Equal("Updated", entity.Name);
            Assert.Equal("Updated Desc", entity.Description);
            Assert.Equal(50, entity.Price);
        }


        // UPDATE - NOT FOUND
        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenProductNotFound()
        {
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDto();

            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ProductEntity)null);

            await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(id, dto));
        }

        // DELETE - SUCCESS
        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct_WhenExists()
        {
            var id = Guid.NewGuid();
            var entity = ProductEntity.Create("P001", "Test", 15, "Cat");
            entity.Id = id;

            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _repository.Setup(r => r.DeleteAsync(id)).ReturnsAsync(true);

            var result = await _service.DeleteAsync(id);

            _repository.Verify(r => r.DeleteAsync(id), Times.Once);
            Assert.True(result);
        }

        // DELETE - NOT FOUND
        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenProductNotFound()
        {
            var id = Guid.NewGuid();
            _repository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ProductEntity)null);

            await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(id));
        }
    }
}
