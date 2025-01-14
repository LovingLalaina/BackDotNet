namespace back_dotnet.Utils
{
    public static class TypeOrmErrorCode
    {
        public static readonly string DUPLICATED_FIELD = "23505";
        public static readonly string UUID_INVALID = "22P02";
        public static readonly string VIOLATE_FOREIGN_KEY = "23503";
        public static readonly string VIOLATE_MIN_LENGTH = "23514";
        public static readonly string VIOLATE_NOT_NULL_CONSTRAINT = "23502";
    }
}
