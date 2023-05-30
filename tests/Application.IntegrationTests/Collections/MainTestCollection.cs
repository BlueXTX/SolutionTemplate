using BlueXT.Application.IntegrationTests.Factories;

namespace BlueXT.Application.IntegrationTests.Collections;

[CollectionDefinition(nameof(MainTestCollection))]
public class MainTestCollection : ICollectionFixture<BlueXTApiFactory> { }
