using VisitorHandler;
//AI_COMMENTS
namespace Graphics;
using SkiaSharp;
/// <summary>
/// This class can be used to draw a graph of relationships.
/// </summary>
public class RelationshipDrawer
{
    /// <summary>
    /// The width of the output image.
    /// </summary>
    private const int Width = 1000;
    /// <summary>
    /// The height of the output image.
    /// </summary>
    private const int Height = 1000;
    /// <summary>
    /// The list of relationships to be drawn.
    /// </summary>
    private readonly List<Relationship> _relationships;
    /// <summary>
    /// The image to be drawn to.
    /// </summary>
    private readonly SKBitmap _bitmap;
    /// <summary>
    /// The canvas to be drawn on.
    /// </summary>
    private readonly SKCanvas _canvas;
    /// <summary>
    /// The paint to be used for text.
    /// </summary>
    private readonly SKPaint _textPaint;
    /// <summary>
    /// The paint to be used for rectangles.
    /// </summary>
    private readonly SKPaint _rectanglePaint;
    /// <summary>
    /// The paint to be used for friendly relationships.
    /// </summary>
    private readonly SKPaint _friendPaint;
    /// <summary>
    /// The paint to be used for unfriendly relationships.
    /// </summary>
    private readonly SKPaint _enemyPaint;
    /// <summary>
    /// Initializes the drawer with the specified relationships.
    /// </summary>
    /// <param name="relationships">The relationships to be drawn.</param>
    public RelationshipDrawer(List<Relationship> relationships) 
    {
        _relationships = relationships;
        _bitmap = new SKBitmap(Width, Height);
        _canvas = new SKCanvas(_bitmap);    
        _canvas.Clear(SKColor.Parse("#fff"));
        _textPaint = new ()
        {
            Color = SKColors.Black,
            TextSize = 15,
            Typeface = SKTypeface.FromFamilyName("Times New Roman"),
        };
        _rectanglePaint = new()
        {
            Color = SKColors.LightBlue
        };
        _friendPaint = new()
        {
            Color = SKColors.Green
        };
        _enemyPaint = new SKPaint()
        {
            Color = SKColors.Red
        };
    }

    /// <summary>
    /// The width of the rectangles used to draw names.
    /// </summary>
    private const int RectWidth = 250;
    /// <summary>
    /// The height of the rectangles used to draw names.
    /// </summary>
    private const int RectHeight = 20;
    
    /// <summary>
    /// Draws a rectangle with the specified text centered in it.
    /// </summary>
    /// <param name="text">The text to be drawn.</param>
    /// <param name="x">The x-coordinate of the center of the rectangle.</param>
    /// <param name="y">The y-coordinate of the center of the rectangle.</param>
    /// <returns>The center of the rectangle.</returns>
    private SKPoint DrawRectangleWithText(string text, int x, int y)
    {
        SKRect textRect = new SKRect(x - RectWidth / 2, y - RectHeight / 2, x + RectWidth / 2, y + RectHeight / 2); // coordinates locate the center of the rectangle
        _canvas.DrawRect(textRect, _rectanglePaint);
        float textWidth = _textPaint.MeasureText(text);
        _canvas.DrawText(text, (textRect.Left + textRect.Right) / 2 - textWidth / 2, textRect.Top + _textPaint.TextSize, _textPaint);// put text in the middle
        return new SKPoint(x, y);
    }
    /// <summary>
    /// A dictionary from names to coordinates.
    /// </summary>
    private readonly Dictionary<string, SKPoint> _nameToPoint = new Dictionary<string, SKPoint>();

    /// <summary>
    /// Draws the rectangles containing names.
    /// </summary>
    private void DrawRects()
    {
        HashSet<string> namesSet = new HashSet<string>();
        foreach (Relationship relationship in _relationships)
        {
            namesSet.Add(relationship.first.GetField("label")[1..^1]);
            namesSet.Add(relationship.second.GetField("label")[1..^1]);
        }
        int values = 0;
        int y = RectHeight * 3;
        List<string> names = [..namesSet];
        
        while (values < names.Count)
        {
            for (int x = RectWidth; x + RectWidth < Width && values < names.Count; x += (int)(RectWidth * (1.2)))
            {
                _nameToPoint[names[values]] = DrawRectangleWithText(names[values], x, y);
                values++;
            }
            y += RectHeight * 4;
        }
    }

    /// <summary>
    /// Draws a line between two points. The line is drawn as two lines with a random point in between.
    /// </summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <param name="paint">The paint to be used for the line.</param>
    private void GetPath(SKPoint a, SKPoint b, SKPaint paint)
    {
        Random rnd = new ();
        double dx = Math.Abs(a.X - b.X);
        double dy = Math.Abs(a.Y - b.Y); // here I generate a random point so that edges are not straight and you can see them
        double interX = rnd.NextDouble() * (dx + 15);
        double interY = rnd.NextDouble() * (dy + 15);
        SKPoint interPoint = new SKPoint(Math.Min(a.X, b.X) + (float) interX, Math.Min(a.Y, b.Y) + (float) interY); 
        _canvas.DrawLine(a, interPoint, paint);
        _canvas.DrawLine(interPoint, b, paint);
    }
    /// <summary>
    /// Draws the edges between the rectangles.
    /// </summary>
    private void DrawEdges()
    {
        foreach (Relationship relationship in _relationships)
        {
            string label1 = relationship.first.GetField("label")[1..^1];
            string label2 = relationship.second.GetField("label")[1..^1];
            if (relationship.type == "дружба")
            {
                GetPath(_nameToPoint[label1], _nameToPoint[label2], _friendPaint);
            }
            else if (relationship.type == "недоверие")
            {
                GetPath(_nameToPoint[label1], _nameToPoint[label2],_enemyPaint);
            }
        }
    }

    /// <summary>
    /// Draws the image.
    /// </summary>
    public void Draw()
    {
        DrawRects();
        DrawEdges();
    }
    /// <summary>
    /// Saves the image to the specified path.
    /// </summary>
    /// <param name="outputPath">The path to save the image to.</param>
    public void SaveImage(string outputPath)
    { 
        FileStream stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
        SKImage image = SKImage.FromBitmap(_bitmap); 
        SKData encodedImage = image.Encode(); 
        encodedImage.SaveTo(stream);
        stream.Dispose();
    }
}
