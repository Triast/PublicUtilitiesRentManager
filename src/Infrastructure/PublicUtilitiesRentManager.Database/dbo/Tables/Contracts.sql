CREATE TABLE [dbo].[Contracts]
(
	[Id] NVARCHAR(128) NOT NULL,
	[TenantId] NVARCHAR(128) NOT NULL,
	[RoomId] NVARCHAR(128) NOT NULL,
	[AccrualTypeId] NVARCHAR(128) NOT NULL,
	[CalcCoefficientId] NVARCHAR(128) NOT NULL,
	[StartDate] DATE NOT NULL, 
    [EndDate] DATE NOT NULL, 
    CONSTRAINT [PK_Contracts] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Contracts_Tenants] FOREIGN KEY ([TenantId])
		REFERENCES [dbo].[Tenants] ([Id]),
	CONSTRAINT [FK_Contracts_Rooms] FOREIGN KEY ([RoomId])
		REFERENCES [dbo].[Rooms] ([Id]),
	CONSTRAINT [FK_Contracts_AccrualTypes] FOREIGN KEY ([AccrualTypeId])
		REFERENCES [dbo].[AccrualTypes] ([Id]),
	CONSTRAINT [FK_Contracts_CalcCoefficients] FOREIGN KEY ([CalcCoefficientId])
		REFERENCES [dbo].[CalcCoefficients] ([Id])
)
