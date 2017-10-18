CREATE TABLE [dbo].[ClientBillDetail]
(
	[CBillDetailID] INT NOT NULL PRIMARY KEY, 
    [CBillID] INT NULL, 
    [ItemID] INT NULL, 
    [Qty] INT NULL, 
    [UnitCostPrice] DECIMAL(18, 2) NULL, 
    [UnitSellPrice] DECIMAL(18, 2) NULL, 
    [TaxPerc] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_ClientBillDetail_ClientBill] FOREIGN KEY ([CBillID]) REFERENCES [ClientBill]([CBillID]), 
    CONSTRAINT [FK_ClientBillDetail_Item] FOREIGN KEY ([ItemID]) REFERENCES [Item]([ItemID])
)
