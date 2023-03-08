using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using newplgapi.model;
using newplgapi.Repository.Interfaces;

namespace newplgapi.Repository.Implements
{
    internal class PlgRepository : IPlgRepository
    {
        private IDapperContext _context;

        public PlgRepository(IDapperContext context)
        {
            _context = context;
        }
        
        // ================= AUTH ==============================
        public async Task<bool> SaveChangePassword(string userId, string oldPassword, string newPassword)
        {
            try
            {

                User user = await _context.db.QueryFirstOrDefaultAsync<User>("SELECT * FROM plg_tblMstUser WHERE userId=@userId", (object)new
                {
                    userId = userId
                });
                if (user == null || !this.VerifyPasswordHash(oldPassword, user.passwordHash, user.passwordSalt))
                    return false;

                byte[] passwordHash;
                byte[] passwordSalt;
                this.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
                
                User users = new User()
                {
                    userId = userId,
                    passwordHash = passwordHash,
                    passwordSalt = passwordSalt
                };
                
                IEnumerable<object> objects = await _context.db.QueryAsync("UPDATE plg_tblMstUser SET PasswordHash = @hash , PasswordSalt = @salt WHERE userId=@userId", (object)new
                {
                    hash = passwordHash,
                    salt = passwordSalt,
                    userId = userId
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CreatePasswordHash(string newPassword, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA512 hmacshA512 = new HMACSHA512())
            {
                passwordSalt = hmacshA512.Key;
                passwordHash = hmacshA512.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            }
        }

        private bool VerifyPasswordHash(string oldPassword, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512 hmacshA512 = new HMACSHA512(passwordSalt))
            {
                byte[] hash = hmacshA512.ComputeHash(Encoding.UTF8.GetBytes(oldPassword));
                for (int index = 0; index < hash.Length; ++index)
                {
                if ((int) hash[index] != (int) passwordHash[index])
                    return false;
                }
            }
            return true;
        }

        public async Task<IEnumerable<MenuDtl>> GetMenuDtl(string group)
        {
            return await _context.db.QueryAsync<MenuDtl>("SELECT  A.* From plg_tblMenuDtl A INNER JOIN plg_tblMenuAccess B ON B.menuid = A.id WHERE B.groupid = @group ORDER BY A.id ASC", new {group = group});
        }

        public async Task<IEnumerable<MenuHdr>> GetMenuHdr(string group)
        {
             return await _context.db.QueryAsync<MenuHdr>("SELECT  A.* From plg_tblMenuHdr A INNER JOIN plg_tblMenuAccess B ON B.menuid = A.id WHERE B.groupid = @group ORDER BY A.id ASC", new {group = group});
        }
        // ================ END ===============================




        
        // ==================== Master =====================
        public async Task SaveRoleDateTime(RoleDateTime input)
        {
            await _context.db.InsertAsync<RoleDateTime>(input);
        }
        
        public async Task<DateTimeNow> GetDateTimeNow()
        {
            return await _context.db.QueryFirstOrDefaultAsync<DateTimeNow>("SELECT GETDATE() AS dateTimeNow");
        }
        
        public async Task<RoleDateTime> GetLastRoleDateTime0()
        {
            return await _context.db.QueryFirstOrDefaultAsync<RoleDateTime>("SELECT TOP 1 * FROM plg_tblMstRoleDateTime ORDER BY ID DESC");
        }

        public async Task<RoleDateTime> GetLastRoleDateTime1()
        {
            return await _context.db.QueryFirstOrDefaultAsync<RoleDateTime>("SELECT TOP 1 * FROM plg_tblMstRoleDateTime ORDER BY ID DESC");
        }

        public async Task<RoleDateTime> GetLastRoleDateTime2()
        {
            return await _context.db.QueryFirstOrDefaultAsync<RoleDateTime>("SELECT TOP 1 * FROM plg_tblMstRoleDateTime ORDER BY ID DESC");
        }

        public async Task SaveBudgetPeriod(BudgetPeriod input)
        {
            await _context.db.InsertAsync<BudgetPeriod>(input);
        }

        public async Task<BudgetPeriod> GetLastBudgetPeriod()
        {
            return await _context.db.QueryFirstOrDefaultAsync<BudgetPeriod>("SELECT TOP 1 * FROM plg_tblMstBudgetUsing ORDER BY ID DESC");
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategory(string group)
        {
            // return await _context.db.QueryAsync<SubCategory>("SELECT SubCategoryID, SubCategoryName FROM tblMstItemSubCategory WHERE CategoryID NOT IN (5, 10, 12, 13, 14, 15, 16, 114, 119, 120, 121, 118, 117, 115, 7) AND SubCategoryID NOT IN (87, 255, 80, 253) ORDER BY SubCategoryName ASC");
            return await _context.db.QueryAsync<SubCategory>("api_plggetmstitemsubcategoryaccess", new {GroupAccess = group}, commandType: CommandType.StoredProcedure);
        }
        
        public async Task<IEnumerable<Currency>> GetCurrency(string group)
        {
            return await _context.db.QueryAsync<Currency>("SELECT * From plg_tblMstCurrencyAccess WHERE GroupAccess = @group", new {group = group});
        }

        // public async Task<IEnumerable<ItemSupplier>> GetMyItem(string sup, string group)
        // {
        //     return await _context.db.QueryAsync<ItemSupplier>("api_plggetmyitem_New", new {SupID = sup, GroupAccess = group}, commandType: CommandType.StoredProcedure);
        // }

        public async Task<IEnumerable<ItemSupplier>> GetMyItem(string sup, string group, int pages)
        {
            return await _context.db.QueryAsync<ItemSupplier>("api_plggetmyitem_New_Paging", new {SupID = sup, GroupAccess = group, Pages = pages}, commandType: CommandType.StoredProcedure);
        }

        public async Task<CountData> GetCountMyItem(string sup)
        {
            return await _context.db.QueryFirstOrDefaultAsync<CountData>("api_plggetcountmyitem_New_Paging_P1P2", new {SupID = sup}, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ItemRequest>> GetChooseMyItem(string id, string sup, string group)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetchoosemyitem_New", new { SubCategory = id, SupID = sup, GroupAccess = group }, commandType: CommandType.StoredProcedure);
        }

        public async Task SaveMyItem(MyItemInput input)
        {
            // await _context.db.InsertAsync<MyItemInput>(input);
            try
            {
                _context.BeginTransaction();               
                await _context.db.QueryFirstAsync<string>("api_plgsavemyitem_New", 
                    new {SupplierID = input.SupplierID, 
                        ItemID = input.ItemID,
                        NewItemID = input.NewItemID }, 
                commandType:CommandType.StoredProcedure, transaction:_context.transaction);     
                _context.Commit();
            }
            catch (System.Exception)
            {
                _context.Rollback();
                throw;
            }     
        }

        public async Task RemoveMyItem(MyItemInput input)
        {
            // if(input.ItemID != "0"){
            //     await _context.db.QueryAsync("DELETE FROM plg_tblMstItemSupplier WHERE ItemID = @itemID AND SupplierID = @supplierID",
            //                                 new { itemID = input.ItemID, supplierID = input.SupplierID  });
            // } else
            // {
            //     await _context.db.QueryAsync("DELETE FROM plg_tblMstItemSupplier WHERE NewItemID = @newItemID AND SupplierID = @supplierID",
            //                                 new { newItemID = input.NewItemID, supplierID = input.SupplierID  });
            // }
            try
            {
                _context.BeginTransaction();               
                await _context.db.QueryFirstAsync<string>("api_plgremovemyitem_New", 
                    new {SupplierID = input.SupplierID, 
                        ItemID = input.ItemID,
                        NewItemID = input.NewItemID }, 
                commandType:CommandType.StoredProcedure, transaction:_context.transaction);     
                _context.Commit();
            }
            catch (System.Exception)
            {
                _context.Rollback();
                throw;
            }    
        }

        public async Task RefreshItemOpenPrice()
        {
            try
            {
                _context.BeginTransaction();               
                await _context.db.QueryFirstAsync<string>("api_plgimporttoplgmstitem_New", new {}, commandType:CommandType.StoredProcedure, transaction:_context.transaction);     
                _context.Commit();
            }
            catch (System.Exception)
            {
                _context.Rollback();
                throw;
            }     
        }

        public async Task<IEnumerable<ItemRequest>> GetItemOpenPrice(string subCat)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetitemopenprice_New", new { SubCategory = subCat}, commandType: CommandType.StoredProcedure);
        }
        // ===================================== END MASTER =============================




        
        // ============================= Transaksi Supplier ================================
        public async Task<IEnumerable<ItemRequest>> GetProductItem(string id, string sup, string group)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetproductitem_New", new { SubCategory = id, SupID = sup, GroupAccess = group }, commandType: CommandType.StoredProcedure);
        }

        // public async Task<IEnumerable<ItemRequest>> GetMyProductItem(string sup, string group)
        // {
        //     return await _context.db.QueryAsync<ItemRequest>("api_plggetmyproductitem_New", new { SupID = sup, GroupAccess = group }, commandType: CommandType.StoredProcedure);
        // }

        public async Task<IEnumerable<ItemRequest>> GetMyProductItem(string sup, string group, int pages)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetmyproductitem_New_Paging", new { SupID = sup, GroupAccess = group, Pages = pages }, commandType: CommandType.StoredProcedure);
        }

        public async Task<CountData> GetCountMyProductItem(string sup)
        {
            return await _context.db.QueryFirstOrDefaultAsync<CountData>("api_plggetcountmyproductitem_New_Paging_P1P2", new {SupID = sup}, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ItemRequest>> GetMyPrice(string sup, string group)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetmyprice_New", new { SupID = sup, GroupAccess = group }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ItemRequest>> GetNewProductItem(string sup, string group)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetnewproductitem_New5", new {SupID = sup, GroupAccess = group }, commandType: CommandType.StoredProcedure);
        }       

        public async Task SaveData(PriceInput input)
        {
            try
            {
                _context.BeginTransaction();               
                await _context.db.QueryFirstAsync<string>("api_plgsavesupplierprice_New5", 
                    new {SupplierID = input.SupplierId, 
                        ItemID = input.ItemId, 
                        Currency = input.Currency, 
                        Price = input.Harga, 
                        ValidUntil = input.ValidUntil,
                        DeliveryDate = input.DeliveryDate,
                        Period = input.Periode,
                        Remark = input.Remark,
                        CreatedBy = input.CreatedBy,
                        Qnty = input.Qnty,
                        QtyRt = input.QtyRt,
                        NewItemID = input.NewItemID,
                        Factory = input.Factory,
                        Filename = input.FileName }, 
                commandType:CommandType.StoredProcedure, transaction:_context.transaction);     
                _context.Commit();
            }
            catch (System.Exception)
            {
                _context.Rollback();
                throw;
            }     
            // var itemID = input.ItemId;
            // var supplierID = input.SupplierId;
            // var period = input.Periode;

            // PriceInput price = await _context.db.QueryFirstOrDefaultAsync<PriceInput>("Select * FROM plg_tblTrnInputHarga WHERE ItemId = @itemID AND SupplierId = @supplierID AND Periode = @period", (object)new
            // {
            //     itemID = itemID,
            //     supplierID = supplierID,
            //     period = period
            // });

            // if (price == null)
                // await _context.db.InsertAsync<PriceInput>(input);
            // else
            //     await _context.db.QueryAsync("UPDATE plg_tblTrnInputHarga SET Harga = @Harga, Currency = @Curr, ValidUntil = @Validi, DeliveryDate = @Deliv, Remark = @Rem, UpdatedBy = @by, UpdatedDate = @nows WHERE ItemId = @itemID AND SupplierId = @supplierID AND Periode = @period",
            //                                     new { Harga = input.Harga, Curr = input.Currency, Validi = input.ValidUntil, Deliv = input.DeliveryDate , Rem = input.Remark, Id = input.Id, by = input.CreatedBy, nows = input.CreatedDate, itemID = itemID, supplierID = supplierID, period = period }
            //                                     );
        }

        public async Task UpdateData(PriceInput input)
        {
            try
            {
                _context.BeginTransaction();               
                await _context.db.QueryFirstAsync<string>("api_plgupdatesupplierprice_New5", 
                    new {SupplierID = input.SupplierId, 
                        ItemID = input.ItemId, 
                        Currency = input.Currency, 
                        Price = input.Harga, 
                        ValidUntil = input.ValidUntil,
                        DeliveryDate = input.DeliveryDate,
                        Period = input.Periode,
                        Remark = input.Remark,
                        UpdatedBy = input.UpdatedBy,
                        NewItemID = input.NewItemID,
                        Filename = input.FileName }, 
                commandType:CommandType.StoredProcedure, transaction:_context.transaction);     
                _context.Commit();
            }
            catch (System.Exception)
            {
                _context.Rollback();
                throw;
            }  
            // await _context.db.QueryAsync("UPDATE plg_tblTrnInputHarga SET Harga = @Harga, Currency = @Curr, ValidUntil = @Valid, DeliveryDate = @Deliv, Remark = @Rem, UpdatedBy = @by, UpdatedDate = @nows WHERE ItemId = @Item AND NewItemID = @NewItem AND Periode = @Period AND SupplierId = @Supp",
            //                                 new { 
            //                                     Harga = input.Harga, 
            //                                     Curr = input.Currency, 
            //                                     Valid = input.ValidUntil, 
            //                                     Deliv = input.DeliveryDate , 
            //                                     Rem = input.Remark, 
            //                                     Id = input.Id, 
            //                                     by = input.UpdatedBy, 
            //                                     nows = input.UpdatedDate, 
            //                                     Item = input.ItemId, 
            //                                     NewItem = input.NewItemID, 
            //                                     Period = input.Periode, 
            //                                     Supp = input.SupplierId });
        }
        // ============================= End Transaksi Supplier ================================





        // ============================= Transaksi Purchaser ================================
        public async Task<IEnumerable<ItemRequest>> GetProductItemPrice(string subcat, string group, string factAbbr)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetsupplierprice_New2", new { SubCategory = subcat, FactAbbr = factAbbr, GroupAccess = group }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ItemRequest>> GetNewProductItemPrice(string factAbbr)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetsupplierpricenewitem_New", new {FactAbbr = factAbbr}, commandType: CommandType.StoredProcedure);
        }

