CREATE TABLE [dbo].[RateAdditions]
(
	[RateAdditionID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RateAdditionDesc] VARCHAR(100) NULL, 
    [Percentage] DECIMAL(18, 2) NULL
)
