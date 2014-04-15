// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>The SQL Identity column.</summary>
        private const string SqlIdentityColumn = "SELECT C.Name FROM sys.objects o INNER JOIN sys.columns c ON o.object_id = c.object_id WHERE c.is_identity = 1 AND o.object_id = OBJECT_ID(@ObjectName)";

        /// <summary>The SQL primary key.</summary>
        private const string SqlPrimaryKey = "SELECT COL_NAME(ic.OBJECT_ID, ic.column_id) AS ColumnName FROM sys.indexes AS i INNER JOIN sys.index_columns AS ic ON i.OBJECT_ID = ic.OBJECT_ID AND i.index_id = ic.index_id WHERE i.is_primary_key = 1 AND ic.OBJECT_ID = OBJECT_ID(@ObjectName)";

        /// <summary>The SQL select into.</summary>
        private const string SqlSelectInto = "SELECT {2} INTO {1} FROM {0} WHERE 1 = 2";

        /// <summary>The SQL drop table.</summary>
        private const string SqlDropTable = "DROP TABLE {0}";

        /// <summary>The SQL action update.</summary>
        private const string SqlActionUpdate = "UPDATE A SET {2} FROM {0} AS A INNER JOIN {1} AS B ON {3}";

        /// <summary>The SQL action delete.</summary>
        private const string SqlActionDelete = "DELETE FROM A FROM {0} AS A INNER JOIN {1} AS B ON {2}";

        /// <summary>The SQL action insert.</summary>
        private const string SqlActionInsert = "INSERT INTO {0} ({3}) SELECT {4} FROM {1} AS B WHERE NOT EXISTS (SELECT 1 FROM {0} AS A WHERE {2})";

        /// <summary>The SQL get table column.</summary>
        private const string SqlGetTableColumn = "SELECT * FROM {0} WHERE 1 = 0";

        /// <summary>The SQL Batch operation with where clause.</summary>
        private const string SqlBatchOperation = @"
DECLARE @BatchSize INT
DECLARE @NbItem INT
DECLARE @I INT

SET @BatchSize = {3}
SET @NbItem = (SELECT COUNT(1) FROM {0})
SET @I = 1


WHILE @I <= @NbItem
BEGIN
    {1}
    WHERE [{2}] >= @I AND [{2}] < @I + @BatchSize

    SET @I = @I + @BatchSize
END
";

        /// <summary>The SQL Batch operation with and clause.</summary>
        private const string SqlBatchOperationAnd = @"
DECLARE @BatchSize INT
DECLARE @NbItem INT
DECLARE @I INT

SET @BatchSize = {3}
SET @NbItem = (SELECT COUNT(1) FROM {0})
SET @I = 1


WHILE @I <= @NbItem
BEGIN
    {1}
    AND [{2}] >= @I AND [{2}] < @I + @BatchSize

    SET @I = @I + @BatchSize
END
";
    }
}