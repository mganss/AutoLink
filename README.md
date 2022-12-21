# AutoLink

[![Version](https://img.shields.io/nuget/v/AutoLink.svg)](https://www.nuget.org/packages/AutoLink)

AutoLink is a .NET library to perform autolinking on texts, i.e. replace URIs with HTML links.

AutoLink is configurable:

- Configure URI prefixes (schemes) that trigger recognition of URIs. Default URI prefixes are:
  - `http://`
  - `https://`
  - `ftp://`
  - `mailto:`
  - `www.` (will be prepended with `http://` in link)
- Provide your own callback to render HTML based on a URI, e.g. to customize link text or add custom attributes

AutoLink assumes that the input text is raw text, not HTML. If you have HTML, consider using AutoLink as a post-processing step of [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer) ([example](https://github.com/mganss/HtmlSanitizer/blob/e4fadc972f3078a24dfc7e56af0799041c8edf3b/test/HtmlSanitizer.Tests/Tests.cs#L2169-L2191)).

AutoLink tries to exclude trailing punctuation (`!?,.:;`), closing parentheses, and closing brackets from the end of URIs while preserving parentheses that are part of the URI.

## Usage

```C#
var autolink = new AutoLink();
var text = @"Check it (http://x.org/wiki/Link_(film)).";
var html = autolink.Link(text);
// -> Check it (<a href="http://x.org/wiki/Link_(film)">http://x.org/wiki/Link_(film)</a>).
```
