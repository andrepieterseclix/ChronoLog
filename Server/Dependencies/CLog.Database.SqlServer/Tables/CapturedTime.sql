CREATE TABLE [dbo].[CapturedTime] (
    [Id]          BIGINT   IDENTITY (1, 1) NOT NULL,
    [UserId]      BIGINT   NOT NULL,
    [Date]        DATETIME NOT NULL,
    [HoursWorked] TINYINT  NOT NULL,
    CONSTRAINT [PK_CapturedTime] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_CapturedTime] CHECK ([HoursWorked] <= (24)),
    CONSTRAINT [FK_CapturedTime] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_CapturedTime_Date]
    ON [dbo].[CapturedTime]([Date] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CapturedTime_UserId_Date]
    ON [dbo].[CapturedTime]([UserId] ASC, [Date] ASC);
