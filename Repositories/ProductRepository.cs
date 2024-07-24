using System.Data;
using ecommerce.Data;
using ecommerce.Dtos.ProductDtos;
using ecommerce.Helpers;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using ecommerce.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductImageRepository _productImageRepo;
        private readonly string _connString;
        public ProductRepository(ApplicationDbContext context, IProductImageRepository productImageRepo, IConfiguration config)
        {
            _context = context;
            _productImageRepo = productImageRepo;
            _connString = config.GetConnectionString("DefaultConnection")!;
        }
        public async Task<(Product product, bool succeed, string message)> CreateProductAsync(CreateProductRequestDto productDto)
        {
            var productModel = productDto.ToProductFromCreate();

            await _context.Products.AddAsync(productModel);
            var uploadImageResult = await _productImageRepo.UploadProductImageFromProductCreateAsync(productModel.Id, productDto);

            if (!uploadImageResult.succeed) return (productModel, false, uploadImageResult.message);

            await _context.SaveChangesAsync();

            return (productModel, true, uploadImageResult.message);
        }

        public async Task<Product?> DeleteProductAsync(string id)
        {
            var productModel = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (productModel == null) return null;

            _context.Remove(productModel);
            await _context.SaveChangesAsync();

            return productModel;
        }

        public async Task<List<Product>> GetAllProductsAsync(QueryObject query)
        {
            var products = _context.Products
                                    .Include(p => p.Feedbacks)
                                    .ThenInclude(feedback => feedback.AppUser)
                                    .AsQueryable();

            // Search by product name
            if (!string.IsNullOrWhiteSpace(query.ProductName))
            {
                products = products.Where(p => p.Name.Contains(query.ProductName));
            }

            // Sort by options
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    products = query.IsDecending ? products.OrderByDescending(product => product.ActualPrice) : products.OrderBy(product => product.ActualPrice);
                }
            }

            // Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await products
                        .Skip(skipNumber)
                        .Take(query.PageSize)
                        .ToListAsync();
        }

        public async Task<List<Product>?> GetAllProductsByCategoryIdAsync(string id, QueryObject query)
        {
            var categoryExist = await _context.Categories.AnyAsync(category => category.Id == id);
            if (!categoryExist) return null;

            var products = _context.Products
                                    .Where(p => p.CategoryId == id)
                                    .Include(p => p.Feedbacks)
                                    .ThenInclude(feedback => feedback.AppUser)
                                    .AsQueryable();

            // Search by product name
            if (!string.IsNullOrWhiteSpace(query.ProductName))
            {
                products = products.Where(p => p.Name.Contains(query.ProductName));
            }

            // Sort by options
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    products = query.IsDecending ? products.OrderByDescending(product => product.ActualPrice) : products.OrderBy(product => product.ActualPrice);
                }
            }

            // Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await products
                        .Skip(skipNumber)
                        .Take(query.PageSize)
                        .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _context.Products
                                .Include(p => p.Category)
                                .Include(p => p.ProductImages)
                                .Include(p => p.Feedbacks).ThenInclude(f => f.AppUser)
                                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int?> GetSalesCount(string productId)
        {
            var productExist = await _context.Products.AnyAsync(p => p.Id == productId);
            if (!productExist) return null;

            var products = await _context.Product_Orders.Where(p => p.ProductId == productId).ToListAsync();

            int quantity = 0;
            foreach (var product in products)
            {
                quantity += product.Quantity;
            }

            return quantity;
        }

        public async Task<bool> ProductExist(string id)
        {
            return await _context.Products.AnyAsync(p => p.Id == id);
        }

        public async Task<Product?> UpdateProductAsync(string id, UpdateProductRequestDto productDto)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct == null) return null;

            existingProduct.Name = productDto.Name;
            existingProduct.OriginalPrice = productDto.OriginalPrice;
            existingProduct.ActualPrice = productDto.ActualPrice;
            existingProduct.Discription = productDto.Discription;
            existingProduct.CategoryId = productDto.CategoryId;

            await _context.SaveChangesAsync();
            return existingProduct;
        }
    }
}