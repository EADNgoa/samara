﻿CREATE TABLE [dbo].[Config]
(
	[ConfigID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PANnumber] VARCHAR(15) NULL, 
    [TANnumber] VARCHAR(15) NULL, 
    [RowsPerPage] INT NOT NULL DEFAULT 10, 
    [TDSperc] DECIMAL(18, 2) NULL
)
