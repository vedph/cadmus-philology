﻿using Cadmus.Core;
using Cadmus.General.Parts;
using Cadmus.Philology.Parts;
using Cadmus.Seed.General.Parts;
using Cadmus.Seed.Parts.Test;
using Fusi.Tools.Configuration;
using System;
using System.Reflection;
using Xunit;

namespace Cadmus.Seed.Philology.Parts.Test;

// this is to test the layer part with a fragment coming from this assembly

public sealed class TokenTextLayerPartSeederTest
{
    private static readonly PartSeederFactory _factory =
            TestHelper.GetFactory();
    private static readonly SeedOptions _seedOptions =
        _factory.GetSeedOptions();
    private static readonly IItem _item =
        _factory.GetItemSeeder().GetItem(1, "facet");

    [Fact]
    public void TypeHasTagAttribute()
    {
        Type t = typeof(TokenTextLayerPartSeeder);
        TagAttribute? attr = t.GetTypeInfo().GetCustomAttribute<TagAttribute>();
        Assert.NotNull(attr);
        Assert.Equal("seed.it.vedph.token-text-layer", attr!.Tag);
    }

    [Fact]
    public void Seed_NoOptions_Null()
    {
        TokenTextLayerPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);

        IPart? part = seeder.GetPart(_item, null, _factory);

        Assert.Null(part);
    }

    [Fact]
    public void Seed_InvalidOptions_Null()
    {
        TokenTextLayerPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);
        seeder.Configure(new TokenTextLayerPartSeederOptions
        {
            MaxFragmentCount = 0
        });

        IPart? part = seeder.GetPart(_item, null, _factory);

        Assert.Null(part);
    }

    [Fact]
    public void Seed_OptionsNoText_Null()
    {
        TokenTextLayerPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);
        seeder.Configure(new TokenTextLayerPartSeederOptions
        {
            MaxFragmentCount = 3
        });

        IPart? part = seeder.GetPart(_item, "fr.it.vedph.quotations", _factory);

        Assert.Null(part);
    }

    [Fact]
    public void Seed_Options_Ok()
    {
        TokenTextLayerPartSeeder seeder = new();
        seeder.SetSeedOptions(_seedOptions);
        seeder.Configure(new TokenTextLayerPartSeederOptions
        {
            MaxFragmentCount = 3
        });

        // item with text
        IItem item = _factory.GetItemSeeder().GetItem(1, "facet");
        TokenTextPartSeeder textSeeder = new();
        textSeeder.SetSeedOptions(_seedOptions);
        item.Parts.Add(textSeeder.GetPart(_item, null, _factory)!);

        IPart? part = seeder.GetPart(item, "fr.it.vedph.quotations", _factory);

        Assert.NotNull(part);

        TokenTextLayerPart<QuotationsLayerFragment>? lp =
            part as TokenTextLayerPart<QuotationsLayerFragment>;
        Assert.NotNull(lp);
        Assert.NotEmpty(lp!.Fragments);
    }
}
