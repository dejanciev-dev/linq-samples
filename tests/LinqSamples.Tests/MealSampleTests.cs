namespace LinqSamples.Tests;

public class MealSampleTests
{
    [Fact] public void Total_calories() => Assert.Equal(3350, MealSamples.TotalCalories());
    [Fact] public void Per_cuisine() => Assert.Equal(["Italian:2", "Japanese:2", "Mexican:2"], MealSamples.PerCuisine());
    [Fact] public void Quickest() => Assert.Equal("Taco", MealSamples.Quickest());
    [Fact] public void Slowest() => Assert.Equal("Sushi", MealSamples.Slowest());
    [Fact] public void Quick_meals() => Assert.Equal(["Burrito", "Pasta", "Taco"], MealSamples.QuickMeals());
    [Fact] public void Minutes_per_cuisine() => Assert.Equal(["Italian:65", "Japanese:80", "Mexican:40"], MealSamples.MinutesPerCuisine());
    [Fact] public void Lightest_per_cuisine() => Assert.Equal(["Italian:Pasta", "Japanese:Sushi", "Mexican:Taco"], MealSamples.LightestPerCuisine());
    [Fact] public void Average_calories() => Assert.Equal(558.3, MealSamples.AverageCalories());
    [Fact] public void Shareable() => Assert.Equal(["Pasta", "Pizza", "Taco"], MealSamples.Shareable());
    [Fact] public void By_cuisine_then_time() => Assert.Equal(["Pizza", "Pasta", "Sushi", "Ramen", "Burrito", "Taco"], MealSamples.ByCuisineThenTime());
}
