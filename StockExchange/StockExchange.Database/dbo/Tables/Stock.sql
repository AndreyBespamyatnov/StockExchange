CREATE TABLE [dbo].[Stock] (
    [Code]        NVARCHAR (128) NOT NULL,
    [CompanyName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.StockEntries] PRIMARY KEY CLUSTERED ([Code] ASC)
);

