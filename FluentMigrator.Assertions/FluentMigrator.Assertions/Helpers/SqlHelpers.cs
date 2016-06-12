using System;

namespace FluentMigrator.Assertions.Helpers
{
    public static class SqlHelpers
    {
        public static string CreateRaiseErrorSql(string errorMessage)
        {
            return $"RAISERROR('{errorMessage.EscapeApostraphes()}', 15, -1); RETURN";
        }
        
        public static string RemoveCommentsAndWhiteSpace(out string variableName, string sqlString)
        {
            variableName = "X" + Guid.NewGuid().ToString("N");
            return $@"
DECLARE @CodeBlockStart{variableName} int, @CodeBlockEnd{variableName} int, @{variableName} varchar(max)
SET @{variableName} = {sqlString}
SET @CodeBlockStart{variableName} = PATINDEX('%/*%', @{variableName})

WHILE @CodeBlockStart{variableName} > 0
BEGIN
    SET @CodeBlockEnd{variableName} = PATINDEX('%*/%', @{variableName})
    SET @{variableName} = LEFT(@{variableName}, @CodeBlockStart{variableName} - 1) + RIGHT(@{variableName}, LEN(@{variableName}) - (@CodeBlockEnd{variableName} + 1))
    SET @CodeBlockStart{variableName} = PATINDEX('%/*%', @{variableName})
END

DECLARE @DoubleDashStart{variableName} int, @DoubleDashLineEnd{variableName} int
SET @DoubleDashStart{variableName} = PATINDEX('%--%', @{variableName})
WHILE @DoubleDashStart{variableName} > 0
BEGIN
    SET @DoubleDashLineEnd{variableName} = PATINDEX('%' + CHAR(13) + CHAR(10) + '%', RIGHT(@{variableName}, LEN(@{variableName}) - (@DoubleDashStart{variableName}))) + @DoubleDashStart{variableName}
    SET @{variableName} = LEFT(@{variableName}, @DoubleDashStart{variableName} - 1) + RIGHT(@{variableName}, LEN(@{variableName}) - @DoubleDashLineEnd{variableName})
    SET @DoubleDashStart{variableName} = PATINDEX('%--%', @{variableName})
END
--Horizontal Tab
SET @{variableName} = Replace(@{variableName},CHAR(9),'');
--Line Feed
SET @{variableName} = Replace(@{variableName},CHAR(10),'');
--Vertical Tab
SET @{variableName} = Replace(@{variableName},CHAR(11),'');
--Form Feed
SET @{variableName} = Replace(@{variableName},CHAR(12),'');
--Carriage Return
SET @{variableName} = Replace(@{variableName},CHAR(13),'');
--Column Break
SET @{variableName} = Replace(@{variableName},CHAR(14),'');
--Non-breaking space
SET @{variableName} = Replace(@{variableName},CHAR(160),'');
SET @{variableName} = LTRIM(RTRIM(@{variableName}));
";
        }
    }
}
