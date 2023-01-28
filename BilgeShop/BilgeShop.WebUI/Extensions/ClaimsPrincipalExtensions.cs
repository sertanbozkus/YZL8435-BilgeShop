using BilgeShop.Data.Enums;
using System.Security.Claims;

namespace BilgeShop.WebUI.Extensions
{
    //int x = rnd.Next(1, 5);  -> normal metot örneği
    //string metin = x.ToString(); -> extension metot örneği
    public static class ClaimsPrincipalExtensions
    {
      public static bool IsLogged(this ClaimsPrincipal user) // this -> parametreyi başa al
        {
            if (user.Claims.FirstOrDefault(x => x.Type == "id") != null)
                return true;
            else
                return false;
        }

        public static string GetFirstName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "firstName")?.Value;
        }
        
        public static string GetLastName(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "lastName")?.Value;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            if (user.Claims.FirstOrDefault(x => x.Type == "userType")?.Value == UserTypeEnum.Admin.ToString())
                return true;
            else
                return false;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

        }

        public static int GetId(this ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(x => x.Type == "id")?.Value);
        }
    }
}
