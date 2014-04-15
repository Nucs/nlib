using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.EntityFramework.Model;
using Z.ExtensionMethods.EntityFramework;

namespace Z.ExtensionMethods.EF6.Examples
{
    [TestClass]
    public class DbContext_GetCodeFirstModel
    {
        [TestMethod]
        public void GetCodeFirstModel()
        {
            using (var ctx = new CodeFirstContext())
            {
                var abc = ctx.GetModelXDocument();
                var def = abc;

                // Examples
                Model model = ctx.GetCodeFirstModel();

                // Unit Test

                {
                    // Unit Test - Database
                    //Assert.AreEqual("EntityFrameworkCodeFirstTest", model.Storage.Name);

                    // Unit Test - Table
                    StorageTable table = model.Storage.Tables.Find(x => x.Name == "TestMapping2");
                    Assert.IsNotNull(table);
                    Assert.AreEqual("custom", table.Schema);
                    Assert.IsNotNull(table.Mapping);
                    
                    // Unit Test - Interface Table
                    Assert.IsNotNull(model.Storage.Tables.Find(x => x.Name == "Student"));

                    // Unit Test - Primary Key
                    Assert.IsNotNull(table.PrimaryKey);
                    Assert.AreEqual(1, table.PrimaryKey.Columns.Count);
                    Assert.AreEqual("PrimaryKeyColumn", table.PrimaryKey.Columns[0].Name);

                    // Unit Test - Columns
                    StorageColumn column;
                    column = table.Columns.Find(x => x.Name == "PrimaryKeyColumn");
                    Assert.IsNotNull(column);
                    Assert.IsFalse(column.IsNullable);
                    Assert.IsNotNull(column.Mapping);
                    column = table.Columns.Find(x => x.Name == "TimestampColumn");
                    Assert.IsNotNull(column);
                    Assert.IsNotNull(column.IsNullable);
                    Assert.IsNotNull(column.Mapping);
                    column = table.Columns.Find(x => x.Name == "CustomColumn2");
                    Assert.IsNotNull(column);
                    Assert.AreEqual("varchar", column.Type);
                    Assert.IsFalse(column.IsNullable);
                    Assert.AreEqual(20, column.MaxLength);
                    Assert.IsNotNull(column.Mapping);
                }

                {
                    // Unit Test - Conceptual
                    //Assert.AreEqual("CodeFirstContext", model.Conceptual.Name);

                    // Unit Test - Entities
                    ConceptualEntity entity = model.Conceptual.Entities.Find(x => x.Name == "TestMappingCodeFirst");
                    Assert.IsNotNull(entity);
                    Assert.AreEqual("TestMappingCodeFirsts", entity.SetName);
                    Assert.IsNotNull(entity.Mapping);

                    // Unit Test - Key
                    Assert.IsNotNull(entity.Key);
                    Assert.AreEqual(1, entity.Key.Properties.Count);
                    Assert.AreEqual("PrimaryKeyColumn", entity.Key.Properties[0].Name);

                    // Unit Test - Columns
                    ConceptualProperty property;
                    property = entity.Properties.Find(x => x.Name == "PrimaryKeyColumn");
                    Assert.IsNotNull(property);
                    Assert.IsFalse(property.IsNullable);
                    Assert.IsNotNull(property.Mapping);
                    property = entity.Properties.Find(x => x.Name == "TimestampColumn");
                    Assert.IsNotNull(property);
                    Assert.IsFalse(property.IsNullable);
                    Assert.IsNotNull(property.Mapping);
                    property = entity.Properties.Find(x => x.Name == "CustomColumn");
                    Assert.IsNotNull(property);
                    Assert.IsFalse(property.IsNullable);
                    Assert.AreEqual(20, property.MaxLength);
                    Assert.IsNotNull(property.Mapping);
                }
            }
        }
    }
}