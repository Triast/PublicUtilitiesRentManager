CREATE TABLE [dbo].[Contracts]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[TenantId] UNIQUEIDENTIFIER NOT NULL,
	[RoomId] UNIQUEIDENTIFIER NOT NULL,
	[AccrualTypeId] UNIQUEIDENTIFIER NOT NULL,
	[CalcCoefficientId] UNIQUEIDENTIFIER NOT NULL,
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
