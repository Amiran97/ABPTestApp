IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ABPTestDB')
BEGIN
  CREATE DATABASE ABPTestDB;
END;
GO
USE ABPTestDB;
GO
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'TheSchema' AND TABLE_NAME = 'Experiments')
BEGIN
  CREATE TABLE [dbo].[Experiments]
  (
  	[Id] UNIQUEIDENTIFIER  DEFAULT NEWID() PRIMARY KEY,
  	[DeviceToken] NVARCHAR(100) NOT NULL,
  	[Key] NVARCHAR(32) NOT NULL,
  	[Value] NVARCHAR(32) NOT NULL
  );
  CREATE UNIQUE INDEX [UniqueExperiments]
  	ON [dbo].[Experiments]([DeviceToken], [Key]);
END;
GO
CREATE OR ALTER PROCEDURE [dbo].[GetExperimentByDeviceTokenAndKey] 
@DeviceToken NVARCHAR(100), 
@Key NVARCHAR(32)
AS
BEGIN
    SELECT [Value] FROM [dbo].[Experiments] WHERE [DeviceToken] = @DeviceToken AND [Key] = @Key
END