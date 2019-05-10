CREATE TABLE [dbo].[Rooms]
(
	[Id] NVARCHAR(128) NOT NULL, 
    [RoomTypeId] NVARCHAR(128) NOT NULL, 
    [Address] NVARCHAR(MAX) NOT NULL, 
    [Square] FLOAT NOT NULL, 
    [IsOccupied] BIT NOT NULL, 
    [Price] DECIMAL(19, 4) NOT NULL,
	[IncreasingCoefToBaseRate] DECIMAL(19, 4) NOT NULL, 
    [PlacementCoef] DECIMAL(19, 4) NOT NULL, 
    [ComfortCoef] DECIMAL(19, 4) NOT NULL, 
    [SocialOrientationCoef] DECIMAL(19, 4) NOT NULL, 
    CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Rooms_RoomTypes] FOREIGN KEY ([RoomTypeId])
		REFERENCES [dbo].[RoomTypes] ([Id])
)
