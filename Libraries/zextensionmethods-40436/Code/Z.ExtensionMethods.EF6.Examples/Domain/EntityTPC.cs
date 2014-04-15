namespace Z.ExtensionMethods.EF6.Examples
{
    public abstract class EntityTPC
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class EntityTPC1 : EntityTPC
    {
        public string DescriptionTPC1 { get; set; }
    }

    public class EntityTPC2 : EntityTPC
    {
        public string DescriptionTPC2 { get; set; }
    }
}