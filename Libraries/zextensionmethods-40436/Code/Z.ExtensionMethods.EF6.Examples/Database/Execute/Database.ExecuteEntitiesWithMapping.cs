using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class Database_ExecuteEntitiesWithMapping
    {
        [TestMethod]
        public void ExecuteEntitiesWithMapping()
        {
            // Test Complex
            using (var ctx = new CodeFirstContext())
            {
                ctx.Database.ExecuteNonQuery("TRUNCATE TABLE ComplexTypeTable");

                ctx.ComplexTypes.Add(new ComplexType()
                {
                    Description = "Description",
                    Info1 = new ComplexTypeInfo1()
                    {
                        DescriptionInfo1 = "DescriptionInfo1",
                        Info2 = new ComplexTypeInfo2() { DescriptionInfo2 = "DescriptionInfo2" }
                    }
                });
                ctx.SaveChanges();

                var entities = ctx.ComplexTypes.ToList();
                var entity = entities[0];
                entity.Description = "BulkDescription";
                entity.Info1.DescriptionInfo1 = "BulkDescriptionInfo1";
                entity.Info1.Info2.DescriptionInfo2 = "BulkDescriptionInfo2";

                // Examples
                ctx.SqlBulkUpdate(entities);
            }

            using (var ctx = new CodeFirstContext())
            {
                // Unit Test
                var result = ctx.Database.ExecuteEntitiesWithMapping<ComplexType>("SELECT * FROM ComplexTypeTable").ToList()[0];
                Assert.AreEqual("BulkDescription", result.Description);
                Assert.AreEqual("BulkDescriptionInfo1", result.Info1.DescriptionInfo1);
                Assert.AreEqual("BulkDescriptionInfo2", result.Info1.Info2.DescriptionInfo2);
            }

            // Test TPC
            using (var ctx = new CodeFirstContext())
            {
                ctx.Database.ExecuteNonQuery("TRUNCATE TABLE EntityTPC1Table");

                ctx.EntityTPCs.Add(new EntityTPC1()
                {
                    Description = "Description",
                    DescriptionTPC1 = "DescriptionTPC1"
                });
                ctx.SaveChanges();

                var entities = ctx.EntityTPCs.ToList().Cast<EntityTPC1>().ToList();
                var entity = entities[0];
                entity.Description = "BulkDescription";
                entity.DescriptionTPC1 = "BulkDescriptionTPC1";

                // Examples
                ctx.SqlBulkUpdate(entities);
            }

            using (var ctx = new CodeFirstContext())
            {
                // Unit Test
                var result = ctx.Database.ExecuteEntitiesWithMapping<EntityTPC1>("SELECT * FROM EntityTPC1Table").ToList()[0];
                Assert.AreEqual("BulkDescription", result.Description);
                Assert.AreEqual("BulkDescriptionTPC1", result.DescriptionTPC1);
            }

            // Test TPH
            using (var ctx = new CodeFirstContext())
            {
                ctx.Database.ExecuteNonQuery("TRUNCATE TABLE EntityTPHs");

                ctx.EntityTPHs.Add(new EntityTPH1()
                {
                    Description = "Description",
                    DescriptionTPH1 = "DescriptionTPH1"
                });
                ctx.SaveChanges();

                var entities = ctx.EntityTPHs.ToList().Cast<EntityTPH1>().ToList();
                var entity = entities[0];
                entity.Description = "BulkDescription";
                entity.DescriptionTPH1 = "BulkDescriptionTPH1";

                // Examples
                ctx.SqlBulkUpdate(entities);
            }

            using (var ctx = new CodeFirstContext())
            {
                // Unit Test
                var result = ctx.Database.ExecuteEntitiesWithMapping<EntityTPH1>("SELECT * FROM EntityTPHs").ToList()[0];
                Assert.AreEqual("BulkDescription", result.Description);
                Assert.AreEqual("BulkDescriptionTPH1", result.DescriptionTPH1);
            }

            // Test TPT
            using (var ctx = new CodeFirstContext())
            {
                Exception ex = null;
                try
                {
                    var list = ctx.EntityTPTs.ToList();
                    ctx.SqlBulkUpdate(list);
                }
                catch (Exception ex2)
                {
                    ex = ex2;
                }

                Assert.IsNotNull(ex);
            }

            // Test Inherit with int Identity
            using (var ctx = new CodeFirstContext())
            {
                ctx.Database.ExecuteNonQuery("TRUNCATE TABLE EntityWithTypeIdInts");

                ctx.EntityWithTypeIdInts.Add(new EntityWithTypeIdInt()
                {
                    Description = "Description"
                });
                ctx.SaveChanges();

                var entities = ctx.EntityWithTypeIdInts.ToList();
                var entity = entities[0];
                entity.Description = "BulkDescription";

                // Examples
                ctx.SqlBulkUpdate(entities);
            }

            using (var ctx = new CodeFirstContext())
            {
                // Unit Test
                var result = ctx.Database.ExecuteEntitiesWithMapping<EntityWithTypeIdInt>("SELECT * FROM EntityWithTypeIdInts").ToList()[0];
                Assert.AreEqual("BulkDescription", result.Description);
            }

            // Test Inherit with Guid Identity
            using (var ctx = new CodeFirstContext())
            {
                ctx.Database.ExecuteNonQuery("TRUNCATE TABLE EntityWithTypeIdGuids");

                ctx.EntityWithTypeIdGuids.Add(new EntityWithTypeIdGuid()
                {
                    Description = "Description"
                });
                ctx.SaveChanges();

                var entities = ctx.EntityWithTypeIdGuids.ToList();
                var entity = entities[0];
                entity.Description = "BulkDescription";

                // Examples
                ctx.SqlBulkUpdate(entities);
            }

            using (var ctx = new CodeFirstContext())
            {
                // Unit Test
                var result = ctx.Database.ExecuteEntitiesWithMapping<EntityWithTypeIdGuid>("SELECT * FROM EntityWithTypeIdGuids").ToList()[0];
                Assert.AreEqual("BulkDescription", result.Description);
            }

            // Test all column mappings
            var date = new DateTime(1981, 04, 13);
            var guid = Guid.NewGuid();

            using (var ctx = new CodeFirstContext())
            {
                ctx.Database.ExecuteNonQuery("TRUNCATE TABLE EntityWithAllColumns");

                ctx.EntityWithAllColumns.Add(new EntityWithAllColumn()
                {
                    SmallIntColumn = 5
                });
                ctx.SaveChanges();

                var entities = ctx.EntityWithAllColumns.ToList();
                var entity = entities[0];
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

                // Examples
                ctx.SqlBulkUpdate(entities);
            }

            using (var ctx = new CodeFirstContext())
            {
                // Unit Test
                var result = ctx.Database.ExecuteEntitiesWithMapping<EntityWithAllColumn>("SELECT * FROM EntityWithAllColumns").ToList()[0];

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
    }
}