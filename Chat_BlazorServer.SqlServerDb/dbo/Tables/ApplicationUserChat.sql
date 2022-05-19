CREATE TABLE [dbo].[ApplicationUserChat] (
    [ChatUsersId] NVARCHAR (450) NOT NULL,
    [UserChatsId] INT            NOT NULL,
    CONSTRAINT [PK_ApplicationUserChat] PRIMARY KEY CLUSTERED ([ChatUsersId] ASC, [UserChatsId] ASC),
    CONSTRAINT [FK_ApplicationUserChat_AspNetUsers_ChatUsersId] FOREIGN KEY ([ChatUsersId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ApplicationUserChat_Chats_UserChatsId] FOREIGN KEY ([UserChatsId]) REFERENCES [dbo].[Chats] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUserChat_UserChatsId]
    ON [dbo].[ApplicationUserChat]([UserChatsId] ASC);

