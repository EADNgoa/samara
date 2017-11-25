CREATE TABLE [dbo].[ClientBill]
(
	[CBillID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ClientID] INT NULL, 
    [Tdate] DATE NULL, 
    [RetentionPerc] DECIMAL(10, 2) NULL, 
    [RetentionAmt] DECIMAL(18, 2) NULL,
	[RetentionAmtIsPaid] bit NOT NULL DEFAULT 0, 
	[TaxPerc] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_ClientBill_Client] FOREIGN KEY ([ClientID]) REFERENCES [Client]([ClientID])
)
