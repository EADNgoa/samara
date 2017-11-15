CREATE TABLE [dbo].[ClientBillDetail]
(
	[CBillDetailID] INT NOT NULL PRIMARY KEY, 
    [CBillID] INT NULL, 
    [AbstractID] INT NULL, 
	
    [Description] VARCHAR(250) NULL, 
    [Amount] DECIMAL(18, 2) NULL, 
    [DebitCredit] BIT NULL, 
    [BeforeTax] BIT NULL, 
    CONSTRAINT [FK_ClientBillDetail_ClientBill] FOREIGN KEY ([CBillID]) REFERENCES [ClientBill]([CBillID]),
 
 
 
  
)
