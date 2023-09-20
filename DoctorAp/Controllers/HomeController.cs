using DoctorAp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DoctorAp.Controllers
{


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _connectionString = "Server=tcp:digicaregroup30.database.windows.net,1433;Initial Catalog=75;Persist Security Info=False;User ID=sqladmin;Password=Blackbird127;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        [Route("Admin")]

        // Action method for the default "Index" view
        public IActionResult Index()
        {
            return View();
        }

      

        // Action method for the "Privacy" view
        public IActionResult Privacy()
        {
            return View();
        }

        // Action method for the "Bill" view
        public IActionResult Bill()
        {
            List<ItemsLead> itemsLead = FetchItemsLead();
            return View(itemsLead);
        }

      


        // Action method for the "Stocks" view
        public IActionResult Stocks()
        {
            List<StockTrackerLead> stocks = FetchStockTrackerLead();
            return View(stocks);
        }

        // Action method for the "Links" view
        public IActionResult Links()
        {
            List<Contacts> contacts = FetchContacts();
            return View(contacts);
        }

        private List<Contacts> FetchContacts()
        {
            List<Contacts> contacts = new List<Contacts>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SELECT TOP (1000) * FROM [dbo].[AspNetUsers]", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(new Contacts()
                            {
                                Id = reader["Id"].ToString(),
                                Firstname = reader["FirstName"].ToString(),
                                Lastname = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch contacts");
                // Handle or log the exception as per your requirement
            }

            return contacts;
        }

       

        private List<StockTrackerLead> FetchStockTrackerLead()
        {
            List<StockTrackerLead> stocktrackerlead = new List<StockTrackerLead>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SELECT TOP (1000) * FROM [dbo].[StockTrackerLead]", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stocktrackerlead.Add(new StockTrackerLead()
                            {
                                Item = reader["Item"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                // Add more properties as needed
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch stocks");
                // Handle or log the exception as per your requirement
            }

            return stocktrackerlead;
        }

        private List<ItemsLead> FetchItemsLead()
        {
            List<ItemsLead> itemsLead = new List<ItemsLead>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SELECT TOP (1000) * FROM [dbo].[ItemsLead]", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            itemsLead.Add(new ItemsLead()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Item = reader["Item"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                CostPerItem = Convert.ToDecimal(reader["CostPerItem"])
                                // Add more properties as needed
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch items lead");
                // Handle or log the exception as per your requirement
            }

            return itemsLead;
        }

        // Action method for the "Error" view
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GetHTMLPageAsPDF()
        {
            try
            {
                List<ItemsLead> itemsLead = FetchItemsLead();

                var htmlContent = "<html><body>";

                decimal totalCost = 0; // Initialize total cost variable

                foreach (var item in itemsLead)
                {
                    decimal totalAmount = item.Quantity * item.CostPerItem; // Calculate total amount

                    htmlContent += $"<div>Item: {item.Item}</div>";
                    htmlContent += $"<div>Quantity: {item.Quantity}</div>";
                    htmlContent += $"<div>Cost Per Item: {item.CostPerItem}</div>";
                    htmlContent += $"<div>Total Amount: {totalAmount}</div>"; // Add total amount
                    htmlContent += "<hr />";

                    totalCost += totalAmount; // Add total amount to total cost
                }

                decimal orderNum = (totalCost * 9999); // Use your formula here

                int orderNumInteger = (int)orderNum; // Convert to integer if necessary

                htmlContent += $"<div>Order Number : {orderNumInteger}</div>"; // Add order number

                htmlContent += $"<div>Total Cost: {totalCost}</div>"; // Add total cost to the HTML

                htmlContent += "</body></html>";

                var Renderer = new IronPdf.ChromePdfRenderer();

                using (var PDF = Renderer.RenderHtmlAsPdf(htmlContent))
                {
                    var contentLength = PDF.BinaryData.Length;

                    Response.Headers["Content-Length"] = contentLength.ToString();
                    Response.Headers.Add("Content-Disposition", $"inline; filename={orderNumInteger}.pdf");

                    return File(PDF.BinaryData, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }





    }
}
