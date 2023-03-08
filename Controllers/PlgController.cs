using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using newplgapi.model;
using newplgapi.Repository.Implements;
using newplgapi.Repository.Interfaces;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Tables;

namespace newplgapi.Controllers
{
    [Route("apimyrsup/[controller]")]
    [ApiController]
    public class PlgController : ControllerBase
    {
        private IDapperContext _context;
        private IHttpContextAccessor _httpContext;
        private IUnitOfWork _uow;
        private IConfiguration _config;

        public PlgController(IConfiguration config)
        {
            _config = config;
            _httpContext = new HttpContextAccessor();
        }

        // ============================= Auth ==============================
        [Authorize(Policy = "RequireAcces")]
        [HttpPost("SaveChangePassword")]
        public async Task<IActionResult> SaveChangePassword(ChangePassword input)
        {
            try
            {
                bool a;
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    a = await this._uow.PlgRepository.SaveChangePassword(input.UserId, input.OldPassword, input.NewPassword);
                }

                return !a ? (IActionResult)this.BadRequest((object)new
                {
                    msg = "Failed"
                }) : (IActionResult)this.Ok((object)new
                {
                    msg = "Success"
                });
            }
            catch (Exception ex)
            {
                return (IActionResult)this.BadRequest((object)new
                {
                    msg = ex.Message
                });
            }
        }

