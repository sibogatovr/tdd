using System.Drawing;
using TagsCloudVizualization;
using FluentAssertions;
using TagsCloudVizualization.Utility;

namespace TagsCloudVizualizationTests;

public class SpiralTests
{
    private Point center = new(250, 250);

    [Test]
    public void Constructor_ShouldNotThrow_WithValidArguments()
    {
        Action constructorAction = () => new Spiral(center, 0.1, 0.2);
        constructorAction.Should().NotThrow();
    }

    [TestCase(0, 0, TestName = "Constructor_ZeroStep_ThrowsArgumentException")]
    [TestCase(-0.1, 0.2, TestName = "Constructor_NegativeAngleStep_ThrowsArgumentException")]
    [TestCase(0.1, -0.2, TestName = "Constructor_NegativeRadiusStep_ThrowsArgumentException")]
    public void Constructor_InvalidStepValues_ThrowsArgumentException(double angleStep, double radiusStep)
    {
        Action constructorAction = () => new Spiral(center, angleStep, radiusStep);
        constructorAction.Should().Throw<ArgumentException>();
    }
}