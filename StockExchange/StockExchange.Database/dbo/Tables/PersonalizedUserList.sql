CREATE TABLE [dbo].[PersonalizedUserList] (
    [UserId]    UNIQUEIDENTIFIER NOT NULL,
    [StockCode] NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.UserListings] PRIMARY KEY CLUSTERED ([UserId] ASC, [StockCode] ASC)
);