        [Authorize(Policy = "RequireAcces")]
        [HttpGet("GetMenuHdr")]
        public async Task<IActionResult> GetMenuHdr(string group)
        {
            try
            {
                var a = new List<MenuHdr>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var b = await _uow.PlgRepository.GetMenuHdr(group);
                    a = b.ToList();
                }
                return Ok(a);

            }
            catch (Exception e)
            {
                return BadRequest(new { msg = e.Message });
            }

        }

        [Authorize(Policy = "RequireAcces")]
        [HttpGet("GetMenuDtl")]
        public async Task<IActionResult> GetMenuDtl(string group)
        {
            try
            {
                var a = new List<MenuDtl>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var b = await _uow.PlgRepository.GetMenuDtl(group);
                    a = b.ToList();
                }

                return Ok(a);
            }
            catch (Exception e)
            {

                return BadRequest(new { msg = e.Message });
            }

        }

        // [Authorize(Policy = "RequireAcces")]
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> test(string sup)
        {
            try
            {
                var a = new SupplierSG();
                using (_context = new DapperContext("PSG"))
                {
                    _uow = new UnitOfWork(_context);
                    a = await _uow.PlgRepository.GetSupplierIDPSG(sup);
                }

                if (a == null ){
                    throw new Exception("Kode Supplier Sambu Group Belum di Set [RSUP]");
                }
                else{
                    return Ok(a);                    
                }

            }
            catch (Exception e)
            {
                return BadRequest(new { msg = e.Message });
            }

        }
        // ============================= End Auth ==============================





        // ============================= Master ==============================
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("SaveRoleDateTime")]
        public async Task<IActionResult> SaveRoleDateTime(RoleDateTime input)
        {
            try
            {
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveRoleDateTime(input);
                }

                return Ok(new { st = "Success" });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetDateTimeNow")]
        public async Task<IActionResult> GetDateTimeNow()
        {
            try
            {
                var dt = new DateTimeNow();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.PlgRepository.GetDateTimeNow();
                }

                return Ok(dt);
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetLastRoleDateTime0")]
        public async Task<IActionResult> GetLastRoleDateTime0()
        {
            try
            {
                var dt = new RoleDateTime();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.PlgRepository.GetLastRoleDateTime0();
                }


                return Ok(dt);
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetLastRoleDateTime1")]
        public async Task<IActionResult> GetLastRoleDateTime1()
        {
            try
            {
                var dt = new RoleDateTime();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.PlgRepository.GetLastRoleDateTime1();
                }

                return Ok(dt);
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetLastRoleDateTime2")]
        public async Task<IActionResult> GetLastRoleDateTime2()
        {
            try
            {
                var dt = new RoleDateTime();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.PlgRepository.GetLastRoleDateTime2();
                }

                return Ok(dt);
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("SaveBudgetPeriod")]
        public async Task<IActionResult> SaveBudgetPeriod(BudgetPeriod input)
        {
            try
            {
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveBudgetPeriod(input);
                }

                using (_context = new DapperContext("PSG"))
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveBudgetPeriod(input);
                }


                return Ok(new { st = "Success" });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetLastBudgetPeriod")]
        public async Task<IActionResult> GetLastBudgetPeriod()
        {
            try
            {
                var dt = new BudgetPeriod();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.PlgRepository.GetLastBudgetPeriod();
                }

                return Ok(dt);
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(string group)
        {
            try
            {
                var datas = new List<SubCategory>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var data = await _uow.PlgRepository.GetSubCategory(group);
                    datas = data.ToList();
                }

                return Ok(new { st = "Success", data = datas });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetCurrency")]
        public async Task<IActionResult> GetCurrency(string group)
        {
            try
            {
                var datas = new List<Currency>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var data = await _uow.PlgRepository.GetCurrency(group);
                    datas = data.ToList();
                }

                return Ok(new { st = "Success", data = datas });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        // [Authorize(Policy = "RequireAdminRole")]
        // [HttpGet("GetMyItem")]
        // public async Task<IActionResult> GetMyItem(string sup, string group)
        // {
        //     try
        //     {
        //         var dt = new List<ItemSupplier>();
        //         using (_context = new DapperContext("RSUP"))
        //         {
        //             _uow = new UnitOfWork(_context);
        //             var dtRSUP = await _uow.PlgRepository.GetMyItem(sup, group);
        //             dt = dtRSUP.ToList();
        //         }

        //         using (_context = new DapperContext("PSG"))
        //         {
        //             _uow = new UnitOfWork(_context);
        //             var dtPSG = await _uow.PlgRepository.GetMyItem(sup, group);
        //             dt.AddRange(dtPSG.ToList());
        //         }

        //         var a =  from i in dt
        //                     let j = new 
        //                     {
        //                         ItemID = i.ItemID,
        //                         NewItemID = i.NewItemID
        //                     }
        //                     group i by j into k
        //                     select new
        //                     {
        //                         ItemID = k.Key.ItemID,
        //                         NewItemID = k.Key.NewItemID                                
        //                     };
                
        //         var dtUnion = a.ToList();
        //         var dtFinal = new List<ItemSupplier>();

        //         foreach(var i in dtUnion){
        //             var item = new ItemSupplier();
        //             var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

        //             item.ItemID = i.ItemID;
        //             item.ItemName = dtFind.ItemName;
        //             item.ItemDesc = dtFind.ItemDesc;
        //             item.NewItemID = i.NewItemID;

        //             dtFinal.Add(item);
        //         }

        //         return Ok(new { st = "Success", data = dtFinal });
        //     }
        //     catch (System.Exception e)
        //     {
        //         return BadRequest(new { st = "Failed", msg = e.Message });
        //     }
        // }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetMyItem")]
        public async Task<IActionResult> GetMyItem(string sup, string group, int pages)
        {
            try
            {
                var dt = new List<ItemSupplier>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var dtRSUP_PSG = await _uow.PlgRepository.GetMyItem(sup, group, pages);
                    dt = dtRSUP_PSG.ToList();
                }

                using (_context = new DapperContext("PSG"))
                {
                    _uow = new UnitOfWork(_context);
                    var dtPSG = await _uow.PlgRepository.GetMyItem(sup, group, pages);
                    dt.AddRange(dtPSG.ToList());
                }

                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemSupplier>();

                foreach(var i in dtUnion){
                    var item = new ItemSupplier();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                    item.ItemID = i.ItemID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = dtFind.ItemDesc;
                    item.NewItemID = i.NewItemID;

                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetCountMyItem")]
        public async Task<IActionResult> GetCountMyItem(string sup)
        {
            try
            {
                var dt = new CountData();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.PlgRepository.GetCountMyItem(sup);
                }
                return Ok(dt);
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetChooseMyItem")]
        public async Task<IActionResult> GetChooseMyItem(string id, string sup, string group)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    var dtRSUP = await _uow.PlgRepository.GetChooseMyItem(id, sup, group);
                    dt = dtRSUP.ToList();
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    var dtPSG = await _uow.PlgRepository.GetChooseMyItem(id, sup, group);
                    dt.AddRange(dtPSG.ToList());
                }

                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                SubCategoryID = i.SubCategoryID,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                SubCategoryID = k.Key.SubCategoryID,
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID &&  x.NewItemID == i.NewItemID).FirstOrDefault();

                    item.ItemID = i.ItemID;
                    item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = dtFind.ItemDesc;
                    item.NewItemID = i.NewItemID;
                    
                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("SaveMyItem")]
        public async Task<IActionResult> SaveMyItem(MyItemInput input)
        {
            try
            {
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveMyItem(input);
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveMyItem(input);
                }                

                return Ok(new { st = "Success", msg = "Success To Save" });
            }
            catch (System.Exception e)
            {

                return Ok(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("RemoveMyItem")]
        public async Task<IActionResult> RemoveMyItem(MyItemInput input)
        {
            try
            {
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.RemoveMyItem(input);
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.RemoveMyItem(input);
                }
                

                return Ok(new { st = "Success", msg = "Success To Remove" });
            }
            catch (System.Exception e)
            {

                return Ok(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("RefreshItemOpenPrice")]
        public async Task<IActionResult> RefreshItemOpenPrice(ItemOpenPrice input)
        {
            try
            {
                using (_context = new DapperContext(input.FactAbbr))
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.RefreshItemOpenPrice();
                }

                return Ok(new { st = "Success" });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetItemOpenPrice")]
        public async Task<IActionResult> GetItemOpenPrice(string subCat, string factAbbr)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext(factAbbr)){
                    _uow = new UnitOfWork(_context);
                    var dtFact = await _uow.PlgRepository.GetItemOpenPrice(subCat);
                    dt = dtFact.ToList();
                }
                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                SubCategoryID = i.SubCategoryID,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                SubCategoryID = k.Key.SubCategoryID,
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID &&  x.NewItemID == i.NewItemID).FirstOrDefault();

                    item.ItemID = i.ItemID;
                    item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = dtFind.ItemDesc;
                    item.NewItemID = i.NewItemID;
                    
                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }
        // ============================= End Master ==============================





        // ============================= Transaksi Supplier ================================
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetProductItem")]
        public async Task<IActionResult> GetProductItem(string id, string sup, string group)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    var dtRSUP = await _uow.PlgRepository.GetProductItem(id, sup, group);
                    dt = dtRSUP.ToList();
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    var dtPSG = await _uow.PlgRepository.GetProductItem(id, sup, group);
                    dt.AddRange(dtPSG.ToList());
                }

                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                SubCategoryID = i.SubCategoryID,
                                UOMName = i.UOMName,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                SubCategoryID = k.Key.SubCategoryID,
                                UOMName = k.Key.UOMName,
                                Qnty = k.Sum(i => i.Qnty),
                                QtyRt = Math.Round(k.Sum(i => i.QtyRt)/2, 0, MidpointRounding.AwayFromZero),
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtGroup = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).ToList();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                    var factory = dtFind.Factory;
                    var itemDesc = dtFind.ItemDesc;

                    var factory2 = new List<string>();
                    var itemDesc2 = new List<string>();  
                    if(dtGroup.Count > 1 ) {
                        foreach(var j in dtGroup){
                            factory2.Add(j.Factory);
                            factory2.ToArray();

                            itemDesc2.Add("["+j.Factory+"] " + j.ItemDesc);
                            itemDesc2.ToArray();
                        }
                        factory = string.Join(", ", factory2);
                        itemDesc = string.Join(", ", itemDesc2);
                    }

                    item.ItemID = i.ItemID;
                    item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = itemDesc;
                    item.UOMName = i.UOMName;
                    item.Qnty = i.Qnty;
                    item.QtyRt = i.QtyRt;
                    item.NewItemID = i.NewItemID;
                    item.Factory = factory;

                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        // [Authorize(Policy = "RequireAdminRole")]
        // [HttpGet("GetMyProductItem")]
        // public async Task<IActionResult> GetMyProductItem(string sup, string group)
        // {
        //     try
        //     {
        //         var dt = new List<ItemRequest>();
        //         using(_context = new DapperContext("RSUP")){
        //             _uow = new UnitOfWork(_context);
        //             var dtRSUP = await _uow.PlgRepository.GetMyProductItem(sup, group);
        //             dt = dtRSUP.ToList();
        //         }

        //         using(_context = new DapperContext("PSG")){
        //             _uow = new UnitOfWork(_context);
        //             var dtPSG = await _uow.PlgRepository.GetMyProductItem(sup, group);
        //             dt.AddRange(dtPSG.ToList());
        //         }

        //         var a =  from i in dt
        //                     let j = new 
        //                     {
        //                         ItemID = i.ItemID,
        //                         SubCategoryID = i.SubCategoryID,
        //                         UOMName = i.UOMName,
        //                         NewItemID = i.NewItemID
        //                     }
        //                     group i by j into k
        //                     select new
        //                     {
        //                         ItemID = k.Key.ItemID,
        //                         SubCategoryID = k.Key.SubCategoryID,
        //                         UOMName = k.Key.UOMName,
        //                         Qnty = k.Sum(i => i.Qnty),
        //                         QtyRt = Math.Round(k.Sum(i => i.QtyRt)/2, 0, MidpointRounding.AwayFromZero),
        //                         NewItemID = k.Key.NewItemID                                
        //                     };
                
        //         var dtUnion = a.ToList();
        //         var dtFinal = new List<ItemRequest>();

        //         foreach(var i in dtUnion){
        //             var item = new ItemRequest();
        //             var dtGroup = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).ToList();
        //             var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

        //             var factory = dtFind.Factory;
        //             var itemDesc = dtFind.ItemDesc;

        //             var factory2 = new List<string>();
        //             var itemDesc2 = new List<string>(); 
        //             if(dtGroup.Count > 1 ) {
        //                 foreach(var j in dtGroup){
        //                     factory2.Add(j.Factory);
        //                     factory2.ToArray();

        //                     itemDesc2.Add("["+j.Factory+"] " + j.ItemDesc);
        //                     itemDesc2.ToArray();
        //                 }
        //                 factory = string.Join(", ", factory2);
        //                 itemDesc = string.Join(", ", itemDesc2);
        //             }

        //             item.ItemID = i.ItemID;
        //             item.SubCategoryID = i.SubCategoryID;
        //             item.ItemName = dtFind.ItemName;
        //             item.ItemDesc = itemDesc;
        //             item.UOMName = i.UOMName;
        //             item.Qnty = i.Qnty;
        //             item.QtyRt = i.QtyRt;
        //             item.NewItemID = i.NewItemID;
        //             item.Factory = factory;

        //             dtFinal.Add(item);
        //         }

        //         return Ok(new { st = "Success", data = dtFinal });
        //     }
        //     catch (System.Exception e)
        //     {

        //         return BadRequest(new { st = "Failed", msg = e.Message });
        //     }
        // }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetMyProductItem")]
        public async Task<IActionResult> GetMyProductItem(string sup, string group, int pages)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    var dtRSUP = await _uow.PlgRepository.GetMyProductItem(sup, group, pages);
                    dt = dtRSUP.ToList();
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    var dtPSG = await _uow.PlgRepository.GetMyProductItem(sup, group, pages);
                    dt.AddRange(dtPSG.ToList());
                }

                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                SubCategoryID = i.SubCategoryID,
                                UOMName = i.UOMName,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                SubCategoryID = k.Key.SubCategoryID,
                                UOMName = k.Key.UOMName,
                                Qnty = k.Sum(i => i.Qnty),
                                QtyRt = Math.Round(k.Sum(i => i.QtyRt)/2, 0, MidpointRounding.AwayFromZero),
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtGroup = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).ToList();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                    var factory = dtFind.Factory;
                    var itemDesc = dtFind.ItemDesc;

                    var factory2 = new List<string>();
                    var itemDesc2 = new List<string>(); 
                    if(dtGroup.Count > 1 ) {
                        foreach(var j in dtGroup){
                            factory2.Add(j.Factory);
                            factory2.ToArray();

                            itemDesc2.Add("["+j.Factory+"] " + j.ItemDesc);
                            itemDesc2.ToArray();
                        }
                        factory = string.Join(", ", factory2);
                        itemDesc = string.Join(", ", itemDesc2);
                    }

                    item.ItemID = i.ItemID;
                    item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = itemDesc;
                    item.UOMName = i.UOMName;
                    item.Qnty = i.Qnty;
                    item.QtyRt = i.QtyRt;
                    item.NewItemID = i.NewItemID;
                    item.Factory = factory;

                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetCountMyProductItem")]
        public async Task<IActionResult> GetCountMyProductItem(string sup)
        {
            try
            {
                var dt = new CountData();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    dt = await _uow.PlgRepository.GetCountMyProductItem(sup);
                }


                return Ok(dt);
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        // [Authorize(Policy = "RequireAdminRole")]
        // // [AllowAnonymous]
        // [HttpGet("DownloadMyProductItem")]
        // public IActionResult DownloadMyProductItem(string sup, string group)
        // {
        //     var dtSup = GetSupplierForPrint(sup);
        //     var dtProItem = GetMyProductItemForPrint(sup, group);
        //     var data = GeneratePDFMyProductItem(dtSup, dtProItem);

        //     string contentType = "application/pdf";
        //     //Define the file name
        //     string fileName = "PPH_MyProductItem_" + DateTime.Now + ".pdf";

        //     //Creates a FileContentResult object by using the file contents, content type, and file name
        //     return File(data, contentType, fileName);          
        // }

        [Authorize(Policy = "RequireAdminRole")]
        // [AllowAnonymous]
        [HttpGet("DownloadMyProductItem")]
        public IActionResult DownloadMyProductItem(string sup, string group, int pages)
        {
            var dtSup = GetSupplierForPrint(sup);
            var dtProItem = GetMyProductItemForPrint(sup, group, pages);
            var data = GeneratePDFMyProductItem(dtSup, dtProItem);

            string contentType = "application/pdf";
            //Define the file name
            string fileName = "PPH_MyProductItem_" + DateTime.Now + ".pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name
            return File(data, contentType, fileName);          
        }

        // private IEnumerable<ItemRequest> GetMyProductItemForPrint(string sup, string group){
        //     var dt = Task.Run(() => GetMonMyProductItemForPrint(sup, group));
        //     var dtProItem = dt.Result;

        //     return dtProItem;
        // }

        private IEnumerable<ItemRequest> GetMyProductItemForPrint(string sup, string group, int pages){
            var dt = Task.Run(() => GetMonMyProductItemForPrint(sup, group, pages));
            var dtProItem = dt.Result;

            return dtProItem;
        }

        // private async Task<IEnumerable<ItemRequest>> GetMonMyProductItemForPrint(string sup, string group){
        //     // var dtProItem = new List<ItemRequest>();
        //     // using(_context = new DapperContext("RSUP")){
        //     //     _uow = new UnitOfWork(_context);
        //     //     var dt = await _uow.PlgRepository.GetMyProductItem(sup, group);
        //     //     dtProItem = dt.ToList();
        //     // }
        //     var dt = new List<ItemRequest>();
        //     using(_context = new DapperContext("RSUP")){
        //         _uow = new UnitOfWork(_context);
        //         var dtRSUP = await _uow.PlgRepository.GetMyProductItem(sup, group);
        //         dt = dtRSUP.ToList();
        //     }

        //     using(_context = new DapperContext("PSG")){
        //         _uow = new UnitOfWork(_context);
        //         var dtPSG = await _uow.PlgRepository.GetMyProductItem(sup, group);
        //         dt.AddRange(dtPSG.ToList());
        //     }

        //     var a =  from i in dt
        //                 let j = new 
        //                 {
        //                     ItemID = i.ItemID,
        //                     SubCategoryID = i.SubCategoryID,
        //                     UOMName = i.UOMName,
        //                     NewItemID = i.NewItemID
        //                 }
        //                 group i by j into k
        //                 select new
        //                 {
        //                     ItemID = k.Key.ItemID,
        //                     SubCategoryID = k.Key.SubCategoryID,
        //                     UOMName = k.Key.UOMName,
        //                     Qnty = k.Sum(i => i.Qnty),
        //                     QtyRt = Math.Round(k.Sum(i => i.QtyRt)/2, 0, MidpointRounding.AwayFromZero),
        //                     NewItemID = k.Key.NewItemID                                
        //                 };
            
        //     var dtUnion = a.ToList();                
        //     var dtFinal = new List<ItemRequest>();

        //     foreach(var i in dtUnion){
        //         var item = new ItemRequest();
        //         var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

        //         item.ItemID = i.ItemID;
        //         item.SubCategoryID = i.SubCategoryID;
        //         item.ItemName = dtFind.ItemName;
        //         item.ItemDesc = dtFind.ItemDesc;
        //         item.UOMName = i.UOMName;
        //         item.Qnty = i.Qnty;
        //         item.QtyRt = i.Qnty;
        //         item.NewItemID = i.NewItemID;
        //         item.Factory = dtFind.Factory;

        //         dtFinal.Add(item);
        //     }
        //     return dtFinal;
        // }

        private async Task<IEnumerable<ItemRequest>> GetMonMyProductItemForPrint(string sup, string group, int pages){
            var dt = new List<ItemRequest>();
            using(_context = new DapperContext("RSUP")){
                _uow = new UnitOfWork(_context);
                var dtRSUP = await _uow.PlgRepository.GetMyProductItem(sup, group, pages);
                dt = dtRSUP.ToList();
            }

            using(_context = new DapperContext("PSG")){
                _uow = new UnitOfWork(_context);
                var dtPSG = await _uow.PlgRepository.GetMyProductItem(sup, group, pages);
                dt.AddRange(dtPSG.ToList());
            }

            var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                SubCategoryID = i.SubCategoryID,
                                UOMName = i.UOMName,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                SubCategoryID = k.Key.SubCategoryID,
                                UOMName = k.Key.UOMName,
                                Qnty = k.Sum(i => i.Qnty),
                                QtyRt = Math.Round(k.Sum(i => i.QtyRt)/2, 0, MidpointRounding.AwayFromZero),
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtGroup = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).ToList();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                    var factory = dtFind.Factory;
                    var itemDesc = dtFind.ItemDesc;

                    var factory2 = new List<string>();
                    var itemDesc2 = new List<string>(); 
                    if(dtGroup.Count > 1 ) {
                        foreach(var j in dtGroup){
                            factory2.Add(j.Factory);
                            factory2.ToArray();

                            itemDesc2.Add("["+j.Factory+"] " + j.ItemDesc);
                            itemDesc2.ToArray();
                        }
                        factory = string.Join(", ", factory2);
                        itemDesc = string.Join(", ", itemDesc2);
                    }

                    item.ItemID = i.ItemID;
                    item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = itemDesc;
                    item.UOMName = i.UOMName;
                    item.Qnty = i.Qnty;
                    item.QtyRt = i.QtyRt;
                    item.NewItemID = i.NewItemID;
                    item.Factory = factory;

                    dtFinal.Add(item);
                }
            return dtFinal;
        }

        private Stream GeneratePDFMyProductItem(Supplier dtSup, IEnumerable<ItemRequest> dtProItem)
        {
            CultureInfo culture = new CultureInfo("id-ID");

            //Create a new PDF document.
            PdfDocument pdfDocument = new PdfDocument();

            //Add a page to the PDF document
            PdfPage pdfPage = pdfDocument.Pages.Add();
            PdfGraphics graphics = pdfPage.Graphics;

            //Create a header and draw the image.
            RectangleF boundsHeader = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150); // Batas Bounds Header sebanyak 150
            RectangleF boundsFooter = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150); // Batas Bounds Header sebanyak 150

            PdfPageTemplateElement header = new PdfPageTemplateElement(boundsHeader);

            PdfFont fontHeader = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Underline|PdfFontStyle.Bold);
            PdfFont fontHeaderDetail = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            //Load the PDF document
            FileStream imageStream = new FileStream("wwwroot/Logo/RSUP.png", FileMode.Open, FileAccess.Read);
            PdfImage image = new PdfBitmap(imageStream);

            //Draw the header.
            // ini Header Kiri
            header.Graphics.DrawImage(image, new PointF(0, 0), new SizeF(50, 50));
            header.Graphics.DrawString("QUOTATION", fontHeader, PdfBrushes.Black, new Syncfusion.Drawing.PointF(215, 20));
            header.Graphics.DrawString("PT. Riau Sakti United Plantations", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 55));
            header.Graphics.DrawString("Pulau Burung", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 65));
            header.Graphics.DrawString("Phone : (0779) 54188, Fax : (0779) 541000", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 75));
            header.Graphics.DrawString("Email : p2-purchasing@rsup.co.id", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 85));

            // header.Graphics.DrawString("PPH No", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 105));
            // header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(60, 105));
            // header.Graphics.DrawString(dt.PPHNo, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(70, 105));

            // header.Graphics.DrawString("PPH Date", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 115));
            // header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(60, 115));
            // header.Graphics.DrawString(dt.TransDate.ToString("dd/MM/yyyy"), fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(70, 115));

            // // Ini Header Kanan (Y = 250)
            header.Graphics.DrawString("Supplier", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 55));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 55));
            header.Graphics.DrawString(dtSup.SupplierName, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 55));

            header.Graphics.DrawString("Contact P ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 65));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 65));
            header.Graphics.DrawString(dtSup.ContactPerson1 == null ? "" : dtSup.ContactPerson1, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 65));

            header.Graphics.DrawString("Address ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 75));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 75));
            header.Graphics.DrawString(dtSup.Address1 == null ? "" : dtSup.Address1, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 75));

            header.Graphics.DrawString("Country ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 85));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 85));
            header.Graphics.DrawString(dtSup.CountryName == null ? "" : dtSup.CountryName, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 85));

            header.Graphics.DrawString("Phone ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 95));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 95));
            header.Graphics.DrawString(dtSup.Telephone == null ? "" : dtSup.Telephone, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 95));

            header.Graphics.DrawString("Fax ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 105));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 105));
            header.Graphics.DrawString(dtSup.Fax == null ? "" : dtSup.Fax, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 105));

            header.Graphics.DrawString("Email ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 115));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 115));
            header.Graphics.DrawString(dtSup.Email == null ? "" : dtSup.Email, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 115));

            //Add the header at the top.
            pdfDocument.Template.Top = header;

            //================================================================
            PdfGrid pdfGrid = new PdfGrid();
            //Create a DataTable
            DataTable dataTable = new DataTable();
            //Add columns to the DataTable
            dataTable.Columns.Add("No");
            dataTable.Columns.Add(" Item Name");
            dataTable.Columns.Add("Factory");
            dataTable.Columns.Add("UOM");
            dataTable.Columns.Add("Quantity");
            dataTable.Columns.Add("Currency");
            dataTable.Columns.Add("Unit Price");
            dataTable.Columns.Add("Delivery Date");
            dataTable.Columns.Add("Remark");

            int no = 1;
            foreach (var dt in dtProItem)
            {
                var deliveryDate = dt.DeliveryDate != null ? dt.DeliveryDate.Value.ToString("dd/MM/yyy") : "";
                var unitPrice = dt.Harga == 0 ? "" : String.Format("{0:N}", dt.Harga);
                var data = new object[] { no, dt.ItemName + "\n" + dt.ItemDesc, dt.Factory,  dt.UOMName, Math.Round(dt.Qnty, 2), dt.Currency, unitPrice, deliveryDate, dt.Remark };
                dataTable.Rows.Add(data);
                no++;
            }

            //Assign data source
            pdfGrid.DataSource = dataTable;

            //Draw grid to the page of PDF document
            PdfStringFormat formatColumnNo = new PdfStringFormat(); formatColumnNo.Alignment = PdfTextAlignment.Center; formatColumnNo.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[0].Width = 20; pdfGrid.Columns[0].Format = formatColumnNo;
            PdfStringFormat formatColumnItemName = new PdfStringFormat(); formatColumnItemName.Alignment = PdfTextAlignment.Left; formatColumnItemName.LineAlignment = PdfVerticalAlignment.Middle; ;pdfGrid.Columns[1].Width = 150; pdfGrid.Columns[1].Format = formatColumnItemName;
            PdfStringFormat formatColumnFactory = new PdfStringFormat(); formatColumnFactory.Alignment = PdfTextAlignment.Center; formatColumnFactory.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[2].Width = 37; pdfGrid.Columns[2].Format = formatColumnFactory;
            PdfStringFormat formatColumnUOMName = new PdfStringFormat(); formatColumnUOMName.Alignment = PdfTextAlignment.Center; formatColumnUOMName.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[3].Width = 40; pdfGrid.Columns[3].Format = formatColumnUOMName;
            PdfStringFormat formatColumnQuantity = new PdfStringFormat(); formatColumnQuantity.Alignment = PdfTextAlignment.Right; formatColumnQuantity.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[4].Width = 40; pdfGrid.Columns[4].Format = formatColumnQuantity;
            PdfStringFormat formatColumnCurrency = new PdfStringFormat(); formatColumnCurrency.Alignment = PdfTextAlignment.Center; formatColumnCurrency.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[5].Width = 38; pdfGrid.Columns[5].Format = formatColumnCurrency;
            PdfStringFormat formatColumnPrice = new PdfStringFormat(); formatColumnPrice.Alignment = PdfTextAlignment.Center; formatColumnPrice.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[6].Width = 50; pdfGrid.Columns[6].Format = formatColumnPrice;
            PdfStringFormat formatColumnDate = new PdfStringFormat(); formatColumnDate.Alignment = PdfTextAlignment.Center; formatColumnDate.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[7].Width = 55; pdfGrid.Columns[7].Format = formatColumnDate;
            PdfStringFormat formatColumnRemark = new PdfStringFormat(); formatColumnRemark.Alignment = PdfTextAlignment.Center; formatColumnRemark.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[8].Width = 85; pdfGrid.Columns[8].Format = formatColumnRemark;

            //PdfGridLayoutFormat nty = new PdfGridLayoutFormat();

            pdfGrid.Draw(pdfPage, new PointF(0, 150));


            //=========================================================================


            //Create a Page template that can be used as footer.
            PdfPageTemplateElement footer = new PdfPageTemplateElement(boundsFooter);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 7);
            PdfBrush brush = new PdfSolidBrush(Color.Black);

            //Create page number field.
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, brush);

            //Create page count field.
            PdfPageCountField count = new PdfPageCountField(font, brush);

            //Add the fields in composite fields.
            PdfCompositeField compositeField = new PdfCompositeField(font, brush, "Page {0} of {1}", pageNumber, count);
            PdfCompositeField printedDate = new PdfCompositeField(font, brush, "Printed On : " + DateTime.Now);
            compositeField.Bounds = footer.Bounds;
            printedDate.Bounds = footer.Bounds;

            //Draw the composite field in footer.
            compositeField.Draw(footer.Graphics, new PointF(0, 140));
            printedDate.Draw(footer.Graphics, new PointF(410, 140));

            //Add the footer template at the bottom.
            // pdfDocument.Template.Bottom = footer;

            //Save the document into stream
            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            stream.Position = 0;

            //Closes the document
            pdfDocument.Close(true);
            string fileName = "PPH " + DateTime.Now;

            return stream;
        }    

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetMyPrice")]
        public async Task<IActionResult> GetMyPrice(string sup, string group)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    var dtRSUP = await _uow.PlgRepository.GetMyPrice(sup, group);
                    dt = dtRSUP.ToList();
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    var dtPSG = await _uow.PlgRepository.GetMyPrice(sup, group);
                    dt.AddRange(dtPSG.ToList());
                }

                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                SubCategoryID = i.SubCategoryID,
                                UOMName = i.UOMName,
                                Qnty = i.Qnty,
                                QtyRt = i.QtyRt,
                                Currency = i.Currency,
                                Harga = i.Harga,
                                ValidUntil = i.ValidUntil,
                                DeliveryDate = i.DeliveryDate,
                                ImportBy = i.ImportBy,
                                Periode = i.Periode,
                                Remark = i.Remark,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                SubCategoryID = k.Key.SubCategoryID,
                                UOMName = k.Key.UOMName,
                                Qnty = k.Key.Qnty,
                                QtyRt = k.Key.QtyRt,
                                Currency = k.Key.Currency,
                                Harga = k.Key.Harga,
                                ValidUntil = k.Key.ValidUntil,
                                DeliveryDate = k.Key.DeliveryDate,
                                ImportBy = k.Key.ImportBy,
                                Periode = k.Key.Periode,
                                Remark = k.Key.Remark,
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                    item.ItemID = i.ItemID;
                    item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = dtFind.ItemDesc;
                    item.UOMName = i.UOMName;
                    item.Qnty = i.Qnty;
                    item.QtyRt = i.QtyRt;
                    item.Currency = i.Currency;
                    item.Harga = i.Harga;
                    item.ValidUntil = i.ValidUntil;
                    item.DeliveryDate = i.DeliveryDate;
                    item.ImportBy = i.ImportBy;
                    item.IDInput = dtFind.IDInput;
                    item.Periode = i.Periode;
                    item.Remark = i.Remark;
                    item.NewItemID = i.NewItemID;
                    
                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetNewProductItem")]
        public async Task<IActionResult> GetNewProductItem(string sup, string group)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    var dtRSUP = await _uow.PlgRepository.GetNewProductItem(sup, group);
                    dt = dtRSUP.ToList();
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    var dtPSG = await _uow.PlgRepository.GetNewProductItem(sup, group);
                    dt.AddRange(dtPSG.ToList());
                }

                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                SubCategoryID = i.SubCategoryID,
                                UOMName = i.UOMName,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                SubCategoryID = k.Key.SubCategoryID,
                                UOMName = k.Key.UOMName,
                                Qnty = k.Sum(i => i.Qnty),
                                QtyRt = Math.Round(k.Sum(i => i.QtyRt)/2, 0, MidpointRounding.AwayFromZero),
                                NewItemID = k.Key.NewItemID                            
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtGroup = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).ToList();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                    var factory = dtFind.Factory;
                    var itemDesc = dtFind.ItemDesc;

                    var factory2 = new List<string>();
                    var itemDesc2 = new List<string>();  
                    if(dtGroup.Count > 1 ) {
                        foreach(var j in dtGroup){
                            factory2.Add(j.Factory);
                            factory2.ToArray();

                            itemDesc2.Add("["+j.Factory+"] " + j.ItemDesc);
                            itemDesc2.ToArray();
                        }
                        factory = string.Join(", ", factory2);
                        itemDesc = string.Join(", ", itemDesc2);
                    }

                    item.ItemID = i.ItemID;
                    item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = itemDesc;
                    item.UOMName = i.UOMName;
                    item.Qnty = i.Qnty;
                    item.QtyRt = i.QtyRt;
                    item.NewItemID = i.NewItemID;
                    item.Factory = factory;
                    item.Currency = dtFind.Currency;
                    item.Harga = dtFind.Harga;
                    item.ValidUntil = dtFind.ValidUntil;
                    item.FileName = dtFind.FileName;
                    item.Remark = dtFind.Remark;

                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("SavePrice")]
        public async Task<IActionResult> SavePrice(PriceInput input)
        {
            try
            {
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveData(input);
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveData(input);
                }                

                return Ok(new { st = "Success", msg = "Success To Save" });
            }
            catch (System.Exception e)
            {

                return Ok(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice(PriceInput input)
        {
            try
            {
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.UpdateData(input);
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.UpdateData(input);
                }                

                return Ok(new { st = "Success", msg = "Success To Save" });
            }
            catch (System.Exception e)
            {

                return Ok(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("SavePrice5")]
        public async Task<IActionResult> SavePrice5([FromForm] PriceInputUpload docFile)
        {
            try
            {
                if (docFile.file == null || docFile.file.Length == 0)
                    return Ok(new { st = "File not found!" });

                var ext = Path.GetExtension(docFile.file.FileName);
                var filename = docFile.ChangeName == "" ? docFile.FileName : docFile.ChangeName + ext;

                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Doc\\", filename);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await docFile.file.CopyToAsync(stream);
                    }
                }
                catch (System.Exception e)
                {
                    return BadRequest(Ok(new { st = e, msg = "Upload failed" }));
                }

                PriceInput input = new PriceInput{
                    SupplierId = docFile.SupplierId,
                    ItemId = docFile.ItemId,
                    Qnty = docFile.Qnty,
                    QtyRt = docFile.QtyRt,
                    Harga = docFile.Harga,
                    Currency = docFile.Currency,
                    ValidUntil = docFile.ValidUntil,
                    DeliveryDate = docFile.DeliveryDate,
                    Periode = docFile.Periode,
                    CreatedBy = docFile.CreatedBy,
                    NewItemID = docFile.NewItemID,
                    Factory = docFile.Factory,
                    FileName = filename,
                    Remark = docFile.Remark
                };

                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveData(input);
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveData(input);
                }                

                return Ok(new { st = "Success", msg = "Success To Save" });
            }
            catch (System.Exception e)
            {

                return Ok(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("UpdatePrice5")]
        public async Task<IActionResult> UpdatePrice5([FromForm] PriceInputUpload docFile)
        {
            try
            {
                if (docFile.file == null || docFile.file.Length == 0)
                    return Ok(new { st = "File not found!" });

                var ext = Path.GetExtension(docFile.file.FileName);
                var filename = docFile.ChangeName == "" ? docFile.FileName : docFile.ChangeName + ext;

                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Doc\\", filename);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await docFile.file.CopyToAsync(stream);
                    }
                }
                catch (System.Exception e)
                {
                    return BadRequest(Ok(new { st = e, msg = "Upload failed" }));
                }

                PriceInput input = new PriceInput{
                    SupplierId = docFile.SupplierId,
                    ItemId = docFile.ItemId,
                    Harga = docFile.Harga,
                    Currency = docFile.Currency,
                    ValidUntil = docFile.ValidUntil,
                    DeliveryDate = docFile.DeliveryDate,
                    Periode = docFile.Periode,
                    UpdatedBy = docFile.UpdatedBy,
                    NewItemID = docFile.NewItemID,
                    FileName = filename,
                    Remark = docFile.Remark
                };

                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.UpdateData(input);
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.UpdateData(input);
                }                

                return Ok(new { st = "Success", msg = "Success To Save" });
            }
            catch (System.Exception e)
            {

                return Ok(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("DownloadFile")]
        public IActionResult DownloadFile(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            try
            {
                var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot\\Doc\\", filename);

                var streami = new FileStream(@path, FileMode.Open);
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(streami);
                int cp = loadedDocument.Pages.Count;
                // PdfPageBase loadedPage;
                // PdfGraphics graphics;
                // PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 100);
                // PdfGraphicsState state;

                // if(factAbbr != "PSS"){
                //     for (int i = 0; i < cp; i++)
                //     {
                //         loadedPage = loadedDocument.Pages[i];
                //         graphics = loadedPage.Graphics;
                //         state = graphics.Save();
                //         graphics.SetTransparency(0.15f);
                //         graphics.RotateTransform(-60);
                //         graphics.DrawString(userID, font, PdfBrushes.Gray, new PointF(-500, 350));
                //     }
                // }

                MemoryStream stream = new MemoryStream();
                loadedDocument.Save(stream);
                loadedDocument.Close(true);
                streami.Close();
                stream.Position = 0;
                string contentType = "application/pdf";
                //Define the file name.
                string fileNames = "watermark.pdf";
                //Creates a FileContentResult object by using the file contents, content type, and file name.

                // insert History
                // var hs = new HistoryDownload
                // {
                //     Nama = userID,
                //     FileName = id,
                //     Tanggal = DateTime.Now,
                //     Tipe = "Download"
                // };

                // using (_context = new DapperContext())
                // {
                //     _uow = new UnitOfWork(_context);
                //      _uow.DocumentRepository.InsertHistory(hs);
                // }
                return File(stream, contentType, fileNames);
            }
            catch (System.Exception ex)
            {
                return BadRequest(Ok(new { st = ex }));
            }
        }

        [AllowAnonymous]
        [HttpGet("DownloadFile2")]
        public IActionResult DownloadFile2(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            try
            {
                var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot\\Doc\\", filename);

                var streami = new FileStream(@path, FileMode.Open);
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(streami);
                int cp = loadedDocument.Pages.Count;
                // PdfPageBase loadedPage;
                // PdfGraphics graphics;
                // PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 100);
                // PdfGraphicsState state;

                // if(factAbbr != "PSS"){
                //     for (int i = 0; i < cp; i++)
                //     {
                //         loadedPage = loadedDocument.Pages[i];
                //         graphics = loadedPage.Graphics;
                //         state = graphics.Save();
                //         graphics.SetTransparency(0.15f);
                //         graphics.RotateTransform(-60);
                //         graphics.DrawString(userID, font, PdfBrushes.Gray, new PointF(-500, 350));
                //     }
                // }

                MemoryStream stream = new MemoryStream();
                loadedDocument.Save(stream);
                loadedDocument.Close(true);
                streami.Close();
                stream.Position = 0;
                string contentType = "application/pdf";
                //Define the file name.
                string fileNames = filename;
                //Creates a FileContentResult object by using the file contents, content type, and file name.

                // insert History
                // var hs = new HistoryDownload
                // {
                //     Nama = userID,
                //     FileName = id,
                //     Tanggal = DateTime.Now,
                //     Tipe = "Download"
                // };

                // using (_context = new DapperContext())
                // {
                //     _uow = new UnitOfWork(_context);
                //      _uow.DocumentRepository.InsertHistory(hs);
                // }
                return File(stream, contentType, fileNames);
            }
            catch (System.Exception ex)
            {
                return BadRequest(Ok(new { st = ex }));
            }
        }
        // ============================= End Transaksi Supplier ================================





        // ============================= Transaksi Purchaser ================================
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetPriceItem")]
        public async Task<IActionResult> GetPriceItem(string sub, string group, string factAbbr)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext(factAbbr)){
                    _uow = new UnitOfWork(_context);
                    var dtFact = await _uow.PlgRepository.GetProductItemPrice(sub, group, factAbbr);
                    dt = dtFact.ToList();
                }

                return Ok(new { st = "Success", data = dt });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("SaveToPPH")]
        public async Task<IActionResult> SaveToPPH(ImportToPPH input)
        {
            try
            {
                string dtRSUP;
                string dtPSG;
                string pphNo;
                var supRSUP = new SupplierSG();
                var supPSG = new SupplierSG();

                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    supRSUP = await _uow.PlgRepository.GetSupplierSG(input.SupplierID);

                    if (supRSUP == null ){
                        throw new Exception("Kode Supplier Sambu Group Belum di Set [RSUP]");
                    }
                    else{
                        dtRSUP = await _uow.PlgRepository.SaveToPPH(input.ItemID, input.SupplierID, input.Period, input.User);
                    }
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    supPSG = await _uow.PlgRepository.GetSupplierSG(supRSUP.kode_supplier_sg);

                    if (supPSG == null ){
                        throw new Exception("Kode Supplier Sambu Group Belum di Set [PSG] Untuk Kode " + supRSUP.kode_supplier_sg);
                    }
                    else{
                        dtPSG = await _uow.PlgRepository.SaveToPPH(input.ItemID, supPSG.SupplierID, input.Period, input.User);
                    }  
                }

                if(input.FactAbbr == "RSUP"){
                    pphNo = dtRSUP;
                } 
                else{
                    pphNo = dtPSG;
                }

                return Ok(new { st = "Success", msg = pphNo });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetPriceNewItem")]
        public async Task<IActionResult> GetPriceNewItem(string factAbbr)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext(factAbbr)){
                    _uow = new UnitOfWork(_context);
                    var dtFact = await _uow.PlgRepository.GetNewProductItemPrice(factAbbr);
                    dt = dtFact.ToList();
                }

                return Ok(new { st = "Success", data = dt });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("UpdateToNewItem")]
        public async Task<IActionResult> UpdateToNewItem(UpdateToNewItem input)
        {
            try
            {
                string a; 
                using(_context = new DapperContext(input.Factory)){
                    _uow = new UnitOfWork(_context);
                    a = await _uow.PlgRepository.UpdateToNewItem(input.Id, input.User);
                }
                return Ok(new { st = "Success", msg = a });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }
        // ============================= End Transaksi Purchaser ================================ 
        




        // ============================= Monitoring ================================
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetMonMyPrice")]
        public async Task<IActionResult> GetMonMyPrice(string prd, string sup, string group)
        {
            try
            {
                var dt = new List<ItemRequest>();
                using(_context = new DapperContext("RSUP")){
                    _uow = new UnitOfWork(_context);
                    var dtRSUP = await _uow.PlgRepository.GetMonMyPrice(prd, sup, group);
                    dt = dtRSUP.ToList();
                }

                using(_context = new DapperContext("PSG")){
                    _uow = new UnitOfWork(_context);
                    var dtPSG = await _uow.PlgRepository.GetMonMyPrice(prd, sup, group);
                    dt.AddRange(dtPSG.ToList());
                }

                var a =  from i in dt
                            let j = new 
                            {
                                ItemID = i.ItemID,
                                //SubCategoryID = i.SubCategoryID,
                                UOMName = i.UOMName,
                                Qnty = i.Qnty,
                                //QtyRt = i.QtyRt,
                                Currency = i.Currency,
                                Harga = i.Harga,
                                //ValidUntil = i.ValidUntil,
                                DeliveryDate = i.DeliveryDate,
                                //ImportBy = i.ImportBy,
                                //Periode = i.Periode,
                                Remark = i.Remark,
                                NewItemID = i.NewItemID
                            }
                            group i by j into k
                            select new
                            {
                                ItemID = k.Key.ItemID,
                                //SubCategoryID = k.Key.SubCategoryID,
                                UOMName = k.Key.UOMName,
                                Qnty = k.Key.Qnty,
                                //QtyRt = k.Key.QtyRt,
                                Currency = k.Key.Currency,
                                Harga = k.Key.Harga,
                                //ValidUntil = k.Key.ValidUntil,
                                DeliveryDate = k.Key.DeliveryDate,
                                //ImportBy = k.Key.ImportBy,
                                //Periode = k.Key.Periode,
                                Remark = k.Key.Remark,
                                NewItemID = k.Key.NewItemID                                
                            };
                
                var dtUnion = a.ToList();
                var dtFinal = new List<ItemRequest>();

                foreach(var i in dtUnion){
                    var item = new ItemRequest();
                    var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                    item.ItemID = i.ItemID;
                    //item.SubCategoryID = i.SubCategoryID;
                    item.ItemName = dtFind.ItemName;
                    item.ItemDesc = dtFind.ItemDesc;
                    item.UOMName = i.UOMName;
                    item.Qnty = i.Qnty;
                    //item.QtyRt = i.QtyRt;
                    item.Currency = i.Currency;
                    item.Harga = i.Harga;
                    //item.ValidUntil = i.ValidUntil;
                    item.DeliveryDate = i.DeliveryDate;
                    //item.ImportBy = i.ImportBy;
                    item.IDInput = dtFind.IDInput;
                    //item.Periode = i.Periode;
                    item.Remark = i.Remark;
                    item.NewItemID = i.NewItemID;
                    
                    dtFinal.Add(item);
                }

                return Ok(new { st = "Success", data = dtFinal });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        } 

        [Authorize(Policy = "RequireAdminRole")]
        // [AllowAnonymous]
        [HttpGet("DownloadMyPrice")]
        public IActionResult DownloadMyPrice(string prd, string sup, string group)
        {
            var dtSup = GetSupplierForPrint(sup);
            var dtPrice = GetMyPriceForPrint(prd, sup, group);
            var data = GeneratePDFMyPrice(dtSup, dtPrice);

            string contentType = "application/pdf";
            //Define the file name
            string fileName = "PPH_MyPrice_" + DateTime.Now + ".pdf";

            //Creates a FileContentResult object by using the file contents, content type, and file name
            return File(data, contentType, fileName);
            // FileStreamResult fileStreamResult = new FileStreamResult(data, contentType);
            // fileStreamResult.FileDownloadName = fileName;  
            // return fileStreamResult;            
        }

        private Supplier GetSupplierForPrint(string sup){
            var dt = Task.Run(() => GetSupplier(sup));
            var dtSup = dt.Result;

            return dtSup;
        }

        private async Task<Supplier> GetSupplier(string sup)
        {
            var dtSup = new Supplier();
            using (_context = new DapperContext("RSUP"))
            {
                _uow = new UnitOfWork(_context);
                var dt = await _uow.PlgRepository.GetSupplier(sup);
                dtSup = dt;
            }
            return dtSup;
        }
        

        private IEnumerable<ItemRequest> GetMyPriceForPrint(string prd, string sup, string group){
            var dt = Task.Run(() => GetMonMyPriceForPrint(prd, sup, group));
            var dtPrice = dt.Result;

            return dtPrice;
        }

        private async Task<IEnumerable<ItemRequest>> GetMonMyPriceForPrint(string prd, string sup, string group){
            var dt = new List<ItemRequest>();
            using(_context = new DapperContext("RSUP")){
                _uow = new UnitOfWork(_context);
                var dtRSUP = await _uow.PlgRepository.GetMonMyPrice(prd, sup, group);
                dt = dtRSUP.ToList();
            }

            using(_context = new DapperContext("PSG")){
                _uow = new UnitOfWork(_context);
                var dtPSG = await _uow.PlgRepository.GetMonMyPrice(prd, sup, group);
                dt.AddRange(dtPSG.ToList());
            }

            var a =  from i in dt
                        let j = new 
                        {
                            ItemID = i.ItemID,
                            //SubCategoryID = i.SubCategoryID,
                            UOMName = i.UOMName,
                            Qnty = i.Qnty,
                            //QtyRt = i.QtyRt,
                            Currency = i.Currency,
                            Harga = i.Harga,
                            //ValidUntil = i.ValidUntil,
                            DeliveryDate = i.DeliveryDate,
                            //ImportBy = i.ImportBy,
                            //Periode = i.Periode,
                            Remark = i.Remark,
                            NewItemID = i.NewItemID
                        }
                        group i by j into k
                        select new
                        {
                            ItemID = k.Key.ItemID,
                            //SubCategoryID = k.Key.SubCategoryID,
                            UOMName = k.Key.UOMName,
                            Qnty = k.Key.Qnty,
                            //QtyRt = k.Key.QtyRt,
                            Currency = k.Key.Currency,
                            Harga = k.Key.Harga,
                            //ValidUntil = k.Key.ValidUntil,
                            DeliveryDate = k.Key.DeliveryDate,
                            //ImportBy = k.Key.ImportBy,
                            //Periode = k.Key.Periode,
                            Remark = k.Key.Remark,
                            NewItemID = k.Key.NewItemID                                
                        };
            
            var dtUnion = a.ToList();
            var dtFinal = new List<ItemRequest>();

            foreach(var i in dtUnion){
                var item = new ItemRequest();
                var dtFind = dt.Where(x => x.ItemID == i.ItemID && x.NewItemID == i.NewItemID).FirstOrDefault();

                item.ItemID = i.ItemID;
                //item.SubCategoryID = i.SubCategoryID;
                item.ItemName = dtFind.ItemName;
                item.ItemDesc = dtFind.ItemDesc;
                item.UOMName = i.UOMName;
                item.Qnty = i.Qnty;
                //item.QtyRt = i.QtyRt;
                item.Currency = i.Currency;
                item.Harga = i.Harga;
                //item.ValidUntil = i.ValidUntil;
                item.DeliveryDate = i.DeliveryDate;
                //item.ImportBy = i.ImportBy;
                item.IDInput = dtFind.IDInput;
                //item.Periode = i.Periode;
                item.Remark = i.Remark;
                item.NewItemID = i.NewItemID;
                
                dtFinal.Add(item);
            }
            return dtFinal;
        }        

        private Stream GeneratePDFMyPrice(Supplier dtSup, IEnumerable<ItemRequest> dtPrice)
        {
            CultureInfo culture = new CultureInfo("id-ID");

            //Create a new PDF document.
            PdfDocument pdfDocument = new PdfDocument();

            //Add a page to the PDF document
            PdfPage pdfPage = pdfDocument.Pages.Add();
            PdfGraphics graphics = pdfPage.Graphics;

            //Create a header and draw the image.
            RectangleF boundsHeader = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150); // Batas Bounds Header sebanyak 150
            RectangleF boundsFooter = new RectangleF(0, 0, pdfDocument.Pages[0].GetClientSize().Width, 150); // Batas Bounds Header sebanyak 150

            PdfPageTemplateElement header = new PdfPageTemplateElement(boundsHeader);

            PdfFont fontHeader = new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Underline|PdfFontStyle.Bold);
            PdfFont fontHeaderDetail = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            //Load the PDF document
            FileStream imageStream = new FileStream("wwwroot/Logo/RSUP.png", FileMode.Open, FileAccess.Read);
            PdfImage image = new PdfBitmap(imageStream);

            //Draw the header.
            // ini Header Kiri
            header.Graphics.DrawImage(image, new PointF(0, 0), new SizeF(50, 50));
            header.Graphics.DrawString("QUOTATION", fontHeader, PdfBrushes.Black, new Syncfusion.Drawing.PointF(215, 20));
            header.Graphics.DrawString("PT. Riau Sakti United Plantations", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 55));
            header.Graphics.DrawString("Pulau Burung", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 65));
            header.Graphics.DrawString("Phone : (0779) 54188, Fax : (0779) 541000", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 75));
            header.Graphics.DrawString("Email : p2-purchasing@rsup.co.id", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 85));

            // header.Graphics.DrawString("PPH No", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 105));
            // header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(60, 105));
            // header.Graphics.DrawString(dt.PPHNo, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(70, 105));

            // header.Graphics.DrawString("PPH Date", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(0, 115));
            // header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(60, 115));
            // header.Graphics.DrawString(dt.TransDate.ToString("dd/MM/yyyy"), fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(70, 115));

            // // Ini Header Kanan (Y = 250)
            header.Graphics.DrawString("Supplier", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 55));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 55));
            header.Graphics.DrawString(dtSup.SupplierName, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 55));

            header.Graphics.DrawString("Contact P ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 65));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 65));
            header.Graphics.DrawString(dtSup.ContactPerson1 == null ? "" : dtSup.ContactPerson1, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 65));

            header.Graphics.DrawString("Address ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 75));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 75));
            header.Graphics.DrawString(dtSup.Address1 == null ? "" : dtSup.Address1, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 75));

            header.Graphics.DrawString("Country ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 85));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 85));
            header.Graphics.DrawString(dtSup.CountryName == null ? "" : dtSup.CountryName, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 85));

            header.Graphics.DrawString("Phone ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 95));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 95));
            header.Graphics.DrawString(dtSup.Telephone == null ? "" : dtSup.Telephone, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 95));

            header.Graphics.DrawString("Fax ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 105));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 105));
            header.Graphics.DrawString(dtSup.Fax == null ? "" : dtSup.Fax, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 105));

            header.Graphics.DrawString("Email ", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(250, 115));
            header.Graphics.DrawString(":", fontHeaderDetail, PdfBrushes.Black, new Syncfusion.Drawing.PointF(305, 115));
            header.Graphics.DrawString(dtSup.Email == null ? "" : dtSup.Email, fontHeaderDetail, PdfBrushes.Green, new Syncfusion.Drawing.PointF(320, 115));

            //Add the header at the top.
            pdfDocument.Template.Top = header;

            //================================================================
            PdfGrid pdfGrid = new PdfGrid();
            //Create a DataTable
            DataTable dataTable = new DataTable();
            //Add columns to the DataTable
            dataTable.Columns.Add("No");
            dataTable.Columns.Add("Item Name");
            dataTable.Columns.Add("UOM");
            dataTable.Columns.Add("Quantity");
            dataTable.Columns.Add("Currency");
            dataTable.Columns.Add("Unit Price");
            dataTable.Columns.Add("Delivery Date");
            dataTable.Columns.Add("Remark");

            int no = 1;
            foreach (var dt in dtPrice)
            {
                var deliveryDate = dt.DeliveryDate != null ? dt.DeliveryDate.Value.ToString("dd/MM/yyy") : "";
                var data = new object[] { no, dt.ItemName + "\n" + dt.ItemDesc, dt.UOMName, Math.Round(dt.Qnty, 2), dt.Currency, String.Format("{0:N}", dt.Harga), deliveryDate, dt.Remark };
                dataTable.Rows.Add(data);
                no++;
            }

            //Assign data source
            pdfGrid.DataSource = dataTable;

            //Draw grid to the page of PDF document
            PdfStringFormat formatColumnNo = new PdfStringFormat(); formatColumnNo.Alignment = PdfTextAlignment.Center; formatColumnNo.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[0].Width = 20; pdfGrid.Columns[0].Format = formatColumnNo;
            PdfStringFormat formatColumnItemName = new PdfStringFormat(); formatColumnItemName.Alignment = PdfTextAlignment.Left; formatColumnItemName.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[1].Width = 150; pdfGrid.Columns[1].Format = formatColumnItemName;
            PdfStringFormat formatColumnUOMName = new PdfStringFormat(); formatColumnUOMName.Alignment = PdfTextAlignment.Left; formatColumnUOMName.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[2].Width = 40; pdfGrid.Columns[2].Format = formatColumnUOMName;
            PdfStringFormat formatColumnQuantity = new PdfStringFormat(); formatColumnQuantity.Alignment = PdfTextAlignment.Right; formatColumnQuantity.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[3].Width = 40; pdfGrid.Columns[3].Format = formatColumnQuantity;
            PdfStringFormat formatColumnCurrency = new PdfStringFormat(); formatColumnCurrency.Alignment = PdfTextAlignment.Center; formatColumnCurrency.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[4].Width = 40; pdfGrid.Columns[4].Format = formatColumnCurrency;
            PdfStringFormat formatColumnPrice = new PdfStringFormat(); formatColumnPrice.Alignment = PdfTextAlignment.Right; formatColumnPrice.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[5].Width = 65; pdfGrid.Columns[5].Format = formatColumnPrice;
            PdfStringFormat formatColumnDate = new PdfStringFormat(); formatColumnDate.Alignment = PdfTextAlignment.Center; formatColumnDate.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[6].Width = 55; pdfGrid.Columns[6].Format = formatColumnDate;
            PdfStringFormat formatColumnRemark = new PdfStringFormat(); formatColumnRemark.Alignment = PdfTextAlignment.Left; formatColumnRemark.LineAlignment = PdfVerticalAlignment.Middle; pdfGrid.Columns[7].Width = 100; pdfGrid.Columns[7].Format = formatColumnRemark;

            //PdfGridLayoutFormat nty = new PdfGridLayoutFormat();

            pdfGrid.Draw(pdfPage, new PointF(0, 150));


            //=========================================================================


            //Create a Page template that can be used as footer.
            PdfPageTemplateElement footer = new PdfPageTemplateElement(boundsFooter);
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 7);
            PdfBrush brush = new PdfSolidBrush(Color.Black);

            //Create page number field.
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, brush);

            //Create page count field.
            PdfPageCountField count = new PdfPageCountField(font, brush);

            //Add the fields in composite fields.
            PdfCompositeField compositeField = new PdfCompositeField(font, brush, "Page {0} of {1}", pageNumber, count);
            PdfCompositeField printedDate = new PdfCompositeField(font, brush, "Printed On : " + DateTime.Now);
            compositeField.Bounds = footer.Bounds;
            printedDate.Bounds = footer.Bounds;

            //Draw the composite field in footer.
            compositeField.Draw(footer.Graphics, new PointF(0, 140));
            printedDate.Draw(footer.Graphics, new PointF(410, 140));

            //Add the footer template at the bottom.
            // pdfDocument.Template.Bottom = footer;

            //Save the document into stream
            MemoryStream stream = new MemoryStream();
            pdfDocument.Save(stream);
            stream.Position = 0;

            //Closes the document
            pdfDocument.Close(true);
            string fileName = "PPH " + DateTime.Now;

            return stream;

            //Creates a FileContentResult object by using the file contents, content type, and file name
            //return File(stream, contentType, fileName);
            // return new FileStreamResult(stream, contentType)
            // {
            //     FileDownloadName = fileName
            // };
        }

        // static bool taskCompleted = false;
 
        // public ActionResult GeneratePDF()
        // {
        //     taskCompleted = false;
        //     System.Threading.Thread.Sleep(5000);
        //     //Create a new PdfDocument
        //     PdfDocument document = new PdfDocument();
        //     //Add a page to the document
        //     PdfPage page = document.Pages.Add();
        //     //Create Pdf graphics for the page
        //     PdfGraphics graphics = page.Graphics;
        //     //Create a solid brush
        //     PdfBrush brush = new PdfSolidBrush(Color.Black);
        //     //Set the font
        //     PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20f);
        //     //Draw the text
        //     graphics.DrawString("Hello world!", font, brush, new PointF(20, 20));
        //     taskCompleted = true;
        //     //Export the document after saving
        //     return document.ExportAsActionResult("output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
        // }
                
        // //Get the status of PDF generation
        // public bool GetStatus()
        // {
        //     return taskCompleted;
        // }

        // ============================= End Monitoring ================================ 





        // ============================= Utility ================================
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetGroupAccess")]
        public async Task<IActionResult> GetGroupAccess()
        {
            try
            {
                var datas = new List<GroupAccess>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var data = await _uow.PlgRepository.GetGroupAccess();
                    datas = data.ToList();
                }

                return Ok(new { st = "Success", data = datas });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetGroupAccessByID")]
        public async Task<IActionResult> GetGroupAccessByID(string id)
        {
            try
            {
                var datas = new GroupAccess();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    datas = await _uow.PlgRepository.GetGroupAccessByID(id);
                }

                return Ok(new { st = "Success", data = datas });
            }
            catch (System.Exception e)
            {

                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("SaveGroupAccess")]
        public async Task<IActionResult> SaveGroupAccess(GroupAccess input)
        {
            try
            {
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.SaveGroupAccess(input);
                }

                return Ok(new { st = "Success" });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("UpdateGroupAccess")]
        public async Task<IActionResult> UpdateGroupAccess(GroupAccess input)
        {
            try
            {
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.UpdateGroupAccess(input);
                }

                return Ok(new { st = "Success" });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("DeleteGroupAccess")]
        public async Task<IActionResult> DeleteGroupAccess(GroupAccess input)
        {
            try
            {
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    await _uow.PlgRepository.DeleteGroupAccess(input);
                }

                return Ok(new { st = "Success" });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { st = "Failed", msg = e.Message });
            }
        }

        [Authorize(Policy = "RequireAcces")]
        [HttpGet("GetAllMenuHdr")]
        public async Task<IActionResult> GetAllMenuHdr()
        {
            try
            {
                var a = new List<MenuHdr>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var b = await _uow.PlgRepository.GetAllMenuHdr();
                    a = b.ToList();
                }
                return Ok(a);

            }
            catch (Exception e)
            {
                return BadRequest(new { msg = e.Message });
            }

        }

        [Authorize(Policy = "RequireAcces")]
        [HttpGet("GetAllMenuDtl")]
        public async Task<IActionResult> GetAllMenuDtl()
        {
            try
            {
                var a = new List<MenuDtl>();
                using (_context = new DapperContext("RSUP"))
                {
                    _uow = new UnitOfWork(_context);
                    var b = await _uow.PlgRepository.GetAllMenuDtl();
                    a = b.ToList();
                }

                return Ok(a);
            }
            catch (Exception e)
            {

                return BadRequest(new { msg = e.Message });
            }

        }
        // ============================= END Utility ================================






        [AllowAnonymous]
        [HttpGet("GetStatusOnline")]
        public IActionResult GetStatusOnline(string group)
        {
            return Ok(new{sip=1});
        }  

    }
}