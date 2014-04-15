using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Z.ExtensionMethods.EF6.Examples
{
    public class CodeFirstContext : DbContext
    {
        public CodeFirstContext() : base("CodeFirstConnectionString")
        {
        }

        public IDbSet<TestWithoutIdentity> TestWithoutIdentitys { get; set; }
        public IDbSet<Student> Students { get; set; }
        public DbSet<TestMappingFizz2> TestMappingFizzs { get; set; }
        public DbSet<TestMappingCodeFirst> TestMappingCodeFirsts { get; set; }
        public DbSet<BulkCopyTestCodeFirst> BulkCopyTestCodeFirsts { get; set; }
        public DbSet<ComplexType> ComplexTypes { get; set; }
        public DbSet<EntityTPC> EntityTPCs { get; set; }
        public DbSet<EntityWithTypeIdGuid> EntityWithTypeIdGuids { get; set; }
        public DbSet<EntityWithTypeIdInt> EntityWithTypeIdInts { get; set; }
        public DbSet<EntityTPH> EntityTPHs { get; set; }
        public DbSet<EntityTPT> EntityTPTs { get; set; }
        public DbSet<EntityWithAllColumn> EntityWithAllColumns { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.ComplexType<ComplexTypeInfo1>();

            mb.Entity<ComplexType>().ToTable("ComplexTypeTable");
            mb.Entity<ComplexType>().Property(x => x.Description).HasColumnName("DescriptionColumn");

            mb.Entity<EntityTPT1>().ToTable("EntityTPT1Table");
            mb.Entity<EntityTPT2>().ToTable("EntityTPT2Table");

            mb.Entity<EntityTPC1>().Map(x =>
            {
                x.MapInheritedProperties();
                x.ToTable("EntityTPC1Table");
            });

            mb.Entity<EntityTPC2>().Map(x =>
            {
                x.MapInheritedProperties();
                x.ToTable("EntityTPC2Table");
            });

            mb.Entity<EntityWithTypeIdGuid>().Property(x => x.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            base.OnModelCreating(mb);
        }
    }

    [Table("TestWithoutIdentityCustomName")]
    public class TestWithoutIdentity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ID { get; set; }
        [Column("CustomIntValue")]
        public int IntValue { get; set; }
    }

    [Table("Student")]
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
    }

    [Table("Standard")]
    public class Standard
    {
        public int StandardId { get; set; }
        public string StandardName { get; set; }
        public string Description { get; set; }
    }

    [Table("BulkCopyTest")]
    public class BulkCopyTestCodeFirst
    {
        [Column(Order = 0)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID1 { get; set; }

        [Column(Order = 1)]
        [Key]
        public int ID2 { get; set; }

        public bool? ValueBit { get; set; }
        public int? ValueInt { get; set; }

        [MaxLength(50)]
        public string ValueString { get; set; }
    }

    [Table("TestMapping")]
    public class TestMappingFizz2
    {
        [Key]
        [Column("PrimaryKeyColumn")]
        public int PrimaryKeyColumnBuzz { get; set; }

        [Column("IntColumn")]
        public int IntColumnBuzz { get; set; }

        [Column("VarcharColumn")]
        public string VarcharColumnBuzz { get; set; }

        [NotMapped]
        public string UnmappingColumn { get; set; }
    }

    [Table("TestMapping2", Schema = "custom")]
    public class TestMappingCodeFirst
    {
        [Key]
        public int PrimaryKeyColumn { get; set; }

        [ConcurrencyCheck]
        [Timestamp]
        public byte[] TimestampColumn { get; set; }

        [Column("CustomColumn2", TypeName = "varchar")]
        [MaxLength(20)]
        [Required]
        public string CustomColumn { get; set; }

        [NotMapped]
        public string UmmappingColumn { get; set; }
    }
}