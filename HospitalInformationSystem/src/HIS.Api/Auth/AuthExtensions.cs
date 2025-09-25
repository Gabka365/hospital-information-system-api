namespace HIS.Api.Auth
{
    public static class AuthExtensions
    {
        public static Guid GetUserId(this HttpContext context)
        {
            var guidString = context.User.Claims.FirstOrDefault(x => x.Type == "UserId").Value;
            
            if (Guid.TryParse(guidString, out Guid result))
            {
                return result;  
            }

            return Guid.Empty;
        }
    }
}
