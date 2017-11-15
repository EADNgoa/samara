CREATE TABLE [dbo].[Work]
(
	[WorkID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [WorkName] VARCHAR(200) NULL,
    [UnitID] INT NULL, 
    [Rate] DECIMAL(18, 2) NULL, 
 
)
