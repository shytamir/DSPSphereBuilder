using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DSPSphereBuilder.Feasibility
{
    public static class ProbeJson
    {
        public static T Read<T>(Stream input) =>
            (T)new DataContractJsonSerializer(typeof(T)).ReadObject(input);

        public static string Write<T>(T value)
        {
            using (var output = new MemoryStream())
            {
                new DataContractJsonSerializer(typeof(T)).WriteObject(output, value);
                return Encoding.UTF8.GetString(output.ToArray());
            }
        }
    }
}
