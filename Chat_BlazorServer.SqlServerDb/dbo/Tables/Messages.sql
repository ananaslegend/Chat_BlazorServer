CREATE TABLE [dbo].[Messages] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [AuthorId] NVARCHAR (450) NULL,
    [ChatId]   INT            NOT NULL,
    [Date]     DATETIME2 (7)  NOT NULL,
    [ReplyId]  INT            NULL,
    [Data]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_Messages_Chats_ChatId] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Messages_ReplyId] FOREIGN KEY ([ReplyId]) REFERENCES [dbo].[Messages] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_AuthorId]
    ON [dbo].[Messages]([AuthorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_ChatId]
    ON [dbo].[Messages]([ChatId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_ReplyId]
    ON [dbo].[Messages]([ReplyId] ASC);

