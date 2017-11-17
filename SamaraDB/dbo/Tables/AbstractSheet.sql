CREATE TABLE [dbo].[AbstractSheet]
(
	[AbstractID] INT NOT NULL PRIMARY KEY, 
    [WorkID] INT NULL,
	[UnitID] INT NULL,
    [Qty] INT NULL,
	[RatePerUnit] DECIMAL(18, 2) NULL, 
    [Amount] DECIMAL(18, 2) NULL,
)
