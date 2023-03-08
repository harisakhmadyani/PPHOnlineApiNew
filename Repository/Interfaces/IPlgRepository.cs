using System.Collections.Generic;
using System.Threading.Tasks;
using newplgapi.model;

namespace newplgapi.Repository.Interfaces
{
    public interface IPlgRepository
    {
        Task<bool> SaveChangePassword(string userId, string oldPassword, string newPassword);
        Task<IEnumerable<MenuHdr>> GetMenuHdr(string group);
        Task<IEnumerable<MenuDtl>> GetMenuDtl(string group);


        Task SaveRoleDateTime(RoleDateTime input);
        Task<DateTimeNow> GetDateTimeNow();
        Task<RoleDateTime> GetLastRoleDateTime0();
        Task<RoleDateTime> GetLastRoleDateTime1();
        Task<RoleDateTime> GetLastRoleDateTime2();
        Task SaveBudgetPeriod(BudgetPeriod input);
        Task<BudgetPeriod> GetLastBudgetPeriod();        
        Task<IEnumerable<SubCategory>> GetSubCategory(string group);
        Task<IEnumerable<Currency>> GetCurrency(string group);
        // Task<IEnumerable<ItemSupplier>> GetMyItem(string sup, string group);
        Task<IEnumerable<ItemSupplier>> GetMyItem(string sup, string group, int pages);
        Task<CountData> GetCountMyItem(string sup);
        Task<IEnumerable<ItemRequest>> GetChooseMyItem(string id, string sup, string group);
        Task SaveMyItem(MyItemInput input);
        Task RemoveMyItem(MyItemInput input);
        Task RefreshItemOpenPrice();
        Task<IEnumerable<ItemRequest>> GetItemOpenPrice(string subCat);


        Task<IEnumerable<ItemRequest>> GetProductItem(string id, string sup, string group);
        // Task<IEnumerable<ItemRequest>> GetMyProductItem(string sup, string group);
        Task<IEnumerable<ItemRequest>> GetMyProductItem(string sup, string group, int pages);
        Task<CountData> GetCountMyProductItem(string sup);
        Task<IEnumerable<ItemRequest>> GetMyPrice(string sup, string group);
        Task<IEnumerable<ItemRequest>> GetNewProductItem(string sup, string group);
        Task SaveData(PriceInput input);
        Task UpdateData(PriceInput input);
        
        
        Task<IEnumerable<ItemRequest>> GetProductItemPrice(string subcat, string group, string factAbbr);
        Task<IEnumerable<ItemRequest>> GetNewProductItemPrice(string factAbbr);
        Task<SupplierSG> GetSupplierSG(string sup);
        Task<SupplierSG> GetSupplierIDPSG(string sup);
        Task<string> SaveToPPH(string item, string supp, string period, string user);
        Task<string> UpdateToNewItem(long id, string user);


        Task<Supplier> GetSupplier(string sup);
        Task<IEnumerable<ItemRequest>> GetMonMyPrice(string prd, string sup, string group);


        Task<IEnumerable<GroupAccess>> GetGroupAccess();
        Task<GroupAccess> GetGroupAccessByID(string groupAccessID);
        Task SaveGroupAccess(GroupAccess input);
        Task UpdateGroupAccess(GroupAccess input);
        Task DeleteGroupAccess(GroupAccess input);
        Task<IEnumerable<MenuHdr>> GetAllMenuHdr();
        Task<IEnumerable<MenuDtl>> GetAllMenuDtl();    
    }
}