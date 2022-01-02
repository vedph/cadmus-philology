using Cadmus.Core;
using Cadmus.Core.Config;
using Cadmus.General.Parts;
using Cadmus.Philology.Parts;
using Cadmus.Seed.General.Parts;
using Cadmus.Seed.Philology.Parts;
using Fusi.Microsoft.Extensions.Configuration.InMemoryJson;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit;

namespace Cadmus.Seed.Parts.Test
{
    static internal class TestHelper
    {
        static public string LoadResourceText(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            using StreamReader reader = new(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(
                    $"Cadmus.Seed.Philology.Parts.Test.Assets.{name}")!,
                Encoding.UTF8);
            return reader.ReadToEnd();
        }

        static public PartSeederFactory GetFactory()
        {
            // map
            TagAttributeToTypeMap map = new();
            map.Add(new[]
            {
                // Cadmus.Core
                typeof(StandardItemSortKeyBuilder).Assembly,
                // Cadmus.General.Parts
                typeof(NotePart).Assembly,
                // Cadmus.Philology.Parts
                typeof(ApparatusLayerFragment).Assembly
            });

            // container
            Container container = new();
            PartSeederFactory.ConfigureServices(
                container,
                new StandardPartTypeProvider(map),
                new[]
                {
                    // Cadmus.Seed.General.Parts
                    typeof(NotePartSeeder).Assembly,
                    // Cadmus.Seed.Philology.Parts
                    typeof(ApparatusLayerFragmentSeeder).Assembly
                });

            // config
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddInMemoryJson(LoadResourceText("SeedConfig.json"));
            var configuration = builder.Build();

            return new PartSeederFactory(container, configuration);
        }

        static public void AssertPartMetadata(IPart part)
        {
            Assert.NotNull(part.Id);
            Assert.NotNull(part.ItemId);
            Assert.NotNull(part.UserId);
            Assert.NotNull(part.CreatorId);
        }
    }
}
