﻿CREATE TABLE [dbo].[Accruals]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [ContractId] UNIQUEIDENTIFIER NOT NULL, 
    [AccrualDate] DATE NOT NULL, 
    [Summ] DECIMAL(19, 4) NOT NULL, 
    CONSTRAINT [PK_Accruals] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Accruals_Contracts] FOREIGN KEY ([ContractId])
		REFERENCES [dbo].[Contracts] ([Id])
)
