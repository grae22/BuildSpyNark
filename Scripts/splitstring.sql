DROP FUNCTION dbo.splitstring
GO

CREATE FUNCTION dbo.splitstring( @stringToSplit VARCHAR(MAX), @splitOn CHAR )
RETURNS
 @returnList TABLE ([Name] [nvarchar] (500))
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(@splitOn, @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(@splitOn, @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT @name

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @stringToSplit

 RETURN
END