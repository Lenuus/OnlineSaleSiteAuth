using System;
using System.Text;

namespace OnlineSaleSiteAuth.Common.Helpers
{
    public class CouponCreaterHelper
    {
        private static readonly Random _random = new Random();

        public static string GenerateCouponCode(int length=12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var couponCodeBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                couponCodeBuilder.Append(chars[_random.Next(chars.Length)]);
            }

            return couponCodeBuilder.ToString();
        }
    }
}
