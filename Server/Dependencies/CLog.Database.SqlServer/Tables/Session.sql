CREATE TABLE [dbo].[Session] (
    [Id]            BIGINT           IDENTITY (1, 1) NOT NULL,
    [RefId]         UNIQUEIDENTIFIER NOT NULL,
    [SessionKey]    NVARCHAR (512)   NOT NULL,
    [UserId]        BIGINT           NOT NULL,
    [LoginTimeUtc]  DATETIME         NOT NULL,
    [LastActiveUtc] DATETIME         NOT NULL,
    [IsActive]		BIT				 NOT NULL, 
    CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Session_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Session_RefId]
    ON [dbo].[Session]([RefId] ASC);
