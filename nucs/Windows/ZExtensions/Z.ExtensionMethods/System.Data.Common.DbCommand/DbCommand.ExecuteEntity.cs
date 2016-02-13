// Copyright (c) 2014 Jonathan Magnan (http://jonathanmagnan.com).
// All rights reserved (http://jonathanmagnan.com/extension-methods-library).
// Licensed under MIT License (MIT) (http://zextensionmethods.codeplex.com/license).

using System.Data;
using System.Data.Common;

public static partial class DbCommandExtension
{
    /// <summary>
    ///     A DbCommand extension method that executes the entity operation.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <param name="this">The @this to act on.</param>
    /// <returns>A T.</returns>
    /// <example>
    ///     <code>
    ///           using System;
    ///           using System.Collections.Generic;
    ///           using System.Data.Common;
    ///           using System.Data.SqlClient;
    ///           using System.Linq;
    ///           using Microsoft.VisualStudio.TestTools.UnitTesting;
    ///           using Z.ExtensionMethods;
    ///           
    ///           namespace ExtensionMethods.Examples
    ///           {
    ///               [TestClass]
    ///               public class System_Data_Common_DbCommand_ExecuteEntity
    ///               {
    ///                   [TestMethod]
    ///                   public void ExecuteEntity()
    ///                   {
    ///                       var date = new DateTime(1981, 04, 13);
    ///                       var guid = Guid.NewGuid();
    ///           
    ///                       var entity = new EntityWithAllColumn();
    ///                       entity.BitIntColumn = 1;
    ///                       entity.BinaryColumn = new byte[] { 1 };
    ///                       entity.BitColumn = true;
    ///                       entity.CharColumn = &quot;z&quot;;
    ///                       entity.DateColumn = date;
    ///                       entity.DateTimeColumn = date;
    ///                       entity.DateTime2Column = date;
    ///                       entity.DateTimeOffsetColumn = date;
    ///                       entity.DecimalColumn = 1.25m;
    ///                       entity.FloatColumn = 1.25f;
    ///                       entity.ImageColumn = new byte[] { 1 };
    ///                       entity.IntColumn = 1;
    ///                       entity.MoneyColumn = 1.25m;
    ///                       entity.NCharColumn = &quot;z&quot;;
    ///                       entity.NTextColumn = &quot;z&quot;;
    ///                       entity.NumericColumn = 1;
    ///                       entity.NVarcharColumn = &quot;z&quot;;
    ///                       entity.NVarcharMaxColumn = &quot;z&quot;;
    ///                       entity.RealColumn = 1.25f;
    ///                       entity.SmallDateTimeColumn = date;
    ///                       entity.SmallIntColumn = null;
    ///                       entity.SmallMoneyColumn = 1.25m;
    ///                       entity.TextColumn = &quot;z&quot;;
    ///                       entity.TimeColumn = date.TimeOfDay;
    ///                       entity.TimestampColumn = new byte[] { 1 };
    ///                       entity.TinyIntColumn = 1;
    ///                       entity.UniqueIdentifierColumn = guid;
    ///                       entity.VarBinaryColumn = new byte[] { 1 };
    ///                       entity.VarBinaryMaxColumn = new byte[] { 1 };
    ///                       entity.VarcharColumn = &quot;z&quot;;
    ///                       entity.VarcharMaxColumn = &quot;z&quot;;
    ///                       entity.XmlColumn = &quot;z&quot;;
    ///           
    ///                       using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
    ///                       {
    ///                           conn.Open();
    ///                           conn.ExecuteNonQuery(&quot;TRUNCATE TABLE EntityWithAllColumns&quot;);
    ///           
    ///                           using (var copy = new SqlBulkCopy(conn))
    ///                           {
    ///                               copy.DestinationTableName = &quot;EntityWithAllColumns&quot;;
    ///                               copy.BulkInsert(new List&lt;EntityWithAllColumn&gt; { entity });
    ///                           }
    ///           
    ///                           // Unit Test
    ///                           var result = conn.ExecuteEntity&lt;EntityWithAllColumn&gt;(&quot;SELECT TOP 1 * FROM EntityWithAllColumns&quot;);
    ///                           Assert.AreEqual(1, result.BitIntColumn);
    ///                           Assert.AreEqual(1, result.BinaryColumn[0]);
    ///                           Assert.AreEqual(true, result.BitColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.CharColumn);
    ///                           Assert.AreEqual(date, result.DateColumn);
    ///                           Assert.AreEqual(date, result.DateTimeColumn);
    ///                           Assert.AreEqual(date, result.DateTime2Column);
    ///                           Assert.AreEqual(date, result.DateTimeOffsetColumn);
    ///                           Assert.AreEqual(1.25m, result.DecimalColumn);
    ///                           Assert.AreEqual(1.25f, result.FloatColumn);
    ///                           Assert.AreEqual(1, result.ImageColumn[0]);
    ///                           Assert.AreEqual(1, result.IntColumn);
    ///                           Assert.AreEqual(1.25m, result.MoneyColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.NCharColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.NTextColumn);
    ///                           Assert.AreEqual(1, result.NumericColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.NVarcharColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.NVarcharMaxColumn);
    ///                           Assert.AreEqual(1.25f, result.RealColumn);
    ///                           Assert.AreEqual(date, result.SmallDateTimeColumn);
    ///                           Assert.AreEqual(null, result.SmallIntColumn);
    ///                           Assert.AreEqual(1.25m, result.SmallMoneyColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.TextColumn);
    ///                           Assert.AreEqual(date.TimeOfDay, result.TimeColumn);
    ///                           Assert.AreEqual(1, result.TimestampColumn[0]);
    ///                           Assert.AreEqual((Byte)1, result.TinyIntColumn);
    ///                           Assert.AreEqual(guid, result.UniqueIdentifierColumn);
    ///                           Assert.AreEqual(1, result.VarBinaryColumn[0]);
    ///                           Assert.AreEqual(1, result.VarBinaryMaxColumn[0]);
    ///                           Assert.AreEqual(&quot;z&quot;, result.VarcharColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.VarcharMaxColumn);
    ///                           Assert.AreEqual(&quot;z&quot;, result.XmlColumn);
    ///                       }
    ///                   }
    ///           
    ///                   public class EntityWithAllColumn
    ///                   {
    ///                       public int ID { get; set; }
    ///                       public long? BitIntColumn { get; set; }
    ///                       public byte[] BinaryColumn { get; set; }
    ///                       public bool? BitColumn { get; set; }
    ///                       public string CharColumn { get; set; }
    ///                       public DateTime? DateColumn { get; set; }
    ///                       public DateTime? DateTimeColumn { get; set; }
    ///                       public DateTime? DateTime2Column { get; set; }
    ///                       public DateTimeOffset? DateTimeOffsetColumn { get; set; }
    ///                       public decimal? DecimalColumn { get; set; }
    ///                       public double? FloatColumn { get; set; }
    ///                       public byte[] ImageColumn { get; set; }
    ///                       public int? IntColumn { get; set; }
    ///                       public decimal? MoneyColumn { get; set; }
    ///                       public string NCharColumn { get; set; }
    ///                       public string NTextColumn { get; set; }
    ///                       public decimal? NumericColumn { get; set; }
    ///                       public string NVarcharColumn { get; set; }
    ///                       public string NVarcharMaxColumn { get; set; }
    ///                       public float? RealColumn { get; set; }
    ///                       public DateTime? SmallDateTimeColumn { get; set; }
    ///                       public short? SmallIntColumn { get; set; }
    ///                       public decimal? SmallMoneyColumn { get; set; }
    ///                       public string TextColumn { get; set; }
    ///                       public TimeSpan? TimeColumn { get; set; }
    ///                       public byte[] TimestampColumn { get; set; }
    ///                       public byte? TinyIntColumn { get; set; }
    ///                       public Guid? UniqueIdentifierColumn { get; set; }
    ///                       public byte[] VarBinaryColumn { get; set; }
    ///                       public byte[] VarBinaryMaxColumn { get; set; }
    ///                       public string VarcharColumn { get; set; }
    ///                       public string VarcharMaxColumn { get; set; }
    ///                       public string XmlColumn { get; set; }
    ///                   }
    ///               }
    ///           }
    ///     </code>
    /// </example>
    public static T ExecuteEntity<T>(this DbCommand @this) where T : new()
    {
        using (IDataReader reader = @this.ExecuteReader())
        {
            reader.Read();
            return reader.ToEntity<T>();
        }
    }
}