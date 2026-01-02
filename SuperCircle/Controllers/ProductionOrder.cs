using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperCircle.Models;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;


namespace SuperCircle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionOrder : ControllerBase
    {

        private readonly SuperCircleContext _context;
        private readonly HttpClient _client;
        public ProductionOrder(SuperCircleContext context, HttpClient client)
        {
            _context = context;
            _client = client;
        }
        //[HttpGet]
        //public IActionResult GetData()
        //{
        //    string path = @"C:\Users\SAISATEIK\OneDrive - Saisatwik Technologies Private Limited\Desktop\SuperCircleProductionOrder.txt";
        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.Load(path);
        //        string json = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);

        //        // Return JSON
        //        return Content(json, "application/json");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new
        //        {
        //            message = "Error reading XML file",
        //            detail = ex.Message
        //        });
        //    }
        //}

        [HttpGet("getProductionOrder")]
        public async Task<IActionResult> GetProductionOrder()
        {
            var productionOrder = await _context.SuperCircles.AsNoTracking().ToListAsync();

            var xml = SerializeToXML(productionOrder, rootName: "ProductionOrders");

            return Content(xml, "application/xml");   // ✅ return XML instead of JSON
        }
        private string SerializeToXML<T>(T obj, string rootName = null)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");  // remove xmlns:xsi and xmlns:xsd

            XmlSerializer serializer;

            if (string.IsNullOrEmpty(rootName))
                serializer = new XmlSerializer(typeof(T));
            else
                serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));

            using var ms = new MemoryStream();
            using (var writer = new StreamWriter(ms, Encoding.UTF8))
            {
                serializer.Serialize(writer, obj, ns);
            }

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        [HttpGet("getproductionOrderfromapi")]
        public async Task<IActionResult> GetDataFromApi()
        {
            var response = await _client.GetAsync(
                "https://localhost:7101/api/ProductionOrder/getProductionOrder");

            response.EnsureSuccessStatusCode();

            var xmlString = await response.Content.ReadAsStringAsync();

            var doc = new XmlDocument();
            doc.LoadXml(xmlString);

            var json = JsonConvert.SerializeXmlNode(doc, Formatting.None, false);
            var jObject = JObject.Parse(json);
            jObject.Remove("?xml");

            var productionOrdersNode = jObject["ProductionOrders"];
            if (productionOrdersNode == null)
                return BadRequest("ProductionOrders node not found");

            var wrapper = productionOrdersNode.ToObject<ProductionOrderWrapper>();
            if (wrapper == null || !wrapper.SuperCircle.Any())
                return BadRequest("No data found");

            _context.ChangeTracker.Clear();

            foreach (var x in wrapper.SuperCircle)
            {
                _context.ProductionOrders.Add(new ProductionOrders
                {
                    DocDate = x.DocDate,
                    DocEntry = x.DocEntry,
                    DocNum = x.DocNum,
                    HeaderItemCode = x.HeaderItemCode,
                    HeaderPlannedQty = x.HeaderPlannedQty,
                    HeaderProdName = x.HeaderProdName,
                    HeaderWarehouse = x.HeaderWarehouse,
                    IssuedQty = x.IssuedQty,
                    LineNum = x.LineNum,
                    RowItemCode = x.RowItemCode,
                    RowPlannedQty = x.RowPlannedQty,
                    RowWarehouse = x.RowWarehouse,
                    SeriesName = x.SeriesName,
                    Status = x.Status,
                    ActualQuantity = 0
                });
            }

            await _context.SaveChangesAsync();


            await _context.SaveChangesAsync();

            return Ok(new
            {
                InsertedRecords = wrapper.SuperCircle.Count
            });
        }

        [HttpGet("getproductionorderfromid/{id}")]
        public async Task<ActionResult>GetProductionOrderFromId(string id)
        {
            try
            {
                var order = await _context.ProductionOrders.AsNoTracking().Where(p => p.HeaderProdName == id).ToListAsync();
                return Ok(order);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getSingleproductionorderfromid/{id}")]
        public async Task<ActionResult> GetSingleProductionOrderFromId(string id)
        {
            try
            {
                var order = await _context.ProductionOrders.AsNoTracking().Where(p => p.HeaderProdName == id).FirstOrDefaultAsync();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUniqueProdNames")]
        public async Task<ActionResult>GetUniqueProdNames(DateTime date)
        {
            try
            {
                var orders = await _context.ProductionOrders.AsNoTracking().Where(p=>p.DocDate<=date).GroupBy(p => p.HeaderProdName).Select(p =>
                
                    new
                    {
                        order = p.Key
                    }).ToListAsync();

                return Ok(orders);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getProductionOrderHeaderItem/{id}")]
        public async Task<ActionResult>GetProductionOrderHeaderItem(string id)
        {
            try
            {
                var item = await _context.ProductionOrders.Where(p => p.HeaderProdName == id).FirstOrDefaultAsync();
                return Ok(item.HeaderItemCode);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("getProductionOrderRowItems/{id}")]
        public async Task<ActionResult>GetProductionOrderItemsRow(string id)
        {
            try
            {
                var items = await _context.ProductionOrders
    .Where(p => p.HeaderProdName == id)
    .Select(p => p.RowItemCode)
    .Distinct()
    .ToListAsync();

                return Ok(items);
               
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public partial class SuperCircleModel
        {
            public byte Id { get; set; }

            public DateOnly DocDate { get; set; }

            public int DocEntry { get; set; }

            public int DocNum { get; set; }

            public string HeaderItemCode { get; set; } = null!;

            public short HeaderPlannedQty { get; set; }

            public string HeaderProdName { get; set; } = null!;

            public string HeaderWarehouse { get; set; } = null!;

            public double IssuedQty { get; set; }

            public byte LineNum { get; set; }

            public string RowItemCode { get; set; } = null!;

            public double RowPlannedQty { get; set; }

            public string RowWarehouse { get; set; } = null!;

            public string SeriesName { get; set; } = null!;

            public string Status { get; set; } = null!;
        }
    }
}
