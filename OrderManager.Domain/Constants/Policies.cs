using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Constants
{
    public static class Policies
    {
        public const string SuperAdminPolicy = "SuperAdminPolic";
        public const string AdminPolicy = "AdminPolicy";
        public const string RestaurantStaffPolicy = "RestaurantStaffPolicy";
        public const string DeliveryStaffPolicy = "DeliveryStaffPolicy";
        public const string CustomerPolicy = "CustomerPolicy";
        public static IEnumerable<string> GetAllPolicies()
        {
            return typeof(Policies)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(fi => fi.GetValue(null)?.ToString() ?? string.Empty);
        }
    }
}
