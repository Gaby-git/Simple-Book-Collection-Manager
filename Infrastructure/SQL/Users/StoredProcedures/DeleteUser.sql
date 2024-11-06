USE SBCMS
GO

CREATE OR ALTER PROCEDURE DeleteUser(
@UserId int 
)
 AS
 BEGIN;

 DELETE FROM [User]	
 WHERE UserId = @UserId;

 END;
 GO