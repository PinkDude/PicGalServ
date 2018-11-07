CREATE TABLE [dbo].[ApplicationUser] (
    [Id]           NVARCHAR (50) NOT NULL,
    [Email]        NCHAR (10)    NOT NULL,
    [EmailNorm]    NCHAR (10)    NOT NULL,
    [PasswordHash] NVARCHAR (50) NOT NULL,
    [PersonInfoId] INT           NULL,
    [RoleId]       INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);