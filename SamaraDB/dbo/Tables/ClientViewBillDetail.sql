CREATE TABLE [dbo].[ClientViewBillDetail]
(
	[ClientViewBillDetailID] INT NOT NULL PRIMARY KEY, 
    [CBillID] INT NULL, 
    [Description] VARCHAR(250) NULL, 
    [Amount] DECIMAL(18, 2) NULL, 
    [DebitCredit] BIT NULL, 
    [BeforeTax] NCHAR(10) NULL, 
    CONSTRAINT [FK_ClientViewBillDetail_ClientBill] FOREIGN KEY ([CBillID]) REFERENCES [ClientBill]([CBillID]), 
)
