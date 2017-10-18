CREATE TABLE [dbo].[ClientBill]
(
	[CBillID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ClientID] INT NULL, 
    [Tdate] DATE NULL, 
    [RetentionPerc] DECIMAL(18, 2) NULL, 
    CONSTRAINT [FK_ClientBill_Client] FOREIGN KEY ([ClientID]) REFERENCES [Client]([ClientID])
)
