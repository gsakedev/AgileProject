namespace OrderManager.Domain.Constants
{
    public static class Roles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string RestaurantStaff = "RestaurantStaff";
        public const string DeliveryStaff = "DeliveryStaff";
        public const string Customer = "Customer";
        public static IEnumerable<string> GetAllRoles()
        {
            return typeof(Roles)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetValue(null)?.ToString() ?? string.Empty);
        }
    }
}
