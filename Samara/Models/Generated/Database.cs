
// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: `DefaultConnection`
//     Provider:               `System.Data.SqlClient`
//     Connection String:      `Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=Samara;Integrated Security=True`
//     Schema:                 ``
//     Include Views:          `False`

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetaPoco;

namespace Samara
{
	public partial class Repository : Database
	{
		public Repository() 
			: base("DefaultConnection")
		{
			CommonConstruct();
		}

		public Repository(string connectionStringName) 
			: base(connectionStringName)
		{
			CommonConstruct();
		}
		
		partial void CommonConstruct();
		
		public interface IFactory
		{
			Repository GetInstance();
		}
		
		public static IFactory Factory { get; set; }
        public static Repository GetInstance()
        {
			if (_instance!=null)
				return _instance;
				
			if (Factory!=null)
				return Factory.GetInstance();
			else
				return new Repository();
        }

		[ThreadStatic] static Repository _instance;
		
		public override void OnBeginTransaction()
		{
			if (_instance==null)
				_instance=this;
		}
		
		public override void OnEndTransaction()
		{
			if (_instance==this)
				_instance=null;
		}
        
	}
	

    
	[TableName("dbo.__MigrationHistory")]
	[PrimaryKey("MigrationId", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class __MigrationHistory  
    {
		[Column] public string MigrationId { get; set; }
		[Column] public string ContextKey { get; set; }
		[Column] public byte[] Model { get; set; }
		[Column] public string ProductVersion { get; set; }
	}
    
	[TableName("dbo.__RefactorLog")]
	[PrimaryKey("OperationKey", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class __RefactorLog  
    {
		[Column] public Guid OperationKey { get; set; }
	}
    
	[TableName("dbo.AspNetRoles")]
	[PrimaryKey("Id", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class AspNetRole  
    {
		[Column] public string Id { get; set; }
		[Column] public string Name { get; set; }
	}
    
	[TableName("dbo.AspNetUserClaims")]
	[PrimaryKey("Id")]
	[ExplicitColumns]
    public partial class AspNetUserClaim  
    {
		[Column] public int Id { get; set; }
		[Column] public string UserId { get; set; }
		[Column] public string ClaimType { get; set; }
		[Column] public string ClaimValue { get; set; }
	}
    
	[TableName("dbo.AspNetUserLogins")]
	[PrimaryKey("LoginProvider", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class AspNetUserLogin  
    {
		[Column] public string LoginProvider { get; set; }
		[Column] public string ProviderKey { get; set; }
		[Column] public string UserId { get; set; }
	}
    
	[TableName("dbo.AspNetUserRoles")]
	[PrimaryKey("UserId", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class AspNetUserRole  
    {
		[Column] public string UserId { get; set; }
		[Column] public string RoleId { get; set; }
	}
    
	[TableName("dbo.AspNetUsers")]
	[PrimaryKey("Id", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class AspNetUser  
    {
		[Column] public string Id { get; set; }
		[Column] public string Email { get; set; }
		[Column] public bool EmailConfirmed { get; set; }
		[Column] public string PasswordHash { get; set; }
		[Column] public string SecurityStamp { get; set; }
		[Column] public string PhoneNumber { get; set; }
		[Column] public bool PhoneNumberConfirmed { get; set; }
		[Column] public bool TwoFactorEnabled { get; set; }
		[Column] public DateTime? LockoutEndDateUtc { get; set; }
		[Column] public bool LockoutEnabled { get; set; }
		[Column] public int AccessFailedCount { get; set; }
		[Column] public string UserName { get; set; }
		[Column] public DateTime DateCreated { get; set; }
		[Column] public bool Disabled { get; set; }
		[Column] public DateTime? LastLogin { get; set; }
	}
    
	[TableName("dbo.Client")]
	[PrimaryKey("ClientId")]
	[ExplicitColumns]
    public partial class Client  
    {
		[Column] public int ClientId { get; set; }
		[Column] public string ClientName { get; set; }
		[Column] public string Address { get; set; }
	}
    
	[TableName("dbo.ClientBill")]
	[PrimaryKey("CBillID")]
	[ExplicitColumns]
    public partial class ClientBill  
    {
		[Column] public int CBillID { get; set; }
		[Column] public int? ClientID { get; set; }
		[Column] public DateTime? Tdate { get; set; }
		[Column] public decimal? RetentionPerc { get; set; }
	}
    
	[TableName("dbo.ClientBillDetail")]
	[PrimaryKey("CBillDetailID", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class ClientBillDetail  
    {
		[Column] public int CBillDetailID { get; set; }
		[Column] public int? CBillID { get; set; }
		[Column] public int? ItemID { get; set; }
		[Column] public int? Qty { get; set; }
		[Column] public decimal? UnitCostPrice { get; set; }
		[Column] public decimal? UnitSellPrice { get; set; }
		[Column] public decimal? TaxPerc { get; set; }
	}
    
	[TableName("dbo.ClientViewBillDetail")]
	[PrimaryKey("ClientViewBillDetailID", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class ClientViewBillDetail  
    {
		[Column] public int ClientViewBillDetailID { get; set; }
		[Column] public int? CBillID { get; set; }
		[Column] public string Description { get; set; }
		[Column] public decimal? Amount { get; set; }
		[Column] public bool? DebitCredit { get; set; }
		[Column] public string BeforeTax { get; set; }
	}
    
	[TableName("dbo.Config")]
	[PrimaryKey("ConfigID")]
	[ExplicitColumns]
    public partial class Config  
    {
		[Column] public int ConfigID { get; set; }
		[Column] public string PANnumber { get; set; }
		[Column] public string TANnumber { get; set; }
		[Column] public int RowsPerPage { get; set; }
	}
    
	[TableName("dbo.Item")]
	[PrimaryKey("ItemID")]
	[ExplicitColumns]
    public partial class Item  
    {
		[Column] public int ItemID { get; set; }
		[Column] public string ItemName { get; set; }
		[Column] public int? UnitID { get; set; }
		[Column] public int? ReorderLevel { get; set; }
		[Column] public decimal? TaxPerc { get; set; }
	}
    
	[TableName("dbo.Project")]
	[PrimaryKey("ProjectID")]
	[ExplicitColumns]
    public partial class Project  
    {
		[Column] public int ProjectID { get; set; }
		[Column] public string Name { get; set; }
	}
    
	[TableName("dbo.SiteCurrentStock")]
	[PrimaryKey("SiteStockID")]
	[ExplicitColumns]
    public partial class SiteCurrentStock  
    {
		[Column] public int SiteStockID { get; set; }
		[Column] public int? SiteID { get; set; }
		[Column] public int? ItemID { get; set; }
		[Column] public int? Qty { get; set; }
	}
    
	[TableName("dbo.Sites")]
	[PrimaryKey("SiteID")]
	[ExplicitColumns]
    public partial class Site  
    {
		[Column] public int SiteID { get; set; }
		[Column] public int? ProjectID { get; set; }
		[Column] public string SiteName { get; set; }
	}
    
	[TableName("dbo.SiteTransasction")]
	[PrimaryKey("SiteTransID")]
	[ExplicitColumns]
    public partial class SiteTransasction  
    {
		[Column] public int SiteTransID { get; set; }
		[Column] public int? UserID { get; set; }
		[Column] public DateTime? Tdate { get; set; }
		[Column] public int? SiteID { get; set; }
		[Column] public int? ItemID { get; set; }
		[Column] public int? QtyAdded { get; set; }
		[Column] public int? QtyRemoved { get; set; }
		[Column] public int? ToSiteID { get; set; }
		[Column] public int? SBillDetailID { get; set; }
		[Column] public string Remarks { get; set; }
	}
    
	[TableName("dbo.StockSummary")]
	[PrimaryKey("StockSummaryID", AutoIncrement=false)]
	[ExplicitColumns]
    public partial class StockSummary  
    {
		[Column] public int StockSummaryID { get; set; }
		[Column] public DateTime? Tdate { get; set; }
		[Column] public int? ItemID { get; set; }
		[Column] public int? SiteID { get; set; }
		[Column] public int? Qty { get; set; }
	}
    
	[TableName("dbo.Supervisor")]
	[PrimaryKey("UserID")]
	[ExplicitColumns]
    public partial class Supervisor  
    {
		[Column] public int UserID { get; set; }
		[Column] public string Name { get; set; }
	}
    
	[TableName("dbo.SupervisorSites")]
	[ExplicitColumns]
    public partial class SupervisorSite  
    {
		[Column] public int UserID { get; set; }
		[Column] public int? SiteID { get; set; }
	}
    
	[TableName("dbo.Supplier")]
	[PrimaryKey("SupplierID")]
	[ExplicitColumns]
    public partial class Supplier  
    {
		[Column] public int SupplierID { get; set; }
		[Column] public string SupplierName { get; set; }
	}
    
	[TableName("dbo.SupplierBill")]
	[PrimaryKey("SBillID")]
	[ExplicitColumns]
    public partial class SupplierBill  
    {
		[Column] public int SBillID { get; set; }
		[Column] public int? SupplierID { get; set; }
		[Column] public DateTime? Tdate { get; set; }
	}
    
	[TableName("dbo.SupplierBillDetail")]
	[PrimaryKey("SBillDetailID")]
	[ExplicitColumns]
    public partial class SupplierBillDetail  
    {
		[Column] public int SBillDetailID { get; set; }
		[Column] public int? SBillID { get; set; }
		[Column] public int? ItemID { get; set; }
		[Column] public int? Qty { get; set; }
		[Column] public decimal? UnitPrice { get; set; }
		[Column] public int? QtyRec { get; set; }
		[Column] public int? QtySold { get; set; }
	}
    
	[TableName("dbo.Units")]
	[PrimaryKey("UnitID")]
	[ExplicitColumns]
    public partial class Unit  
    {
		[Column] public int UnitID { get; set; }
		[Column] public string UnitName { get; set; }
	}
}
