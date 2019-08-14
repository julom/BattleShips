using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace BattleShips.Web
{
    public static class ITempDataDictionaryExtensionMethods
    {
        public static void AddSerializedObject(this ITempDataDictionary tempDictionary, object objectToSerialize, string name)
        {
            var serializedVm = JsonConvert.SerializeObject(objectToSerialize);
            tempDictionary[name] = serializedVm;
        }

    }
}
