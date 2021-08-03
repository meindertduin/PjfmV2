using System;
using System.Linq;

namespace Pjfm.Common.Extensions
{
    public static class Helpers
    {
        public  static string RandomString(int lenght)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var rand = new Random();

            return new String(Enumerable.Repeat(chars, lenght)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }
    }
}