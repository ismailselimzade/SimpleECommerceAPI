namespace SimpleECommerceAPI.Exceptions
{
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException(string entityName, string fieldName, string fieldValue) : base($"{entityName} with {fieldName} '{fieldValue}' already exists")
        {}
    }
}
