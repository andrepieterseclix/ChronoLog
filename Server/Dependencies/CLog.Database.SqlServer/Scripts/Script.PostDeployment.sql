/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @DbName nvarchar(128)
select @DbName = DB_NAME()

IF @DbName = 'ChronoLog-Local' AND NOT EXISTS (SELECT 1 FROM [User] WHERE [UserName] = 'Tester')
	INSERT INTO [User] ([State], [UserName], [Password], [Salt], [Name], [Surname],[Email])
	VALUES (4, 'Tester', 'AfEqRsxb/x+ksSjj6wK0xWqi+kZZf0IMqMcO71Zomq4=', 'Lkqyl9Aa', 'John', 'Doe', 'testuser@companyname.co.za')