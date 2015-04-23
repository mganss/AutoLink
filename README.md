# AutoLink

AutoLink is a .NET PCL to perform autolinking on texts, i.e. replace URIs with HTML links.

AutoLink is configurable:

- Configure URI prefixes (schemes) that trigger recognition of URIs. Default URI prefixes are:
  - `http://`
  - `https://`
  - `ftp://`
  - `mailto:`
  - `www.` (will be prepended with `http://` in link)
- Provide your own callback to render HTML based on a URI, e.g. to customize link text or add custom attributes

AutoLink assumes that the input text is raw text, not HTML.

AutoLink tries to exclude trailing punctuation (`!?,.:;`), closing parentheses, and closing brackets from the end of URIs while preserving parentheses that are part of the URI.

## Usage

```C#
var autolink = new AutoLink();
var text = @"(http://en.wikipedia.org/wiki/Link_(film)).";
var html = autolink.Link(text);
// -> (<a href="http://en.wikipedia.org/wiki/Link_(film)">http://en.wikipedia.org/wiki/Link_(film)</a>).
```
