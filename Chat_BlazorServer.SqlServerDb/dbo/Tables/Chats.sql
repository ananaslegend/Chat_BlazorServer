CREATE TABLE [dbo].[Chats] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Type] INT            NOT NULL,
    [Name] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Chats] PRIMARY KEY CLUSTERED ([Id] ASC)
);

