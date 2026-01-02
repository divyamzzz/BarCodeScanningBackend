using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperCircle.Interfaces;
using SuperCircle.Models;

namespace SuperCircle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCardController : ControllerBase
    {
        private readonly SuperCircleContext _context;
        public JobCardController (SuperCircleContext context)
        {
            _context= context;
        }

        [HttpPost("createJobCardHeader")]
        public async Task<ActionResult> CreateJobCardHeader(ProductionOrderHeaderDto dto)
        {
            try
            {
                var prodDetails = await _context.ProductionOrders.Where(p => p.HeaderProdName == dto.RefNo).FirstOrDefaultAsync();
                var routeId = await _context.ProductionOrderHeaders.OrderByDescending(p => p.RouteCardNo).Select(p => p.RouteCardNo).FirstOrDefaultAsync();
                GenerateIdInterface createId = IDFactory.Create("route");
                var newId = createId.generateId(routeId);
                var header = new ProductionOrderHeader
                {
                    RouteCardNo = newId,
                    RefNo = dto.RefNo,
                    CustomerPartNo = dto.CustomerPartNo,
                    IssueDate = dto.IssueDate,
                    BackPlateNo = dto.BackPlateNo,
                    Formulation = dto.Formulation,
                    OrderQty = dto.OrderQty,
                    PlanningQty = dto.PlanningQty,
                    ClosingDate = dto.ClosingDate,
                    HeaderWareHouse=dto.HeaderWareHouse,
                    headerItem=prodDetails.HeaderItemCode
                };
                _context.ProductionOrderHeaders.Add(header);

                var stages=await _context.ProcessMasters.AsNoTracking().Where(s=>s.isActive==true).ToListAsync();
                foreach(var stage in stages)
                {
                    int x = stage.NumberOfStages;
                    for(int i=1;i<=x;i++)
                    {
                        var prodId = await _context.ProductionOrderStageMasters.AsNoTracking().Where(p => p.ProcessOperation == stage.ProcessName).OrderByDescending(p => p.ProductionOrderStageId).Select(p=>p.ProductionOrderStageId).FirstOrDefaultAsync();
                        ProductionOrderIdInterface createProdId = IDFactoryProd.Create("production");
                        var prodNewId = createProdId.GenerateProdId(stage.ProcessCode,i,dto.RefNo,prodId);
                        var productionOrder = new ProductionOrderStageMaster
                        {
                            ProductionOrderStageId = prodNewId,
                            StageNumber = i,
                            ProductionOrderNumber = dto.RefNo,
                            ProcessOperation = stage.ProcessName,
                            RouteCardNo=newId

                        };
                        _context.ProductionOrderStageMasters.Add(productionOrder);

                    }
                          
                }
                await _context.SaveChangesAsync();
                return Ok(newId);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetJobCardDetails/{id}")]
        public async Task<ActionResult>GetJobCard(string id)
        {
            try
            {
                var header = await _context.ProductionOrderHeaders.Where(p => p.RouteCardNo== id).FirstOrDefaultAsync();
                var stages = await _context.ProductionOrderStageMasters.Where(p=>p.RouteCardNo==id).ToListAsync();
                var res = new
                {
                    header = header,
                    stages=stages
                };
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetStageData/{id}")]
        public async Task<ActionResult>GetJobCardDetails(string id)
        {
            try
            {
                var stage = await _context.ProductionOrderStageMasters.Where(p => p.ProductionOrderStageId == id).FirstOrDefaultAsync();
                return Ok(stage);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateStage")]
        public async Task<ActionResult>UpdateJobCardDetails(UpdateStageDto dto)
        {
            try
            {
                var stage = await _context.ProductionOrderStageMasters.Where(p => p.ProductionOrderStageId == dto.stageId).FirstOrDefaultAsync();
                var prodDetails = await _context.ProductionOrders.Where(p => p.HeaderProdName == stage.ProductionOrderNumber).FirstOrDefaultAsync();
                if(stage==null)
                {
                    return NotFound();
                }
                stage.OperationDate = dto.OperationDate;
                stage.Shift = dto.Shift;
                stage.MachineNo = dto.MachineNo;
                stage.ProducedQty = dto.ProducedQty;
                stage.OkQty = dto.OkQty;
                stage.NotOkQty = dto.NotOkQty;
                stage.DefectReason = dto.DefectReason;
                stage.ProductionOperatorName =dto.ProductionOperatorName;
                stage.ProductionSupervisorName = dto.ProductionSupervisorName;
                stage.Remarks = dto.Remarks;
                stage.QualityQuantity = dto.qualityQty;

                var InventoryEntity=new InventoryStatusMaster {
                    ItemCode=prodDetails.HeaderItemCode,
                    ProducedQuantity=dto.ProducedQty,
                    OkQuantity=dto.OkQty,
                    NotOkQuantity=dto.NotOkQty,
                    ConsumedQuantity=dto.consumedQty,
                    rawItemCode=dto.rawItemCode,
                    QualityQuantity=dto.qualityQty
                };
                _context.InventoryStatusMasters.Add(InventoryEntity);
                await _context.SaveChangesAsync();
                return Ok(dto.stageId);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class ProductionOrderHeaderDto
        {
         

            public string RefNo { get; set; }
            public string CustomerPartNo { get; set; }

            public DateOnly IssueDate { get; set; }

            public string BackPlateNo { get; set; }
            public string Formulation { get; set; }

            public decimal OrderQty { get; set; }
            public decimal? PlanningQty { get; set; }

            public DateOnly? ClosingDate { get; set; }
            public string? HeaderWareHouse { get; set; }
        }

        public class UpdateStageDto
        {
            public string? stageId { get; set; }
            public DateOnly? OperationDate { get; set; }

            public string? Shift { get; set; } = null!;

            public string? MachineNo { get; set; }

            public decimal? ProducedQty { get; set; }

            public decimal? OkQty { get; set; }

            public decimal? NotOkQty { get; set; }

            public string? DefectReason { get; set; }

            public string? ProductionOperatorName { get; set; }

            public string? ProductionSupervisorName { get; set; }

            public string? Remarks { get; set; }
            public string? rawItemCode { get; set; }
            public decimal? consumedQty { get; set; }
            public decimal? qualityQty { get; set; }
        }

    }
}