        public async Task<SupplierSG> GetSupplierSG(string sup)
        {
            return await _context.db.QueryFirstOrDefaultAsync<SupplierSG>("SELECT SupplierID, kode_supplier_sg FROM tblMstSupplier WHERE SupplierID = @sup", (object)new
                {
                    sup = sup
                });
        }

        public async Task<SupplierSG> GetSupplierIDPSG(string sup)
        {
            return await _context.db.QueryFirstOrDefaultAsync<SupplierSG>("SELECT SupplierID, kode_supplier_sg FROM tblMstSupplier WHERE kode_supplier_sg = @sup", (object)new
                {
                    sup = sup
                });
        }

        public async Task<string> SaveToPPH(string item, string supp, string period, string user)
        {
            try
            {
                _context.BeginTransaction();               
                var data = await _context.db.QueryFirstAsync<string>("api_plgimporttopph_New2023", new {ItemID = item, SupplierID = supp, Period = period, UserID = user}, commandType:CommandType.StoredProcedure, transaction:_context.transaction);     
                _context.Commit();
                return data;
            }
            catch (System.Exception)
            {
                _context.Rollback();
                throw;
                 // TODO
            }           
        }

        public async Task<string> UpdateToNewItem(long id, string user)
        {
            try
            {
                _context.BeginTransaction();               
                var data = await _context.db.QueryFirstAsync<string>("api_plgupdatetonewitem_New5", new {ID = id, UserID = user}, commandType:CommandType.StoredProcedure, transaction:_context.transaction);     
                _context.Commit();
                return data;
            }
            catch (System.Exception)
            {
                _context.Rollback();
                throw;
                 // TODO
            }           
        }             
        // ============================= End Transaksi Purchaser ================================





