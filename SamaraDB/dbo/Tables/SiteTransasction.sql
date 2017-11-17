CREATE TABLE [dbo].[SiteTransasction]
(
	[SiteTransID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserID] NVARCHAR(128) NULL, 
    [Tdate] DATE NULL, 
    [SiteID] INT NULL, 
	[ClientID] INT NULL, 
    [SupplierID] INT NULL, 
    [ItemID] INT NULL, 
    [QtyAdded] INT NULL, 
    [QtyRemoved] INT NULL, 
    [ToSiteID] INT NULL, 
    [SBillDetailID] INT NULL, 
    [Remarks] VARCHAR(250) NULL, 
   
    CONSTRAINT [FK_SiteTransasction_Sites] FOREIGN KEY ([SiteID]) REFERENCES [Sites]([SiteID]), 
    CONSTRAINT [FK_SiteTransasction_ToSites] FOREIGN KEY ([ToSiteID]) REFERENCES [Sites]([SiteID]), 
    CONSTRAINT [FK_SiteTransasction_SupplierBillDetail] FOREIGN KEY ([SBillDetailID]) REFERENCES [SupplierBillDetail]([SBillDetailID]), 
    CONSTRAINT [FK_SiteTransasction_Item] FOREIGN KEY ([ItemID]) REFERENCES [Item]([ItemID]),
	CONSTRAINT [FK_SiteTransasction_User] FOREIGN KEY ([UserID]) REFERENCES [AspNetUsers]([Id]),


)
