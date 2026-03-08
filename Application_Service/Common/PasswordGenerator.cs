namespace Application_Service.Common
{
    public static class PasswordGenerator
    {
        public static string Generate()
        {
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
