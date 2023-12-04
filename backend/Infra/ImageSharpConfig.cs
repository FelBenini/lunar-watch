using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;

namespace lunarwatch.backend.Infra;

public static class ImageSharpConfig
{
  private static readonly List<ThumbSize> thumbSizes = new List<ThumbSize>() { new ThumbSize(1280, 720), new ThumbSize(1920, 1080), new ThumbSize(380, 380) };

  public static Task ConfigImageSharp(ImageCommandContext ctx)
  {
    if (ctx.Commands.Count == 0)
    {
      return Task.CompletedTask;
    }

    uint width = ctx.Parser.ParseValue<uint>(
    ctx.Commands.GetValueOrDefault(ResizeWebProcessor.Width),
    ctx.Culture);
    uint height = ctx.Parser.ParseValue<uint>(
    ctx.Commands.GetValueOrDefault(ResizeWebProcessor.Height),
    ctx.Culture);

    List<uint> allowedSizes = new List<uint> { 200, 400, 1200, 2400 };
    if (!thumbSizes.Any(x => x.height == height))
    {
      ctx.Commands.Remove(ResizeWebProcessor.Height);
      if (!allowedSizes.Any(x => x == width) || !thumbSizes.Any(x => x.width == width))
      {
        ctx.Commands.Remove(ResizeWebProcessor.Width);
      }
    }

    return Task.CompletedTask;
  }
}

public class ThumbSize
{
  public uint width { get; set; }
  public uint height { get; set; }

  public ThumbSize(uint w, uint h)
  {
    width = w;
    height = h;
  }
}