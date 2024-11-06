USE SBCMS
GO

CREATE OR ALTER PROCEDURE UpdateUser (
  @UserId int,
  @UserName NVARCHAR,
  @Email NVARCHAR,
  @Password NVARCHAR
)
AS
BEGIN

UPDATE [User] SET 
  UserName = @UserName,
  Email = @Email,
  Password = @Password
  WHERE UserId = @UserId
END
GO