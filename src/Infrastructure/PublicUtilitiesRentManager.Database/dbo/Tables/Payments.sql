CREATE TABLE [dbo].[Payments]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [ContractId] UNIQUEIDENTIFIER NOT NULL, 
    [PaymentDate] DATE NOT NULL, 
    [Summ] DECIMAL(19, 4) NOT NULL, 
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Payments_Contracts] FOREIGN KEY ([ContractId])
		REFERENCES [dbo].[Contracts] ([Id])
)
