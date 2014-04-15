namespace Z.ExtensionMethods.EF6.Examples
{
    public abstract class EntityTPT
    {
        public int ID { get; set; }
        public string Description { get; set; }
    }

    public class EntityTPT1 : EntityTPT
    {
        public string DescriptionTPT1 { get; set; }
    }

    public class EntityTPT2 : EntityTPT
    {
        public string DescriptionTPT2 { get; set; }
    }
}