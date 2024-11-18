using System.Data;
using ecommerce.Data;
using ecommerce.Dtos.FeedbackDtos;
using ecommerce.Dtos.ProductDtos;
using ecommerce.Dtos.ProductImageDtos;
using ecommerce.Helpers;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using ecommerce.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;
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
        public async Task CreateProductAsync(CreateProductRequestDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("Create_Product", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Name", productDto.Name);
                        command.Parameters.AddWithValue("@OriginalPrice", productDto.OriginalPrice);
                        command.Parameters.AddWithValue("@ActualPrice", productDto.ActualPrice);
                        command.Parameters.AddWithValue("@Description", productDto.Discription);
                        command.Parameters.AddWithValue("@CategoryId", productDto.CategoryId);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    using (var command = new SqlCommand("Delete_Product", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProductId", id);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<ProductDto>> GetAllProductsAsync(QueryObject query)
        {
            var products = new List<ProductDto>();
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand("GetAllProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@QueryName", query.ProductName ?? null);
                    command.Parameters.AddWithValue("@MinPrice", query.MinPrice ?? null);
                    command.Parameters.AddWithValue("@MaxPrice", query.MaxPrice ?? null);
                    command.Parameters.AddWithValue("@PageNumber", query.PageNumber ?? 1);
                    command.Parameters.AddWithValue("@PageSize", query.PageSize ?? 20);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var product = new ProductDto
                            {
                                Id = reader.GetString(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                OriginalPrice = reader.GetDecimal(reader.GetOrdinal("OriginalPrice")),
                                ActualPrice = reader.GetDecimal(reader.GetOrdinal("ActualPrice")),
                                Discription = reader.GetString(reader.GetOrdinal("Discription")),
                                CreateAt = reader.GetDateTime(reader.GetOrdinal("CreateAt")),
                                CategoryId = reader.GetString(reader.GetOrdinal("CategoryId")),
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                Stars = reader.IsDBNull(reader.GetOrdinal("Stars")) ? null : reader.GetDecimal(reader.GetOrdinal("Stars")),
                                SaleCount = reader.GetInt32(reader.GetOrdinal("SaleCount"))
                            };

                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }

        public async Task<List<ProductDto>?> GetAllProductsByCategoryIdAsync(string id, QueryObject query)
        {
            try
            {
                var products = new List<ProductDto>();
                using (var connection = new SqlConnection(_connString)) {
                    using (var command = new SqlCommand("GetById_Product_CategoryId", connection)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@CategoryId", id);
                        command.Parameters.AddWithValue("@QueryName", query.ProductName);
                        command.Parameters.AddWithValue("@MinPrice", query.MinPrice);
                        command.Parameters.AddWithValue("@MaxPrice", query.MaxPrice);
                        command.Parameters.AddWithValue("@PageNumber", query.PageNumber);
                        command.Parameters.AddWithValue("@PageSize", query.PageSize);

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync()) {
                            var IdOrdinal = reader.GetOrdinal("Id");
                            var NameOrdinal = reader.GetOrdinal("Name");
                            var OriginalPriceOrdinal = reader.GetOrdinal("OriginalPrice");
                            var ActualPriceOrdinal = reader.GetOrdinal("ActualPrice");
                            var DiscriptionOrdinal = reader.GetOrdinal("Discription");
                            var CreateAtOrdinal = reader.GetOrdinal("CreateAt");
                            var CategoryIdOrdinal = reader.GetOrdinal("CategoryId");
                            var CategoryNameOrdinal = reader.GetOrdinal("CategoryName");
                            var StarsOrdinal = reader.GetOrdinal("Stars");

                            while(await reader.ReadAsync()) {
                                products.Add(new ProductDto {
                                    Id = reader.GetString(IdOrdinal),
                                    Name = reader.GetString(NameOrdinal),
                                    OriginalPrice = reader.GetDecimal(OriginalPriceOrdinal),
                                    ActualPrice = reader.GetDecimal(ActualPriceOrdinal),
                                    Discription = reader.GetString(DiscriptionOrdinal),
                                    CreateAt = reader.GetDateTime(CreateAtOrdinal),
                                    CategoryId = reader.GetString(CategoryIdOrdinal),
                                    CategoryName = reader.GetString(CategoryNameOrdinal),
                                    Stars = reader.IsDBNull(StarsOrdinal) ? null : reader.GetDecimal(reader.GetOrdinal("Stars")),
                                    SaleCount = reader.GetInt32(reader.GetOrdinal("SaleCount"))
                                });
                            }
                        }
                    }
                }
                return products;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ProductDetailDto?> GetProductByIdAsync(string id)
        {
            try
            {
                using (var connection = new SqlConnection(_connString)) {
                    using (var command = new SqlCommand("GetById_Product", connection)) {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProductId", id);
                        command.Parameters.Add("@StatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;

                        await connection.OpenAsync();

                        ProductDetailDto? productDto = null;
                        using (var reader = await command.ExecuteReaderAsync()) {
                            if (await reader.ReadAsync()) {
                                productDto = new ProductDetailDto {
                                    Id = reader.GetString(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    OriginalPrice = reader.GetDecimal(reader.GetOrdinal("OriginalPrice")),
                                    ActualPrice = reader.GetDecimal(reader.GetOrdinal("ActualPrice")),
                                    Discription = reader.GetString(reader.GetOrdinal("Discription")),
                                    CategoryId = reader.GetString(reader.GetOrdinal("CategoryId")),
                                    CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                    SaleCount = reader.GetInt32(reader.GetOrdinal("SaleCount")),
                                    Stars = reader.IsDBNull(reader.GetOrdinal("Stars")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Stars"))
                                };
                            }

                            if (await reader.NextResultAsync()) {
                                var imageIdOrdinal = reader.GetOrdinal("ImageId");
                                var imageNameOrdinal = reader.GetOrdinal("ImageName");

                                while (await reader.ReadAsync()) {
                                    productDto!.ProductImages.Add(new ProductImageDto {
                                        Id = reader.GetString(imageIdOrdinal),
                                        ImageName = reader.GetString(imageNameOrdinal),
                                        ImageBase64 = Convert.ToBase64String((byte[])reader["Data"])
                                    });
                                }
                            }

                            if (await reader.NextResultAsync()) {
                                var feedbackIdOrdinal = reader.GetOrdinal("FeedbackId");
                                var feedbackContentOrdinal = reader.GetOrdinal("Content");
                                var feedbackStarsOrdinal = reader.GetOrdinal("FeedbackStars");
                                var feedbackCreateAtOrdinal = reader.GetOrdinal("CreateAt");
                                var feedbackCreateByOrdinal = reader.GetOrdinal("CreateBy");

                                while (await reader.ReadAsync()) {
                                    productDto!.Feedbacks.Add(new FeedbackDto {
                                        Id = reader.GetString(feedbackIdOrdinal),
                                        Content = reader.GetString(feedbackContentOrdinal),
                                        Stars = reader.GetInt32(feedbackStarsOrdinal),
                                        CreateAt = reader.GetDateTime(feedbackCreateAtOrdinal),
                                        CreateBy = reader.IsDBNull(feedbackCreateByOrdinal) ? null : reader.GetString(feedbackCreateByOrdinal)
                                    });
                                }
                            }

                        }
                        int statusCode = Convert.ToInt32(command.Parameters["@StatusCode"].Value);

                        if (statusCode == 0) return null;

                        return productDto;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
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

        public async Task<int> UpdateProductAsync(string id, UpdateProductRequestDto productDto)
        {
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    using (var command = new SqlCommand("Update_Product", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProductId", id);
                        command.Parameters.AddWithValue("@Name", productDto.Name);
                        command.Parameters.AddWithValue("@OriginalPrice", productDto.OriginalPrice);
                        command.Parameters.AddWithValue("@ActualPrice", productDto.ActualPrice);
                        command.Parameters.AddWithValue("@Description", productDto.Discription);
                        command.Parameters.AddWithValue("@CategoryId", productDto.CategoryId);

                        command.Parameters.Add("@StatusCode", SqlDbType.Int).Direction = ParameterDirection.Output;

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        var statusCode = Convert.ToInt32(command.Parameters["@StatusCode"].Value);

                        return statusCode;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}