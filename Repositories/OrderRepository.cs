using System.Data;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using ecommerce.Data;
using ecommerce.Dtos.OrderDtos;
using ecommerce.Helpers;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using System.IO;
using SautinSoft.Document;

namespace ecommerce.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _connString;
        public OrderRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _connString = config.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Order?> CancelOrderAsync(string orderId, string appUserId)
        {
            var orderModel = await GetOrderByIdAsync(orderId);

            if (orderModel == null) return null;
            if (orderModel.AppUserId != appUserId) return null;

            orderModel.IsCanceled = true;
            orderModel.Status = "Canceled";

            await _context.SaveChangesAsync();

            return orderModel;
        }

        public async Task<Order> CreateOrderAsync(Order orderModel, List<Product_Order> product_Orders)
        {
            await _context.Product_Orders.AddRangeAsync(product_Orders);
            await _context.Orders.AddAsync(orderModel);
            await _context.SaveChangesAsync();

            return orderModel;
        }

        public async Task<Order?> DeleteOrderAsync(string orderId)
        {
            var orderModel = await GetOrderByIdAsync(orderId);
            if (orderModel == null) return null;

            _context.Remove(orderModel);
            await _context.SaveChangesAsync();

            return orderModel;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync(OrderQuery orderQuery)
        {
            try
            {
                List<OrderDto> orders = [];
                using (var connection = new SqlConnection(_connString))
                {
                    using (var command = new SqlCommand("GetAllOrders", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@DateStart", orderQuery.DateStart);
                        command.Parameters.AddWithValue("@DateEnd", orderQuery.DateEnd);
                        command.Parameters.AddWithValue("@PageSize", orderQuery.PageSize);
                        command.Parameters.AddWithValue("@PageNumber", orderQuery.PageNumber);

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            int idOrdinal = reader.GetOrdinal("Id");
                            int firstNameOrdinal = reader.GetOrdinal("FirstName");
                            int lastNameOrdinal = reader.GetOrdinal("LastName");
                            int phoneNumberOrdinal = reader.GetOrdinal("PhoneNumber");
                            int address1Ordinal = reader.GetOrdinal("Address1");
                            int address2Ordinal = reader.GetOrdinal("Address2");
                            int districtOrdinal = reader.GetOrdinal("District");
                            int cityOrdinal = reader.GetOrdinal("City");
                            int paymentOrdinal = reader.GetOrdinal("Payment");
                            int createdAtOrdinal = reader.GetOrdinal("CreateAt");
                            int updateAtOrdinal = reader.GetOrdinal("UpdateAt");
                            int subtotalOrdinal = reader.GetOrdinal("SubToTal");
                            int shippingPriceOrdinal = reader.GetOrdinal("ShippingPrice");
                            int discountOrdinal = reader.GetOrdinal("Discount");
                            int totalInvoicementOrdinal = reader.GetOrdinal("TotalInvoicement");
                            int statusOrdinal = reader.GetOrdinal("Status");
                            int isCanCeledOrdinal = reader.GetOrdinal("IsCanceled");
                            int userNameOrdinal = reader.GetOrdinal("UserName");
                            while (await reader.ReadAsync())
                            {
                                orders.Add(new OrderDto
                                {
                                    Id = reader.GetString(idOrdinal),
                                    FirstName = reader.GetString(firstNameOrdinal),
                                    LastName = reader.GetString(lastNameOrdinal),
                                    PhoneNumber = reader.GetString(phoneNumberOrdinal),
                                    Address1 = reader.GetString(address1Ordinal),
                                    Address2 = reader.IsDBNull(address2Ordinal) ? null : reader.GetString(address2Ordinal),
                                    District = reader.GetString(districtOrdinal),
                                    City = reader.GetString(cityOrdinal),
                                    Payment = reader.GetString(paymentOrdinal),
                                    CreateAt = reader.GetDateTime(createdAtOrdinal),
                                    UpdateAt = reader.IsDBNull(updateAtOrdinal) ? null : reader.GetDateTime(updateAtOrdinal),
                                    SubToTal = reader.GetDecimal(subtotalOrdinal),
                                    ShippingPrice = reader.GetDecimal(shippingPriceOrdinal),
                                    Discount = reader.GetDecimal(discountOrdinal),
                                    TotalInvoicement = reader.GetDecimal(totalInvoicementOrdinal),
                                    Status = reader.GetString(statusOrdinal),
                                    IsCanceled = reader.GetBoolean(isCanCeledOrdinal),
                                    UserName = reader.IsDBNull(userNameOrdinal) ? null : reader.GetString(userNameOrdinal),
                                });
                            }
                        }
                    }
                }
                return orders;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public DataTable GetEmpData()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Empdata";

            // add column name
            dt.Columns.Add("Column name", typeof(string));

            var _list = _context.Orders.ToList();

            if (_list.Count > 0)
            {
                _list.ForEach(item =>
                {
                    dt.Rows.Add(); // insert each row
                });
            }

            return dt;
        }

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            return await _context.Orders
                                .Include(order => order.Product_Orders)
                                .ThenInclude(p => p.Product)
                                .ThenInclude(p => p.Category)
                                .FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(string id)
        {
            return await _context.Orders
                            .Where(order => order.AppUserId == id)
                            .ToListAsync();
        }

        public async Task<string?> MergeReport(string orderId)
        {
            using (var connection = new SqlConnection(_connString))
            {
                using (var command = new SqlCommand("GetById_Order", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@OrderId", orderId);

                    await connection.OpenAsync();

                    string[] fieldNames = { "FirstName", "LastName", "PhoneNumber", "Address1", "District", "City", "Subtotal", "ShippingPrice", "Discount", "TotalInvoicement" };
                    // string[] fieldValues = {};
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var dataSource = new {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Address1 = reader.GetString(reader.GetOrdinal("Address1")),
                                District = reader.GetString(reader.GetOrdinal("District")),
                                City = reader.GetString(reader.GetOrdinal("City")),
                                SubToTal = Convert.ToString(reader.GetDecimal(reader.GetOrdinal("SubToTal"))),
                                ShippingPrice = Convert.ToString(reader.GetDecimal(reader.GetOrdinal("ShippingPrice"))),
                                DisCount = Convert.ToString(reader.GetDecimal(reader.GetOrdinal("DisCount"))),
                                TotalInvoicement = Convert.ToString(reader.GetDecimal(reader.GetOrdinal("TotalInvoicement"))),
                            };

                            string templatePath = "Data/Order.docx";

                            DocumentCore dc = DocumentCore.Load(templatePath);
                            dc.MailMerge.Execute(dataSource);

                            string readyDocPath = "OrderBill.docx";

                            dc.Save(readyDocPath);

                            return readyDocPath;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<Order?> UpdateOrderAsync(string id, UpdateOrderRequestDto orderDto, string appUserId)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);

            if (existingOrder == null) return null;
            if (existingOrder.AppUserId != appUserId) return null;

            existingOrder.FirstName = orderDto.FirstName;
            existingOrder.LastName = orderDto.LastName;
            existingOrder.PhoneNumber = orderDto.PhoneNumber;
            existingOrder.Address1 = orderDto.Address1;
            existingOrder.Address2 = orderDto.Address2;
            existingOrder.District = orderDto.District;
            existingOrder.City = orderDto.City;
            existingOrder.Payment = orderDto.Payment;
            existingOrder.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingOrder;
        }
    }
}