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

:r .\Seeding\Identity\RolesSeed.sql
:r .\Seeding\Identity\UsersSeed.sql
:r .\Seeding\Identity\UserRolesSeed.sql

:r .\Seeding\TenantsSeed.sql
:r .\Seeding\RoomsSeed.sql
:r .\Seeding\AccrualTypesSeed.sql
:r .\Seeding\CalcCoefficientsSeed.sql
:r .\Seeding\ContractsSeed.sql
:r .\Seeding\AccrualsSeed.sql
:r .\Seeding\PaymentsSeed.sql