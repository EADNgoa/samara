CREATE TABLE [dbo].[SupplierBill]
(
	[SBillID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SupplierID] INT NULL, 
    [Tdate] DATE NULL, 
    CONSTRAINT [FK_SupplierBill_Supllier] FOREIGN KEY ([SupplierID]) REFERENCES [Supplier]([SupplierID])
)
