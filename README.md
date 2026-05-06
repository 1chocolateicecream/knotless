# knotless

> chaos refined into order. everything in its place, all in lowercase.

a minimal desktop application built with c# and avalonia ui. it weaves your messy workspace (like your desktop or downloads folder) into a structured tapestry.

no shouting, no caps, just clean mint and creamy yellow accents.

## what it does

- **smart weaving**: automatically sorts files based on their extensions and creation dates (e.g., nesting them neatly into `images/2024/april/`).
- **the black hole**: a designated folder (`blackhole` by default) where old, forgotten files simply vanish into the void after 24 hours.
- **responsive**: does all the heavy lifting in the background so the interface never freezes.
- **persistent**: remembers your target folder and settings between sessions.

## how to use it

make sure you have the .net sdk installed on your machine.

1. clone this repository:
   ```bash
   git clone https://github.com/1chocolateicecream/knotless.git
   cd knotless
   ```

2. run the app:
   ```bash
   dotnet run
   ```

3. click **select folder** to choose where the magic should happen.
4. hit **start weaving** and watch the chaos disappear.

## configuration

after your first run, a `settings.json` file will keep track of your choices. you can open it to tweak your sorting rules, add new extensions, or adjust how hungry the black hole is.

```json
{
  "black_hole": {
    "enabled": true,
    "folder": "blackhole",
    "max_age_hours": 24
  }
}
```

stay lowkey.