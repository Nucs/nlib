// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Z.Utility
{
    internal partial class SqlBulkOperation
    {
        /// <summary>State of the data row.</summary>
        public DataRowState? DataRowState;

        /// <summary>The data source.</summary>
        public object DataSource;

        /// <summary>The identity column.</summary>
        private string IdentityColumn;

        /// <summary>The internal data source.</summary>
        private object InternalDataSource;

        /// <summary>true if this object is custom transaction.</summary>
        private bool IsCustomTransaction;

        /// <summary>The object mapping.</summary>
        public List<string> ObjectMapping;

        /// <summary>The primary keys.</summary>
        public List<string> PrimaryKeys;

        /// <summary>The SQL bulk action.</summary>
        private string SqlBulkAction;

        /// <summary>The second SQL bulk action.</summary>
        private string SqlBulkAction2;

        /// <summary>The SQL bulk copy.</summary>
        public SqlBulkCopy SqlBulkCopy;

        /// <summary>The SQL connection.</summary>
        private SqlConnection SqlConnection;

        /// <summary>The SQL create temporary table action.</summary>
        private string SqlCreateTemporaryTableAction;

        /// <summary>The SQL drop table action.</summary>
        private string SqlDropTableAction;

        /// <summary>The SQL transaction.</summary>
        private SqlTransaction SqlTransaction;

        /// <summary>Name of the temporary column.</summary>
        private string TemporaryColumnName;

        /// <summary>Name of the temporary table.</summary>
        private string TemporaryTableName;

        /// <summary>Gets a list of mappings.</summary>
        /// <value>A List of mappings.</value>
        private List<SqlBulkCopyColumnMapping> MappingList
        {
            get { return SqlBulkCopy.ColumnMappings.Cast<SqlBulkCopyColumnMapping>().ToList(); }
        }
    }
}