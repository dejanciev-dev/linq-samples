namespace LinqSamples.Tests;

public class VetSampleTests
{
    [Fact] public void Total_cost() => Assert.Equal(270, VetSamples.TotalCost());
    [Fact] public void Cost_per_species() => Assert.Equal(["Cat:110", "Dog:160"], VetSamples.CostPerSpecies());
    [Fact] public void Repeat_pets() => Assert.Equal(["Rex", "Tom"], VetSamples.RepeatPets());
    [Fact] public void Top_spender() => Assert.Equal("Rex", VetSamples.TopSpender());
    [Fact] public void Vets() => Assert.Equal(["Kim", "Lee"], VetSamples.Vets());
    [Fact] public void Average_cost() => Assert.Equal(45.0, VetSamples.AverageCost());
    [Fact] public void Visits_per_vet() => Assert.Equal(["Kim:4", "Lee:2"], VetSamples.VisitsPerVet());
    [Fact] public void Seen_by_both() => Assert.Equal(["Rex", "Tom"], VetSamples.SeenByBoth());
    [Fact] public void Expensive_visits() => Assert.Equal(3, VetSamples.ExpensiveVisits());
    [Fact] public void Cheapest() => Assert.Equal("Tom", VetSamples.Cheapest());
}

public class GardenSampleTests
{
    [Fact] public void Tallest() => Assert.Equal("Rose", GardenSamples.Tallest());
    [Fact] public void Total_height() => Assert.Equal(260, GardenSamples.TotalHeight());
    [Fact] public void Beds_by_water() => Assert.Equal(["B:9", "A:5", "C:4"], GardenSamples.BedsByWater());
    [Fact] public void Thirsty() => Assert.Equal(["Fern", "Mint"], GardenSamples.Thirsty());
    [Fact] public void Short() => Assert.Equal(["Mint", "Sage", "Tulip"], GardenSamples.Short());
    [Fact] public void Tallest_per_bed() => Assert.Equal(["A:Rose", "B:Fern", "C:Basil"], GardenSamples.TallestPerBed());
    [Fact] public void Any_over_100() => Assert.False(GardenSamples.AnyOver100());
    [Fact] public void Three_shortest() => Assert.Equal(["Mint", "Tulip", "Sage"], GardenSamples.ThreeShortest());
    [Fact] public void Lowest_water() => Assert.Equal("Sage", GardenSamples.LowestWater());
    [Fact] public void Chunk_sizes() => Assert.Equal([4, 2], GardenSamples.ChunkSizes());
}
