CREATE TABLE [dbo].[SupplierBill]
(
	[SBillID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SupplierID] INT NULL, 
    [Tdate] DATE NULL, 
    [TDSperc] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_SupplierBill_Supllier] FOREIGN KEY ([SupplierID]) REFERENCES [Supplier]([SupplierID])
)
