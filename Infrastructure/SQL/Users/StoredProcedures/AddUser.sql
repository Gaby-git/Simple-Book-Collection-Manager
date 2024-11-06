USE SBCMS
GO

CREATE OR ALTER PROCEDURE AddUser(
  @UserName NVARCHAR,
  @Email NVARCHAR,
  @Password NVARCHAR,
  @UserId INT OUT
)
AS
BEGIN;
  
  INSERT INTO [User] 
         (UserName, Email, Password)
  VALUES (@UserName, @Email, @Password);
  SELECT @UserId = SCOPE_IDENTITY();
END;
GO
  