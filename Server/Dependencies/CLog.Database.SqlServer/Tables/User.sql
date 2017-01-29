CREATE TABLE [dbo].[User] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
	[State]		TINYINT		   NOT NULL,
    [UserName]  NVARCHAR (60)  NOT NULL,
    [Password]  NVARCHAR (512) NOT NULL,
    [Salt]      NVARCHAR (16)  NOT NULL,
    [Name]      NVARCHAR (128) NOT NULL,
    [Surname]   NVARCHAR (128) NOT NULL,
    [Email]     NVARCHAR (128) NOT NULL,
    [ManagerId] BIGINT         NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UIX_User_UserName]
    ON [dbo].[User]([UserName] ASC);
