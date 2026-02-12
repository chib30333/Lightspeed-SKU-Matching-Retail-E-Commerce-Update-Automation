using System.IO;
using System.Text.Json;
using Dupont_Price_Lists.Models;

namespace Dupont_Price_Lists.Services.Profiles
{
    public static class MappingProfileStore
    {
        public static void Save(string path, MappingProfile profile)
        {
            var json = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static MappingProfile Load(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<MappingProfile>(json) ?? new MappingProfile();
        }
    }
}
