namespace LinqSamples.Tests;

public class TransferSampleTests
{
    [Fact] public void Net() => Assert.Equal(["Ann:-55", "Bob:55", "Cy:40", "Dee:-40"], TransferSamples.Net());
    [Fact] public void Richest_and_poorest() => Assert.Equal("Bob/Ann", TransferSamples.RichestAndPoorest());
    [Fact] public void Mutual() => Assert.Equal(["Ann-Bob", "Ann-Cy"], TransferSamples.Mutual());
    [Fact] public void Never_received() => Assert.Equal(["Dee"], TransferSamples.NeverReceived());
    [Fact] public void Sent() => Assert.Equal(["Ann:70", "Bob:35", "Cy:10", "Dee:40"], TransferSamples.Sent());
    [Fact] public void Largest_per_sender() => Assert.Equal(["Ann>Bob", "Bob>Cy", "Cy>Ann", "Dee>Bob"], TransferSamples.LargestPerSender());
    [Fact] public void Two_hop() => Assert.Equal(["Ann>Bob>Cy", "Bob>Cy>Ann", "Cy>Ann>Bob", "Bob>Ann>Cy", "Dee>Bob>Cy", "Dee>Bob>Ann"], TransferSamples.TwoHop());
    [Fact] public void Above_average() => Assert.Equal(["Ann>Bob:50", "Bob>Cy:30", "Dee>Bob:40"], TransferSamples.AboveAverage());
    [Fact] public void Partners() => Assert.Equal(["Ann:Bob,Cy", "Bob:Ann,Cy,Dee", "Cy:Ann,Bob", "Dee:Bob"], TransferSamples.Partners());
    [Fact] public void Running_total() => Assert.Equal([50, 80, 100, 110, 115, 155], TransferSamples.RunningTotal());
}