        // ============================= Monitoring ================================
        public async Task<Supplier> GetSupplier(string sup)
        {
            return await _context.db.QueryFirstOrDefaultAsync<Supplier>("SELECT A.*, B.CountryName From tblMstSupplier A INNER JOIN tblMstCountry B ON (A.CountryID = B.CountryID) WHERE SupplierID = @sup", new {sup = sup});
        }

        public async Task<IEnumerable<ItemRequest>> GetMonMyPrice(string prd, string sup, string group)
        {
            return await _context.db.QueryAsync<ItemRequest>("api_plggetmonmyprice_New", new { Period = prd, SupID = sup, GroupAccess = group }, commandType: CommandType.StoredProcedure);
        } 
        // ============================= End Monitoring ================================


        // ============================= Utility ================================
        public async Task<IEnumerable<GroupAccess>> GetGroupAccess()
        {
            return await _context.db.QueryAsync<GroupAccess>("SELECT * FROM plg_tblMstGroupAccess WHERE DeletedStatus = 0");
        }

        public async Task<GroupAccess> GetGroupAccessByID(string groupAccessID)
        {
            return await _context.db.QueryFirstOrDefaultAsync<GroupAccess>("SELECT * FROM plg_tblMstGroupAccess WHERE GroupAccessID = @groupAccessID", new {groupAccessID = groupAccessID});
        }

