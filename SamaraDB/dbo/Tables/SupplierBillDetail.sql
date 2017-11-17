CREATE TABLE [dbo].[SupplierBillDetail]
(
	[SBillDetailID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SBillID] INT NULL, 
    [LabourID] INT NULL,
   
    [Qty] INT NULL, 
    [UnitPrice] DECIMAL(18, 2) NULL, 
    [QtyRec] INT NULL, 
    [QtySold] INT NULL, 
    CONSTRAINT [FK_SupplierBillDetail_SupplierBill] FOREIGN KEY ([SBillID]) REFERENCES [SupplierBill]([SBillID]), 
    CONSTRAINT [FK_SupplierBillDetail_Labour] FOREIGN KEY ([LabourID]) REFERENCES [Labour]([LabourID]), 

)
