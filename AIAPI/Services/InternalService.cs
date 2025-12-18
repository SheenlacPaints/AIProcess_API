using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Newtonsoft.Json;
using AIAPI.Interfaces;
using Microsoft.Extensions.Options;
using AIAPI.DTOs;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
namespace AIAPI.Services
{
    public class InternalService : IInternalService
    {
        private readonly IConfiguration _config;

        public InternalService(IConfiguration _configuration)
        {
            _config = _configuration;
        }

        public async Task<string> GetAllProcessmetaAsync(paramDTO model)
        {
            try
            {
                using (var con = new SqlConnection(_config.GetConnectionString("Database")))
                using (var cmd = new SqlCommand("sp_get_AI_Process_V1", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FilterValue1", model.filtervalue1);
                    cmd.Parameters.AddWithValue("@FilterValue2", model.filtervalue2);
                    cmd.Parameters.AddWithValue("@FilterValue3", model.filtervalue3);
                    cmd.Parameters.AddWithValue("@FilterValue4", model.filtervalue4);
                    cmd.Parameters.AddWithValue("@FilterValue5", model.filtervalue5);
                    cmd.Parameters.AddWithValue("@FilterValue6", model.filtervalue6);
                    cmd.Parameters.AddWithValue("@FilterValue7", model.filtervalue7);
                    cmd.Parameters.AddWithValue("@FilterValue8", model.filtervalue8);
                    cmd.Parameters.AddWithValue("@FilterValue9", model.filtervalue9);
                    cmd.Parameters.AddWithValue("@FilterValue10", model.filtervalue10);
                    cmd.Parameters.AddWithValue("@FilterValue11", model.filtervalue11);
                    cmd.Parameters.AddWithValue("@FilterValue12", model.filtervalue12);
                    cmd.Parameters.AddWithValue("@FilterValue13", model.filtervalue13);
                    cmd.Parameters.AddWithValue("@FilterValue14", model.filtervalue14);
                    cmd.Parameters.AddWithValue("@FilterValue15", model.filtervalue15);
                    var ds = new DataSet();
                    var adapter = new SqlDataAdapter(cmd);
                    await Task.Run(() => adapter.Fill(ds)); // async wrapper

                    if (ds.Tables.Count > 0)
                    {
                        return JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    }

                    return "[]";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<ProcessMetaResponse> GetAllProcessmetaAsyncnew(paramDTO model)
        {
            var response = new ProcessMetaResponse
            {
                PurchaseHistoryMaster = new List<Dictionary<string, object>>(),
                SchemeMaster = new List<Dictionary<string, object>>(),
                FrequentPurchasedMaster = new List<Dictionary<string, object>>()
            };

            try
            {
                using var con = new SqlConnection(_config.GetConnectionString("Database"));
                using var cmd = new SqlCommand("sp_get_AI_Process_V1", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.CommandTimeout = 200;
                // Add parameters

                cmd.Parameters.AddWithValue("@FilterValue1", model.filtervalue1);
                cmd.Parameters.AddWithValue("@FilterValue2", model.filtervalue2);
                cmd.Parameters.AddWithValue("@FilterValue3", model.filtervalue3);
                cmd.Parameters.AddWithValue("@FilterValue4", model.filtervalue4);
                cmd.Parameters.AddWithValue("@FilterValue5", model.filtervalue5);
                cmd.Parameters.AddWithValue("@FilterValue6", model.filtervalue6);
                cmd.Parameters.AddWithValue("@FilterValue7", model.filtervalue7);
                cmd.Parameters.AddWithValue("@FilterValue8", model.filtervalue8);
                cmd.Parameters.AddWithValue("@FilterValue9", model.filtervalue9);
                cmd.Parameters.AddWithValue("@FilterValue10", model.filtervalue10);
                cmd.Parameters.AddWithValue("@FilterValue11", model.filtervalue11);
                cmd.Parameters.AddWithValue("@FilterValue12", model.filtervalue12);
                cmd.Parameters.AddWithValue("@FilterValue13", model.filtervalue13);
                cmd.Parameters.AddWithValue("@FilterValue14", model.filtervalue14);
                cmd.Parameters.AddWithValue("@FilterValue15", model.filtervalue15);
                await con.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    response.PurchaseHistoryMaster = ReadResultSet(reader);
                }


                if (await reader.NextResultAsync() && reader.HasRows)
                {
                    response.SchemeMaster = ReadResultSet(reader);
                }

                if (await reader.NextResultAsync() && reader.HasRows)
                {
                    response.FrequentPurchasedMaster = ReadResultSet(reader);
                }

                return response;
            }
            catch (Exception)
            {
                return response;
            }
        }

        private List<Dictionary<string, object>> ReadResultSet(SqlDataReader reader)
        {
            var result = new List<Dictionary<string, object>>();

            if (!reader.HasRows) return result;

            var columns = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();

                foreach (var column in columns)
                {
                    var value = reader[column];
                    row[column] = value == DBNull.Value ? null : value;
                }

                result.Add(row);
            }

            return result;
        }



        public async Task<List<OrderDTO>> GetAllPurchasehistorymetaAsync(LoginDTO model)

        {
            try
            {
                using var con = new SqlConnection(_config.GetConnectionString("Database"));
                using var cmd = new SqlCommand("sp_get_AI_Process_V1", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FilterValue1", (object?)model.EmpId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterValue2", "");
                cmd.Parameters.AddWithValue("@FilterValue3", "Grid2");
                cmd.Parameters.AddWithValue("@FilterValue4", (object?)model.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterValue5", "1");
                cmd.Parameters.AddWithValue("@FilterValue6", "");
                cmd.Parameters.AddWithValue("@FilterValue7", "");
                cmd.Parameters.AddWithValue("@FilterValue8", "");
                cmd.Parameters.AddWithValue("@FilterValue9", "");
                cmd.Parameters.AddWithValue("@FilterValue10", "");
                cmd.Parameters.AddWithValue("@FilterValue11", "");
                cmd.Parameters.AddWithValue("@FilterValue12", "");
                cmd.Parameters.AddWithValue("@FilterValue13", "");
                cmd.Parameters.AddWithValue("@FilterValue14", "");
                cmd.Parameters.AddWithValue("@FilterValue15", "");

                await con.OpenAsync();

                var flatList = new List<(OrderDTO Order, OrderDetails Detail)>();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var order = new OrderDTO
                    {
                        OrderID = reader["ORDER_ID"]?.ToString() ?? "",
                        Totalordervalue = reader["Total_Order_Value"]?.ToString() ?? "",
                        Totaldiscount = reader["Total_Discount"]?.ToString() ?? "",
                        DiscountPercentage = reader["Discount_Percentage"]?.ToString() ?? "",
                        Totalinvoicesvalue = reader["Total_Invoiced_Value"]?.ToString() ?? "",
                        Discountinvoiced = reader["Discount_Invoiced"]?.ToString() ?? "",
                        Balancevaluetobeinvoiced = reader["Balance_Value_To_Be_Invoiced"]?.ToString() ?? "",
                        Balancediscountobeinvoiced = reader["Balance_Discount_To_Be_Invoiced"]?.ToString() ?? ""
                    };

                    var detail = new OrderDetails
                    {
                        OrderID = reader["ORDER_ID"]?.ToString() ?? "",
                        Itemname = reader["Item_Name"]?.ToString() ?? "",
                        QuantityInorder = reader["Quantity_In_order"]?.ToString() ?? "",
                        Price = reader["Price"]?.ToString() ?? "",
                        DiscountInOrder = reader["Discount_In_Order"]?.ToString() ?? "",
                        StatusOfOrder = reader["Status_OF_Order"]?.ToString() ?? "",
                        Mobileno = reader["Mobileno"]?.ToString() ?? "",
                        Invoiced_Quantity = reader["Invoiced_Quantity"]?.ToString() ?? "",
                        DiscountINInvocie = reader["Discount_IN_Invoice"]?.ToString() ?? "",
                        InvoicedNumber = reader["Invoiced_Number"]?.ToString() ?? "",
                        Cancelled_Quantity = reader["Cancelled_Quantity"]?.ToString() ?? "",
                        DiscountINCancelledInvocie = reader["Discount_IN_Cancelled_Invocie"]?.ToString() ?? "",
                        CancelledInvoicedNumber = reader["Cancelled_Invoiced_Number"]?.ToString() ?? ""
                    };

                    flatList.Add((order, detail));
                }

                // ✅ SINGLE LINQ – group & build hierarchy
                var result = flatList
                    .GroupBy(x => new
                    {
                        x.Order.OrderID,
                        x.Order.Totalordervalue,
                        x.Order.Totaldiscount,
                        x.Order.DiscountPercentage,
                        x.Order.Totalinvoicesvalue,
                        x.Order.Discountinvoiced,
                        x.Order.Balancevaluetobeinvoiced,
                        x.Order.Balancediscountobeinvoiced
                    })
                    .Select(g =>
                    {
                        var parent = g.First().Order;
                        parent.Order_Details = g.Select(x => x.Detail).ToList();
                        return parent;
                    })
                    .ToList();
                return result;

            }
            catch
            {
                throw;
            }
        }

        public async Task<List<SchemeDTO>> GetAllSchemeProcessmetaAsync(LoginDTO model)
        {
            try
            {
                using var con = new SqlConnection(_config.GetConnectionString("Database"));
                using var cmd = new SqlCommand("sp_get_AI_Process_V1", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FilterValue1", (object?)model.EmpId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterValue2", "");
                cmd.Parameters.AddWithValue("@FilterValue3", "Grid2");
                cmd.Parameters.AddWithValue("@FilterValue4", (object?)model.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FilterValue5", "2");
                cmd.Parameters.AddWithValue("@FilterValue6", "");
                cmd.Parameters.AddWithValue("@FilterValue7", "");
                cmd.Parameters.AddWithValue("@FilterValue8", "");
                cmd.Parameters.AddWithValue("@FilterValue9", "");
                cmd.Parameters.AddWithValue("@FilterValue10","");
                cmd.Parameters.AddWithValue("@FilterValue11","");
                cmd.Parameters.AddWithValue("@FilterValue12","");
                cmd.Parameters.AddWithValue("@FilterValue13","");
                cmd.Parameters.AddWithValue("@FilterValue14","");
                cmd.Parameters.AddWithValue("@FilterValue15","");

                await con.OpenAsync();

                var flatList = new List<(SchemeDTO Scheme, SchemeDetails Detail)>();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var Scheme = new SchemeDTO
                    {
                        SchemeDocno = reader["Scheme_Docno"]?.ToString() ?? "",
                        SchemeName = reader["Scheme_Name"]?.ToString() ?? "",
                        Purchase_Value = reader["Base_Value"]?.ToString() ?? "",
                        Benifit_value = reader["Benifit_value"]?.ToString() ?? "",
                        Ratio= reader["Ratio"]?.ToString() ?? ""
                    };

                    var detail = new SchemeDetails
                    {
                       
                        SchemeName = reader["Scheme_Name"]?.ToString() ?? "",
                        SchemeGroup = reader["Scheme_Group"]?.ToString() ?? "",
                        MinuminValue = reader["Minumin_Value"]?.ToString() ?? "",
                        MaximumValue = reader["Maximum_Value"]?.ToString() ?? "",
                        DiscountType = reader["Discount_Type"]?.ToString() ?? "",
                        DiscountValue = reader["Discount_Value"]?.ToString() ?? "",
                        GiftDetails = reader["Gift_Details"]?.ToString() ?? "",
                        GiftQuantity = reader["Gift_Quantity"]?.ToString() ?? "",
                        ValidFrom = reader["Valid_From"]?.ToString() ?? "",
                        ValidTo = reader["Valid_To"]?.ToString() ?? "",
                        ProductGroup = reader["Product_Group"]?.ToString() ?? "",
                        SchemeType = reader["Scheme_Type"]?.ToString() ?? "",
                        BannerImage = reader["Banner_Image"]?.ToString() ?? "",
                        CX = reader["CX"]?.ToString() ?? "",
                        CY = reader["CY"]?.ToString() ?? "",
                        DECO = reader["DECO"]?.ToString() ?? "",
                        Benifit_value = reader["Benifit_value"]?.ToString() ?? ""

                    };

                    flatList.Add((Scheme, detail));
                }

                // ✅ SINGLE LINQ – group & build hierarchy
                var result = flatList
                    .GroupBy(x => new
                    {
                        x.Scheme.SchemeDocno,
                        x.Scheme.SchemeName,
                        x.Scheme.Purchase_Value
                    })
                    .Select(g =>
                    {
                        var parent = g.First().Scheme;
                        parent.SchemeDetails = g.Select(x => x.Detail).ToList();
                        return parent;
                    })
                    .ToList();

                return (result);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<purchaselist>> GetAllPurchaseProcessmetaAsync(LoginDTO model)
        {
            {
                try
                {
                    using var con = new SqlConnection(_config.GetConnectionString("Database"));
                    using var cmd = new SqlCommand("sp_get_AI_Process_V1", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@FilterValue1", (object?)model.EmpId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FilterValue2", "");
                    cmd.Parameters.AddWithValue("@FilterValue3", "Grid2");
                    cmd.Parameters.AddWithValue("@FilterValue4", (object?)model.PhoneNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FilterValue5", "3");
                    cmd.Parameters.AddWithValue("@FilterValue6", "");
                    cmd.Parameters.AddWithValue("@FilterValue7", "" );
                    cmd.Parameters.AddWithValue("@FilterValue8", "");
                    cmd.Parameters.AddWithValue("@FilterValue9", "");
                    cmd.Parameters.AddWithValue("@FilterValue10","");
                    cmd.Parameters.AddWithValue("@FilterValue11","");
                    cmd.Parameters.AddWithValue("@FilterValue12","");
                    cmd.Parameters.AddWithValue("@FilterValue13","");
                    cmd.Parameters.AddWithValue("@FilterValue14","");
                    cmd.Parameters.AddWithValue("@FilterValue15","");

                    await con.OpenAsync();

                    var flatList = new List<(string ProductType, PurchaseDTO Purchase, PurchaseDetails Detail)>();

                    using var reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        var productType = reader["Product_Type"]?.ToString() ?? "";

                        var Purchase = new PurchaseDTO
                        {
                            
                            Item_Group = reader["Item_Group"]?.ToString() ?? "",


                        };

                        var detail = new PurchaseDetails
                        {
                            Item_Name = reader["Item_Name"]?.ToString() ?? "",
                            Max_Quantity = reader["Max_Quantity"]?.ToString() ?? "",
                            Avg_quantity = reader["Avg_quantity"]?.ToString() ?? "",


                        };

                        flatList.Add((productType, Purchase, detail));
                    }

                    // ✅ SINGLE LINQ – group & build hierarchy
                    var result = flatList
                 .GroupBy(x => x.ProductType) // Group 1: Product Type (The Root)
                 .Select(productGroup => new purchaselist
                 {
                     Product_Type = productGroup.Key,
                     Purchase = productGroup
                         .GroupBy(p => new { p.Purchase.Item_Group }) // Group 2: Headers
                         .Select(headerGroup =>
                         {
                             var header = new PurchaseDTO
                             {                               
                                 Item_Group = headerGroup.Key.Item_Group,
                                 PurchaseDetails = headerGroup.Select(d => d.Detail).ToList() // Group 3: Details
                             };
                             return header;
                         }).ToList()
                 })
                 .ToList();

                    return (result);
                }
                catch
                {
                    throw;
                }
            }

        }
 
      
        public async Task<List<CustomerDTO>> FetchCustomerDetailAsync(string username)
        {
            try
            {
                using var con = new SqlConnection(_config.GetConnectionString("Database"));
                using var cmd = new SqlCommand("sp_get_AI_Process_V1", con)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@FilterValue1", username);
                cmd.Parameters.AddWithValue("@FilterValue2", "");
                cmd.Parameters.AddWithValue("@FilterValue3", "Customer_Fetch");
                cmd.Parameters.AddWithValue("@FilterValue4", "");
                cmd.Parameters.AddWithValue("@FilterValue5", "");
                cmd.Parameters.AddWithValue("@FilterValue6", "");
                cmd.Parameters.AddWithValue("@FilterValue7", "");
                cmd.Parameters.AddWithValue("@FilterValue8", "");
                cmd.Parameters.AddWithValue("@FilterValue9", "");
                cmd.Parameters.AddWithValue("@FilterValue10", "");
                cmd.Parameters.AddWithValue("@FilterValue11", "");
                cmd.Parameters.AddWithValue("@FilterValue12", "");
                cmd.Parameters.AddWithValue("@FilterValue13", "");
                cmd.Parameters.AddWithValue("@FilterValue14", "");
                cmd.Parameters.AddWithValue("@FilterValue15", "");

                await con.OpenAsync();
                var customerList = new List<CustomerDTO>();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var customer = new CustomerDTO
                    {
                        Customer_ID = reader["CustomerID"]?.ToString() ?? "",
                        Customer_Name = reader["customername"]?.ToString() ?? "",
                        Customer_Phoneno = reader["cust_phone_no"]?.ToString() ?? ""
                    };
                    customerList.Add(customer);
                }

                return customerList;
            }
            catch (Exception ex)
            {
 
                throw;
            }
        }
    }
    }

