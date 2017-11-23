CREATE TABLE [dbo].[ClientBillDetail]
(
	[CBillDetailID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CBillID] INT NULL, 	
    [Description] VARCHAR(250) NULL, 
    [Amount] DECIMAL(18, 2) NULL, 
    [DebitCredit] BIT NULL, 
    [BeforeTax] BIT NULL, 
 
 
 
  
)
