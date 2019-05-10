CREATE TABLE [dbo].[CalcCoefficients]
(
	[Id] NVARCHAR(128) NOT NULL,
    [Name] NVARCHAR(MAX) NOT NULL, 
    [Coefficient] FLOAT NOT NULL, 
    CONSTRAINT [PK_CalcCoefficients] PRIMARY KEY CLUSTERED ([Id] ASC)
)
