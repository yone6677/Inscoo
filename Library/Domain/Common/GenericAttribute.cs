
namespace Domain.Common
{
    public class GenericAttribute : BaseEntity
    {
        public string KeyGroup { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Sequence { get; set; }
    }
}