        public async Task SaveGroupAccess(GroupAccess input)
        {
            await _context.db.InsertAsync<GroupAccess>(input);
        }

        public async Task UpdateGroupAccess(GroupAccess input)
        {
            await _context.db.QueryAsync("UPDATE plg_TblMstGroupAccess SET GroupAccessName = @groupAccessName, UpdatedBy = @updatedBy, UpdatedDate = @updatedDate WHERE GroupAccessID = @groupAccessID",
                                                new { groupAccessID = input.GroupAccessID, groupAccessName = input.GroupAccessName, updatedBy = input.UpdatedBy, updatedDate = input.UpdatedDate}
                                                );
        }

        public async Task DeleteGroupAccess(GroupAccess input)
        {
            await _context.db.QueryAsync("UPDATE plg_TblMstGroupAccess SET DeletedStatus = 1, DeletedBy = @deletedBy, DeletedDate = @deletedDate WHERE GroupAccessID = @groupAccessID",
                                                new { groupAccessID = input.GroupAccessID, deletedBy = input.DeletedBy, deletedDate = input.DeletedDate}
                                                );
        }

        public async Task<IEnumerable<MenuHdr>> GetAllMenuHdr()
        {
             return await _context.db.QueryAsync<MenuHdr>("SELECT * From plg_tblMenuHdr ORDER BY id ASC");
        }

        public async Task<IEnumerable<MenuDtl>> GetAllMenuDtl()
        {
            return await _context.db.QueryAsync<MenuDtl>("SELECT  * From plg_tblMenuDtl ORDER BY id ASC");
        }
        // ============================= END Utility ================================          
    }
}