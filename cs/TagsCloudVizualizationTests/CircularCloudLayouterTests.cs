using System.Drawing;
using TagsCloudVizualization;
using FluentAssertions;
using TagsCloudVizualization.Utility;

namespace TagsCloudVizualizationTests;

public class CircularCloudLayouterTests
{
    private CircularCloudLayouter sut;
    private Point center = new(720, 720);

    [SetUp]
    public void Setup()
    {
        sut = new CircularCloudLayouter(center);
    }
    
    [Test]
    public void Constructor_ShouldNotThrow()
    {
        Assert.DoesNotThrow(() => new CircularCloudLayouter(center));
    }

    [TestCase(-4, 16, TestName = "with negative rectangle width")]
    [TestCase(77, -8, TestName = "with negative rectangle height")]
    public void PutNextRectangle_InvalidSize_ThrowsArgumentException(int rectangleWidth, int rectangleHeight)
    {
        Assert.Throws<ArgumentException>(() => sut.PutNextRectangle(new Size(rectangleWidth, rectangleHeight)));
    }
    
    [Test]
    public void PutNextRectangle_ShouldReturnCorrectRectangleSize()
    {
        var rectangle = sut.PutNextRectangle(new Size(8, 8));
        
        rectangle.Size.Should().Be(new Size(8, 8));
    }

    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldHaveCenterAtLayoutCenter()
    {
        var rectangle = sut.PutNextRectangle(new Size(8, 8));
        
        var expectedRectangleCenter = new Point(rectangle.Left + rectangle.Width / 2, rectangle.Top + rectangle.Height / 2);
        expectedRectangleCenter.Should().Be(center);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_SecondRectangleCenterShouldNotBeAtLayoutCenter()
    {
        sut.PutNextRectangle(new Size(8, 8));
        
        var secondRectangle = sut.PutNextRectangle(new Size(6, 6));
        
        var expectedRectangleCenter = new Point(
            secondRectangle.Left + secondRectangle.Width / 2,
            secondRectangle.Top + secondRectangle.Height / 2);

        expectedRectangleCenter.Should().NotBe(center);
    }

    [Test]
    public void PutNextRectangle_TwoRectangles_ShouldNotIntersect()
    {
        var firstRectangle = sut.PutNextRectangle(new Size(8, 8));
        
        var secondRectangle = sut.PutNextRectangle(new Size(77, 77));
        
        secondRectangle.IntersectsWith(firstRectangle).Should().BeFalse();
    }
    
    [TearDown]
    public void SaveImageWhenTestFails()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            SaveFailedTestImage();
        }
    }

    private void SaveFailedTestImage()
    {
        var pathToFailedTests = @"..\..\..\FailedTests";
        CreateDirectoryIfNotExists(pathToFailedTests);

        var image = Visualizer.VisualizeRectangles(sut.Rectangles, 500, 500);
        var fileName = $"{TestContext.CurrentContext.Test.Name}.png";
        Visualizer.SaveBitmap(image, fileName, pathToFailedTests);
    }

    private void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    
}
