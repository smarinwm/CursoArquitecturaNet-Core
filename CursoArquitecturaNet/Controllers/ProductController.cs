﻿using CursoArquitecturaNet.Application.Interfaces;
using CursoArquitecturaNet.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CursoArquitecturaNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProductService productService;

        public ProductController(IMapper mapper, IProductService productService)
        {
            this.mapper = mapper;
            this.productService = productService;
        }

        [HttpGet("GetProductByName/{productName}")]
        public async Task<IEnumerable<ProductDTO>> GetProducts(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                var list = await productService.GetProductList();
                var mapped = mapper.Map<IEnumerable<ProductDTO>>(list);
                return mapped;
            }

            var listByName = await productService.GetProductByName(productName);
            var mappedByName = mapper.Map<IEnumerable<ProductDTO>>(listByName);
            return mappedByName;
        }

        [HttpGet("GetProductById/{productId}")]
        public async Task<ProductDTO> GetProductById(int productId)
        {
            var product = await productService.GetProductById(productId);
            var mapped = mapper.Map<ProductDTO>(product);
            return mapped;
        }

        [HttpPost]
        public async Task<ProductDTO> CreateProduct(ProductDTO productViewModel)
        {
            var mapped = mapper.Map<ProductDTO>(productViewModel);
            if (mapped == null)
                throw new Exception($"Entity could not be mapped.");

            var entityDto = await productService.Create(mapped);

            var mappedViewModel = mapper.Map<ProductDTO>(mapped);
            return mappedViewModel;
        }

        [HttpPut]
        public async Task UpdateProduct(ProductDTO productViewModel)
        {
            var mapped = mapper.Map<ProductDTO>(productViewModel);
            if (mapped == null)
                throw new Exception($"Entity could not be mapped.");

            await productService.Update(mapped);
        }




    }
}
