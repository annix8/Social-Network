namespace SocialNetwork.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class TempDataExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[GlobalConstants.SuccessMessageKey] = message;
        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[GlobalConstants.ErrorMessageKey] = message;
        }

        public static string ShowSuccessMessage(this ITempDataDictionary tempData)
        {
            if (tempData.ContainsKey(GlobalConstants.SuccessMessageKey))
            {
                return tempData[GlobalConstants.SuccessMessageKey].ToString();
            }

            return "";
        }

        public static string ShowErrorMessage(this ITempDataDictionary tempData)
        {
            if (tempData.ContainsKey(GlobalConstants.ErrorMessageKey))
            {
                return tempData[GlobalConstants.ErrorMessageKey].ToString();
            }

            return "";
        }
    }
}
