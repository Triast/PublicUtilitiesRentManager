﻿CREATE TABLE [dbo].[Rooms]
(
	[Id] NVARCHAR(128) NOT NULL, 
    [Address] NVARCHAR(MAX) NOT NULL, 
    [RoomType] NVARCHAR(50) NOT NULL, 
    [Square] FLOAT NOT NULL, 
    [Price] DECIMAL(19, 4) NOT NULL, 
    [IsOccupied] BIT NOT NULL,
	CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED ([Id] ASC)
)
