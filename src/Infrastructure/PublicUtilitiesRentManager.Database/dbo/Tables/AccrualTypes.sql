﻿CREATE TABLE [dbo].[AccrualTypes]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
    CONSTRAINT [PK_AccrualTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
)