CREATE TABLE [dbo].[Payments]
(
	[Id] NVARCHAR(128) NOT NULL,
    [ContractId] NVARCHAR(128) NOT NULL, 
    [PaymentOrderNumber] INT NOT NULL, 
    [PaymentDate] DATE NOT NULL, 
    [Summ] DECIMAL(19, 4) NOT NULL, 
    CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Payments_Contracts] FOREIGN KEY ([ContractId])
		REFERENCES [dbo].[Contracts] ([Id])
)
