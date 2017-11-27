CREATE TABLE [dbo].[ClientBill]
(
	[CBillID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ClientID] INT NULL, 
	[SiteID] INT NULL,
    [Tdate] DATE NULL, 
    [RetentionPerc] DECIMAL(10, 2) NULL, 
    [RetentionAmt] DECIMAL(18, 2) NULL,
	[RetentionAmtIsPaid] bit NOT NULL DEFAULT 0, 
	[TaxPerc] DECIMAL(18, 2) NULL, 
	[GrandTotalNoTax] DECIMAL(18, 2) NULL, 
	[TaxAmt] DECIMAL(18, 2) NULL 
    CONSTRAINT [FK_ClientBill_Client] FOREIGN KEY ([ClientID]) REFERENCES [Client]([ClientID]),
  CONSTRAINT [FK_ClientBill_Sites] FOREIGN KEY ([SiteID]) REFERENCES [Sites]([SiteID])

)
