using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods;

namespace ExtensionMethods.Examples
{
    [TestClass]
    public class System_Data_Common_DbCommand_ExecuteEntities
    {
        [TestMethod]
        public void ExecuteEntities()
        {
            var date = new DateTime(1981, 04, 13);
            var guid = Guid.NewGuid();

            var entity = new EntityWithAllColumn();
            entity.BitIntColumn = 1;
            entity.BinaryColumn = new byte[] { 1 };
            entity.BitColumn = true;
            entity.CharColumn = "z";
            entity.DateColumn = date;
            entity.DateTimeColumn = date;
            entity.DateTime2Column = date;
            entity.DateTimeOffsetColumn = date;
            entity.DecimalColumn = 1.25m;
            entity.FloatColumn = 1.25f;
            entity.ImageColumn = new byte[] { 1 };
            entity.IntColumn = 1;
            entity.MoneyColumn = 1.25m;
            entity.NCharColumn = "z";
            entity.NTextColumn = "z";
            entity.NumericColumn = 1;
            entity.NVarcharColumn = "z";
            entity.NVarcharMaxColumn = "z";
            entity.RealColumn = 1.25f;
            entity.SmallDateTimeColumn = date;
            entity.SmallIntColumn = null;
            entity.SmallMoneyColumn = 1.25m;
            entity.TextColumn = "z";
            entity.TimeColumn = date.TimeOfDay;
            entity.TimestampColumn = new byte[] { 1 };
            entity.TinyIntColumn = 1;
            entity.UniqueIdentifierColumn = guid;
            entity.VarBinaryColumn = new byte[] { 1 };
            entity.VarBinaryMaxColumn = new byte[] { 1 };
            entity.VarcharColumn = "z";
            entity.VarcharMaxColumn = "z";
            entity.XmlColumn = "z";

            using (var conn = new SqlConnection(My.Config.ConnectionString.UnitTest.ConnectionString))
            {
                conn.Open();
                conn.ExecuteNonQuery("TRUNCATE TABLE EntityWithAllColumns");

                using (var copy = new SqlBulkCopy(conn))
                {
                    copy.DestinationTableName = "EntityWithAllColumns";
                    copy.BulkInsert(new List<EntityWithAllColumn> { entity });
                }

                // Unit Test
                var result = conn.ExecuteEntities<EntityWithAllColumn>("SELECT TOP 1 * FROM EntityWithAllColumns").ToList()[0];
                Assert.AreEqual(1, result.BitIntColumn);
                Assert.AreEqual(1, result.BinaryColumn[0]);
                Assert.AreEqual(true, result.BitColumn);
                Assert.AreEqual("z", result.CharColumn);
                Assert.AreEqual(date, result.DateColumn);
                Assert.AreEqual(date, result.DateTimeColumn);
                Assert.AreEqual(date, result.DateTime2Column);
                Assert.AreEqual(date, result.DateTimeOffsetColumn);
                Assert.AreEqual(1.25m, result.DecimalColumn);
                Assert.AreEqual(1.25f, result.FloatColumn);
                Assert.AreEqual(1, result.ImageColumn[0]);
                Assert.AreEqual(1, result.IntColumn);
                Assert.AreEqual(1.25m, result.MoneyColumn);
                Assert.AreEqual("z", result.NCharColumn);
                Assert.AreEqual("z", result.NTextColumn);
                Assert.AreEqual(1, result.NumericColumn);
                Assert.AreEqual("z", result.NVarcharColumn);
                Assert.AreEqual("z", result.NVarcharMaxColumn);
                Assert.AreEqual(1.25f, result.RealColumn);
                Assert.AreEqual(date, result.SmallDateTimeColumn);
                Assert.AreEqual(null, result.SmallIntColumn);
                Assert.AreEqual(1.25m, result.SmallMoneyColumn);
                Assert.AreEqual("z", result.TextColumn);
                Assert.AreEqual(date.TimeOfDay, result.TimeColumn);
                Assert.AreEqual(1, result.TimestampColumn[0]);
                Assert.AreEqual((Byte)1, result.TinyIntColumn);
                Assert.AreEqual(guid, result.UniqueIdentifierColumn);
                Assert.AreEqual(1, result.VarBinaryColumn[0]);
                Assert.AreEqual(1, result.VarBinaryMaxColumn[0]);
                Assert.AreEqual("z", result.VarcharColumn);
                Assert.AreEqual("z", result.VarcharMaxColumn);
                Assert.AreEqual("z", result.XmlColumn);
            }
        }

        public class EntityWithAllColumn
        {
            public int ID { get; set; }
            public long? BitIntColumn { get; set; }
            public byte[] BinaryColumn { get; set; }
            public bool? BitColumn { get; set; }
            public string CharColumn { get; set; }
            public DateTime? DateColumn { get; set; }
            public DateTime? DateTimeColumn { get; set; }
            public DateTime? DateTime2Column { get; set; }
            public DateTimeOffset? DateTimeOffsetColumn { get; set; }
            public decimal? DecimalColumn { get; set; }
            public double? FloatColumn { get; set; }
            public byte[] ImageColumn { get; set; }
            public int? IntColumn { get; set; }
            public decimal? MoneyColumn { get; set; }
            public string NCharColumn { get; set; }
            public string NTextColumn { get; set; }
            public decimal? NumericColumn { get; set; }
            public string NVarcharColumn { get; set; }
            public string NVarcharMaxColumn { get; set; }
            public float? RealColumn { get; set; }
            public DateTime? SmallDateTimeColumn { get; set; }
            public short? SmallIntColumn { get; set; }
            public decimal? SmallMoneyColumn { get; set; }
            public string TextColumn { get; set; }
            public TimeSpan? TimeColumn { get; set; }
            public byte[] TimestampColumn { get; set; }
            public byte? TinyIntColumn { get; set; }
            public Guid? UniqueIdentifierColumn { get; set; }
            public byte[] VarBinaryColumn { get; set; }
            public byte[] VarBinaryMaxColumn { get; set; }
            public string VarcharColumn { get; set; }
            public string VarcharMaxColumn { get; set; }
            public string XmlColumn { get; set; }
        }
    }
}