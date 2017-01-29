CREATE TABLE [dbo].[DateState]
(
	[Id]				BIGINT   IDENTITY (1, 1) NOT NULL,
    [Date]				DATETIME NOT NULL,
    [IsLocked]			BIT      NOT NULL,
    [IsPublicHoliday]   BIT      NOT NULL,
    CONSTRAINT [PK_DateState] PRIMARY KEY CLUSTERED ([Id] ASC),
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_DateState_Date]
    ON [dbo].[DateState]([Date] ASC);